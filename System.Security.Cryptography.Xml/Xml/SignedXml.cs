// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
    public sealed class SignedXml
    {
        private Signature m_signature;

        private AsymmetricAlgorithm _signingKey;
        private XmlDocument _containingDocument = null;

        private bool[] _refProcessed = null;
        private int[] _refLevelCache = null;

        internal XmlElement _context = null;

        private Collection<string> _safeCanonicalizationMethods;

        //
        // public constant Url identifiers most frequently used within the XML Signature classes
        //

        public const string XmlDsigNamespaceUrl = "http://www.w3.org/2000/09/xmldsig#";

        public const string XmlDsigSHA1Url = "http://www.w3.org/2000/09/xmldsig#sha1";
        public const string XmlDsigDSAUrl = "http://www.w3.org/2000/09/xmldsig#dsa-sha1";
        public const string XmlDsigRSASHA1Url = "http://www.w3.org/2000/09/xmldsig#rsa-sha1";

        public const string XmlDsigC14NTransformUrl = "http://www.w3.org/TR/2001/REC-xml-c14n-20010315";
        public const string XmlDsigEnvelopedSignatureTransformUrl = "http://www.w3.org/2000/09/xmldsig#enveloped-signature";

        //
        // public constructors
        //

        public SignedXml(XmlDocument document)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));
            Initialize(document.DocumentElement);
        }

        public SignedXml(XmlElement elem)
        {
            if (elem == null)
                throw new ArgumentNullException(nameof(elem));
            Initialize(elem);
        }

        private void Initialize(XmlElement element)
        {
            _containingDocument = (element == null ? null : element.OwnerDocument);
            _context = element;
            m_signature = new Signature();
            m_signature.SignedXml = this;
            m_signature.SignedInfo = new SignedInfo();
            _signingKey = null;

            _safeCanonicalizationMethods = new Collection<string>(new List<string> { XmlDsigC14NTransformUrl });
        }

        //
        // public properties
        //

        public AsymmetricAlgorithm SigningKey
        {
            get { return _signingKey; }
            set { _signingKey = value; }
        }

        public Signature Signature
        {
            get { return m_signature; }
        }

        public SignedInfo SignedInfo
        {
            get { return m_signature.SignedInfo; }
        }

        public KeyInfo KeyInfo
        {
            get { return m_signature.KeyInfo; }
            set { m_signature.KeyInfo = value; }
        }

        //
        // public methods
        //

        public void AddReference(Reference reference)
        {
            m_signature.SignedInfo.AddReference(reference);
        }

        public void ComputeSignature()
        {
            BuildDigestedReferences();

            // Load the key
            AsymmetricAlgorithm key = SigningKey;

            // Check the signature algorithm associated with the key so that we can accordingly set the signature method
            if (SignedInfo.SignatureMethod == null)
            {
                if (key is RSA)
                {
                    // Default to RSA-SHA1
                    if (SignedInfo.SignatureMethod == null)
                        SignedInfo.SignatureMethod = XmlDsigRSASHA1Url;
                }
                else
                {
                    throw new CryptographicException();
                }
            }

            // See if there is a signature description class defined in the Config file
            RSASignatureDescription signatureDescription = CryptoHelpers.CreateFromName(SignedInfo.SignatureMethod) as RSASignatureDescription;
            HashAlgorithm hashAlg = signatureDescription.CreateDigest();
            byte[] hashvalue = GetC14NDigest(hashAlg);
            RSAPKCS1SignatureFormatter asymmetricSignatureFormatter = signatureDescription.CreateFormatter(key);
            m_signature.SignatureValue = asymmetricSignatureFormatter.CreateSignature(hashvalue);
        }


        //
        // private methods
        //

        private byte[] GetC14NDigest(HashAlgorithm hash)
        {
            bool isKeyedHashAlgorithm = hash is KeyedHashAlgorithm;
            string baseUri = (_containingDocument == null ? null : _containingDocument.BaseURI);
            XmlDocument doc = Utils.PreProcessElementInput(SignedInfo.GetXml());

            // Add non default namespaces in scope
            CanonicalXmlNodeList namespaces = (_context == null ? null : Utils.GetPropagatedAttributes(_context));
            Utils.AddNamespaces(doc.DocumentElement, namespaces);

            Transform c14nMethodTransform = SignedInfo.CanonicalizationMethodObject;

            c14nMethodTransform.LoadInput(doc);
            return c14nMethodTransform.GetDigestedOutput(hash);
        }

        private int GetReferenceLevel(int index, ArrayList references)
        {
            if (_refProcessed[index]) return _refLevelCache[index];
            _refProcessed[index] = true;
            Reference reference = (Reference)references[index];
            if (reference.Uri == null || reference.Uri.Length == 0 || (reference.Uri.Length > 0 && reference.Uri[0] != '#'))
            {
                _refLevelCache[index] = 0;
                return 0;
            }
            if (reference.Uri.Length > 0 && reference.Uri[0] == '#')
            {
                string idref = Utils.ExtractIdFromLocalUri(reference.Uri);
                if (idref == "xpointer(/)")
                {
                    _refLevelCache[index] = 0;
                    return 0;
                }
                // Then the reference points to an object tag
                _refLevelCache[index] = 0;
                return 0;
            }
            // Malformed reference
            throw new CryptographicException();
        }

        private class ReferenceLevelSortOrder : IComparer
        {
            private ArrayList _references = null;
            public ReferenceLevelSortOrder() { }

            public ArrayList References
            {
                get { return _references; }
                set { _references = value; }
            }

            public int Compare(object a, object b)
            {
                Reference referenceA = a as Reference;
                Reference referenceB = b as Reference;

                // Get the indexes
                int iIndexA = 0;
                int iIndexB = 0;
                int i = 0;
                foreach (Reference reference in References)
                {
                    if (reference == referenceA) iIndexA = i;
                    if (reference == referenceB) iIndexB = i;
                    i++;
                }

                int iLevelA = referenceA.SignedXml.GetReferenceLevel(iIndexA, References);
                int iLevelB = referenceB.SignedXml.GetReferenceLevel(iIndexB, References);
                return iLevelA.CompareTo(iLevelB);
            }
        }

        private void BuildDigestedReferences()
        {
            // Default the DigestMethod and Canonicalization
            ArrayList references = SignedInfo.References;
            // Reset the cache
            _refProcessed = new bool[references.Count];
            _refLevelCache = new int[references.Count];

            ReferenceLevelSortOrder sortOrder = new ReferenceLevelSortOrder();
            sortOrder.References = references;
            // Don't alter the order of the references array list
            ArrayList sortedReferences = new ArrayList();
            foreach (Reference reference in references)
            {
                sortedReferences.Add(reference);
            }
            sortedReferences.Sort(sortOrder);

            CanonicalXmlNodeList nodeList = new CanonicalXmlNodeList();
            foreach (Reference reference in sortedReferences)
            {
                // If no DigestMethod has yet been set, default it to sha1
                if (reference.DigestMethod == null)
                    reference.DigestMethod = XmlDsigSHA1Url;

                reference.UpdateHashValue(_containingDocument, nodeList);
            }
        }
    }
}
