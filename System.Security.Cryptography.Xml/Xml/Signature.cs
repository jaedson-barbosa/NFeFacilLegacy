// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Security.Cryptography.X509Certificates;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
    public class Signature
    {
        private string _id;
        private string _canonicalizationMethod;
        private string _signatureMethod;
        private string _signatureLength;
        private Reference _reference;
        private Transform _canonicalizationMethodTransform = null;
        private byte[] _signatureValue;
        private X509Certificate2 _keyInfo;
        private SignedXml _signedXml = null;

        internal SignedXml SignedXml
        {
            get { return _signedXml; }
            set { _signedXml = value; }
        }

        //
        // public properties
        //

        public byte[] SignatureValue
        {
            get { return _signatureValue; }
            set { _signatureValue = value; }
        }

        public X509Certificate2 KeyInfo
        {
            get => _keyInfo;
            set => _keyInfo = value;
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

        public Reference Reference
        {
            get { return _reference; }
            set => _reference = value;
        }

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
            Reference reference = _reference;
            signedInfoElement.AppendChild(reference.GetXml(document));

            return signedInfoElement;
        }
    }
}

