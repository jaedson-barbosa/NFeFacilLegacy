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
        public X509Certificate2 Certificate { get; }

        //
        // public constructors
        //

        public KeyInfoX509Data(X509Certificate2 cert)
        {
            Certificate = cert;
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

            XmlElement x509Element = xmlDocument.CreateElement("X509Certificate", SignedXml.XmlDsigNamespaceUrl);
            x509Element.AppendChild(xmlDocument.CreateTextNode(Convert.ToBase64String(Certificate.RawData)));
            x509DataElement.AppendChild(x509Element);

            return x509DataElement;
        }
    }
}
