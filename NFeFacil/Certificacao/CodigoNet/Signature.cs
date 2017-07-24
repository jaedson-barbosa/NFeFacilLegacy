// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Xml;

namespace System.Security.Cryptography.Xml
{
    public class Signature
    {
        private string _signatureMethod;
        private Reference _reference;
        private Transform _canonicalizationMethodTransform = null;
        private byte[] _signatureValue;

        //
        // public properties
        //

        public byte[] SignatureValue
        {
            get { return _signatureValue; }
            set { _signatureValue = value; }
        }

        public string CanonicalizationMethod =>
                // Default the canonicalization method to C14N
                SignedXml.XmlDsigC14NTransformUrl;

        public Transform CanonicalizationMethodObject
        {
            get
            {
                if (_canonicalizationMethodTransform == null)
                {
                    switch (CanonicalizationMethod)
                    {
                        case "http://www.w3.org/TR/2001/REC-xml-c14n-20010315":
                            _canonicalizationMethodTransform = new XmlDsigC14NTransform();
                            break;
                        case "http://www.w3.org/2000/09/xmldsig#enveloped-signature":
                            _canonicalizationMethodTransform = new XmlDsigEnvelopedSignatureTransform();
                            break;
                        default:
                            throw new ArgumentException();
                    }
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

            // Add the canonicalization method, defaults to SignedXml.XmlDsigNamespaceUrl
            XmlElement canonicalizationMethodElement = CanonicalizationMethodObject.GetXml(document, "CanonicalizationMethod");
            signedInfoElement.AppendChild(canonicalizationMethodElement);

            // Add the signature method
            XmlElement signatureMethodElement = document.CreateElement("SignatureMethod", SignedXml.XmlDsigNamespaceUrl);
            signatureMethodElement.SetAttribute("Algorithm", _signatureMethod);

            signedInfoElement.AppendChild(signatureMethodElement);

            // Add the references
            Reference reference = _reference;
            signedInfoElement.AppendChild(reference.GetXml(document));

            return signedInfoElement;
        }
    }
}

