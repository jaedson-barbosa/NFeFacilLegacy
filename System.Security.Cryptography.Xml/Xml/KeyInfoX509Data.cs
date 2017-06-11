// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Security.Cryptography.X509Certificates;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
    public class KeyInfoX509Data
    {
        // An array of certificates representing the certificate chain 
        private ArrayList _certificates = null;

        //
        // public constructors
        //

        public KeyInfoX509Data() { }

        public KeyInfoX509Data(X509Certificate2 cert)
        {
            AddCertificate(cert);
        }

        //
        // public properties
        //

        public void AddCertificate(X509Certificate2 certificate)
        {
            if (_certificates == null)
                _certificates = new ArrayList();
            _certificates.Add(certificate);
        }


        //
        // public methods
        //

        public XmlElement GetXml()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.PreserveWhitespace = true;
            return GetXml(xmlDocument);
        }

        internal XmlElement GetXml(XmlDocument xmlDocument)
        {
            XmlElement x509DataElement = xmlDocument.CreateElement("X509Data", SignedXml.XmlDsigNamespaceUrl);

            if (_certificates != null)
            {
                foreach (X509Certificate2 certificate in _certificates)
                {
                    XmlElement x509Element = xmlDocument.CreateElement("X509Certificate", SignedXml.XmlDsigNamespaceUrl);
                    x509Element.AppendChild(xmlDocument.CreateTextNode(Convert.ToBase64String(certificate.RawData)));
                    x509DataElement.AppendChild(x509Element);
                }
            }
            else
            {
                throw new InvalidOperationException();
            }

            return x509DataElement;
        }
    }
}
