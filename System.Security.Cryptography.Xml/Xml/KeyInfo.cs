// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Xml;

namespace System.Security.Cryptography.Xml
{
    public class KeyInfo
    {
        public KeyInfoX509Data KeyInfoClause { get; set; }

        public XmlElement GetXml()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.PreserveWhitespace = true;
            return GetXml(xmlDocument);
        }

        internal XmlElement GetXml(XmlDocument xmlDocument)
        {
            // Create the KeyInfo element itself
            XmlElement keyInfoElement = xmlDocument.CreateElement("KeyInfo", SignedXml.XmlDsigNamespaceUrl);

            // Add all the clauses that go underneath it
            XmlElement xmlElement = KeyInfoClause.GetXml(xmlDocument);
            if (xmlElement != null)
            {
                keyInfoElement.AppendChild(xmlElement);
            }
            return keyInfoElement;
        }
    }
}
