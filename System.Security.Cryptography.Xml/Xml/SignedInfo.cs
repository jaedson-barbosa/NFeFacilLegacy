// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
    public class SignedInfo
    {
        private string _id;
        private string _canonicalizationMethod;
        private string _signatureMethod;
        private string _signatureLength;
        private ArrayList _references;
        private SignedXml _signedXml = null;
        private Transform _canonicalizationMethodTransform = null;

        internal SignedXml SignedXml
        {
            get { return _signedXml; }
            set { _signedXml = value; }
        }

        public SignedInfo()
        {
            _references = new ArrayList();
        }

        //
        // public properties
        //

        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
            }
        }

        public string CanonicalizationMethod
        {
            get
            {
                // Default the canonicalization method to C14N
                if (_canonicalizationMethod == null)
                    return SignedXml.XmlDsigC14NTransformUrl;
                return _canonicalizationMethod;
            }
            set
            {
                _canonicalizationMethod = value;
            }
        }

        public Transform CanonicalizationMethodObject
        {
            get
            {
                if (_canonicalizationMethodTransform == null)
                {
                    _canonicalizationMethodTransform = CryptoHelpers.CreateFromName(CanonicalizationMethod) as Transform;
                    _canonicalizationMethodTransform.SignedXml = SignedXml;
                    _canonicalizationMethodTransform.Reference = null;
                }
                return _canonicalizationMethodTransform;
            }
        }

        public string SignatureMethod
        {
            get { return _signatureMethod; }
            set
            {
                _signatureMethod = value;
            }
        }

        public string SignatureLength
        {
            get { return _signatureLength; }
            set
            {
                _signatureLength = value;
            }
        }

        public ArrayList References
        {
            get { return _references; }
        }

        //
        // public methods
        //

        public XmlElement GetXml()
        {
            XmlDocument document = new XmlDocument()
            {
                PreserveWhitespace = true
            };
            // Create the root element
            XmlElement signedInfoElement = document.CreateElement("SignedInfo", SignedXml.XmlDsigNamespaceUrl);
            if (!string.IsNullOrEmpty(_id))
                signedInfoElement.SetAttribute("Id", _id);

            // Add the canonicalization method, defaults to SignedXml.XmlDsigNamespaceUrl
            XmlElement canonicalizationMethodElement = CanonicalizationMethodObject.GetXml(document, "CanonicalizationMethod");
            signedInfoElement.AppendChild(canonicalizationMethodElement);

            // Add the signature method
            XmlElement signatureMethodElement = document.CreateElement("SignatureMethod", SignedXml.XmlDsigNamespaceUrl);
            signatureMethodElement.SetAttribute("Algorithm", _signatureMethod);
            // Add HMACOutputLength tag if we have one
            if (_signatureLength != null)
            {
                XmlElement hmacLengthElement = document.CreateElement(null, "HMACOutputLength", SignedXml.XmlDsigNamespaceUrl);
                XmlText outputLength = document.CreateTextNode(_signatureLength);
                hmacLengthElement.AppendChild(outputLength);
                signatureMethodElement.AppendChild(hmacLengthElement);
            }

            signedInfoElement.AppendChild(signatureMethodElement);

            // Add the references
            for (int i = 0; i < _references.Count; ++i)
            {
                Reference reference = (Reference)_references[i];
                signedInfoElement.AppendChild(reference.GetXml(document));
            }

            return signedInfoElement;
        }

        public void AddReference(Reference reference)
        {
            if (reference == null)
                throw new ArgumentNullException(nameof(reference));

            reference.SignedXml = SignedXml;
            _references.Add(reference);
        }
    }
}
