// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
    public sealed class SignedXml
    {
        private Signature m_signature;

        private XmlDocument _containingDocument = null;

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

        public SignedXml(XmlElement element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            _containingDocument = element?.OwnerDocument;
            _context = element;
            m_signature = new Signature();

            _safeCanonicalizationMethods = new Collection<string>(new List<string> { XmlDsigC14NTransformUrl });
        }

        //
        // public properties
        //

        public Signature Signature
        {
            get { return m_signature; }
        }

        public RSA Key { get; set; }

        //
        // public methods
        //

        public void AddReference(Reference reference)
        {
            m_signature.Reference = reference;
        }

        public void ComputeSignature()
        {
            BuildDigestedReferences();

            // Load the key
            m_signature.SignatureMethod = XmlDsigRSASHA1Url;

            // See if there is a signature description class defined in the Config file
            byte[] hashvalue = GetC14NDigest(SHA1.Create());
            var asymmetricSignatureFormatter = new SignatureFormatter(Key, HashAlgorithmName.SHA1);
            m_signature.SignatureValue = asymmetricSignatureFormatter.SignHash(hashvalue);
        }


        //
        // private methods
        //

        private byte[] GetC14NDigest(HashAlgorithm hash)
        {
            bool isKeyedHashAlgorithm = hash is KeyedHashAlgorithm;
            string baseUri = _containingDocument?.BaseURI;
            XmlDocument doc = Utils.PreProcessElementInput(m_signature.GetXml());

            // Add non default namespaces in scope
            CanonicalXmlNodeList namespaces = (_context == null ? null : Utils.GetPropagatedAttributes(_context));
            Utils.AddNamespaces(doc.DocumentElement, namespaces);

            Transform c14nMethodTransform = m_signature.CanonicalizationMethodObject;

            c14nMethodTransform.LoadInput(doc);
            return c14nMethodTransform.GetDigestedOutput(hash);
        }

        private void BuildDigestedReferences()
        {
            // Default the DigestMethod and Canonicalization
            Reference reference = m_signature.Reference;

            CanonicalXmlNodeList nodeList = new CanonicalXmlNodeList();
            // If no DigestMethod has yet been set, default it to sha1
            if (reference.DigestMethod == null)
                reference.DigestMethod = XmlDsigSHA1Url;

            reference.UpdateHashValue(_containingDocument, nodeList);
        }
    }
}
