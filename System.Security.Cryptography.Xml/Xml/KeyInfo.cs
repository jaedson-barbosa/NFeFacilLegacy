// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
    public class KeyInfo
    {
        private ArrayList _keyInfoClauses;

        public KeyInfo()
        {
            _keyInfoClauses = new ArrayList();
        }

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
            for (int i = 0; i < _keyInfoClauses.Count; ++i)
            {
                XmlElement xmlElement = ((KeyInfoX509Data)_keyInfoClauses[i]).GetXml(xmlDocument);
                if (xmlElement != null)
                {
                    keyInfoElement.AppendChild(xmlElement);
                }
            }
            return keyInfoElement;
        }

        public void AddClause(KeyInfoX509Data clause)
        {
            _keyInfoClauses.Add(clause);
        }
    }
}
