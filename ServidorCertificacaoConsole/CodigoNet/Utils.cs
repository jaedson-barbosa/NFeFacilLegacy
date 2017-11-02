// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;

namespace ServidorCertificacaoConsole.CodigoNet
{
    internal static class Utils
    {
        // The maximum number of characters in an XML document (0 means no limit).
        internal const int MaxCharactersInDocument = 0;

        // The entity expansion limit. This is used to prevent entity expansion denial of service attacks.
        internal const long MaxCharactersFromEntities = (long)1e7;

        private static bool HasNamespace(XmlElement element, string prefix, string value)
        {
            if (IsCommittedNamespace(element, prefix, value)) return true;
            if (element.Prefix == prefix && element.NamespaceURI == value) return true;
            return false;
        }

        // A helper function that determines if a namespace node is a committed attribute
        internal static bool IsCommittedNamespace(XmlElement element, string prefix, string value)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            string name = ((prefix.Length > 0) ? "xmlns:" + prefix : "xmlns");
            if (element.HasAttribute(name) && element.GetAttribute(name) == value) return true;
            return false;
        }

        internal static bool IsRedundantNamespace(XmlElement element, string prefix, string value)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            XmlNode ancestorNode = ((XmlNode)element).ParentNode;
            while (ancestorNode != null)
            {
                XmlElement ancestorElement = ancestorNode as XmlElement;
                if (ancestorElement != null)
                    if (HasNamespace(ancestorElement, prefix, value)) return true;
                ancestorNode = ancestorNode.ParentNode;
            }

            return false;
        }

        internal static string GetAttribute(XmlElement element, string localName, string namespaceURI)
        {
            string s = (element.HasAttribute(localName) ? element.GetAttribute(localName) : null);
            if (s == null && element.HasAttribute(localName, namespaceURI))
                s = element.GetAttribute(localName, namespaceURI);
            return s;
        }

        internal static bool HasAttribute(XmlElement element, string localName, string namespaceURI)
        {
            return element.HasAttribute(localName) || element.HasAttribute(localName, namespaceURI);
        }

        internal static bool IsNamespaceNode(XmlNode n)
        {
            return n.NodeType == XmlNodeType.Attribute && (n.Prefix.Equals("xmlns") || (n.Prefix.Length == 0 && n.LocalName.Equals("xmlns")));
        }

        internal static bool IsXmlNamespaceNode(XmlNode n)
        {
            return n.NodeType == XmlNodeType.Attribute && n.Prefix.Equals("xml");
        }

        // We consider xml:space style attributes as default namespace nodes since they obey the same propagation rules
        internal static bool IsDefaultNamespaceNode(XmlNode n)
        {
            bool b1 = n.NodeType == XmlNodeType.Attribute && n.Prefix.Length == 0 && n.LocalName.Equals("xmlns");
            bool b2 = IsXmlNamespaceNode(n);
            return b1 || b2;
        }

        internal static bool IsEmptyDefaultNamespaceNode(XmlNode n)
        {
            return IsDefaultNamespaceNode(n) && n.Value.Length == 0;
        }

        internal static string GetNamespacePrefix(XmlAttribute a)
        {
            Debug.Assert(IsNamespaceNode(a) || IsXmlNamespaceNode(a));
            return a.Prefix.Length == 0 ? string.Empty : a.LocalName;
        }

        internal static bool HasNamespacePrefix(XmlAttribute a, string nsPrefix)
        {
            return GetNamespacePrefix(a).Equals(nsPrefix);
        }

        internal static bool IsNonRedundantNamespaceDecl(XmlAttribute a, XmlAttribute nearestAncestorWithSamePrefix)
        {
            if (nearestAncestorWithSamePrefix == null)
                return !IsEmptyDefaultNamespaceNode(a);
            else
                return !nearestAncestorWithSamePrefix.Value.Equals(a.Value);
        }

        internal static void SBReplaceCharWithString(StringBuilder sb, char oldChar, string newString)
        {
            int i = 0;
            int newStringLength = newString.Length;
            while (i < sb.Length)
            {
                if (sb[i] == oldChar)
                {
                    sb.Remove(i, 1);
                    sb.Insert(i, newString);
                    i += newStringLength;
                }
                else i++;
            }
        }

        internal static XmlDocument PreProcessElementInput(XmlElement elem)
        {
            if (elem == null)
                throw new ArgumentNullException(nameof(elem));

            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            // Normalize the document
            using (TextReader stringReader = new StringReader(elem.OuterXml))
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.DtdProcessing = DtdProcessing.Prohibit;
                settings.MaxCharactersFromEntities = MaxCharactersFromEntities;
                settings.MaxCharactersInDocument = MaxCharactersInDocument;
                XmlReader reader = XmlReader.Create(stringReader, settings);
                doc.Load(reader);
            }
            return doc;
        }

        internal static bool NodeInList(XmlNode node, XmlNodeList nodeList)
        {
            foreach (XmlNode nodeElem in nodeList)
            {
                if (nodeElem == node) return true;
            }
            return false;
        }

        internal static string GetIdFromLocalUri(string uri, out bool discardComments)
        {
            string idref = uri.Substring(1);
            // initialize the return value
            discardComments = true;

            // Deal with XPointer of type #xpointer(id("ID")). Other XPointer support isn't handled here and is anyway optional 
            if (idref.StartsWith("xpointer(id(", StringComparison.Ordinal))
            {
                int startId = idref.IndexOf("id(", StringComparison.Ordinal);
                int endId = idref.IndexOf(")", StringComparison.Ordinal);
                idref = idref.Substring(startId + 3, endId - startId - 3);
                idref = idref.Replace("\'", "");
                idref = idref.Replace("\"", "");
                discardComments = false;
            }
            return idref;
        }

        internal static string ExtractIdFromLocalUri(string uri)
        {
            string idref = uri.Substring(1);

            // Deal with XPointer of type #xpointer(id("ID")). Other XPointer support isn't handled here and is anyway optional 
            if (idref.StartsWith("xpointer(id(", StringComparison.Ordinal))
            {
                int startId = idref.IndexOf("id(", StringComparison.Ordinal);
                int endId = idref.IndexOf(")", StringComparison.Ordinal);
                idref = idref.Substring(startId + 3, endId - startId - 3);
                idref = idref.Replace("\'", "");
                idref = idref.Replace("\"", "");
            }
            return idref;
        }

        internal static string EscapeTextData(string data)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(data);
            sb.Replace("&", "&amp;");
            sb.Replace("<", "&lt;");
            sb.Replace(">", "&gt;");
            SBReplaceCharWithString(sb, (char)13, "&#xD;");
            return sb.ToString(); ;
        }

        internal static string EscapeAttributeValue(string value)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(value);
            sb.Replace("&", "&amp;");
            sb.Replace("<", "&lt;");
            sb.Replace("\"", "&quot;");
            SBReplaceCharWithString(sb, (char)9, "&#x9;");
            SBReplaceCharWithString(sb, (char)10, "&#xA;");
            SBReplaceCharWithString(sb, (char)13, "&#xD;");
            return sb.ToString();
        }

        internal static XmlDocument GetOwnerDocument(XmlNodeList nodeList)
        {
            foreach (XmlNode node in nodeList)
            {
                if (node.OwnerDocument != null)
                    return node.OwnerDocument;
            }
            return null;
        }

        internal static void AddNamespaces(XmlElement elem, CanonicalXmlNodeList namespaces)
        {
            if (namespaces != null)
            {
                foreach (XmlNode attrib in namespaces)
                {
                    string name = ((attrib.Prefix.Length > 0) ? attrib.Prefix + ":" + attrib.LocalName : attrib.LocalName);
                    // Skip the attribute if one with the same qualified name already exists
                    if (elem.HasAttribute(name) || (name.Equals("xmlns") && elem.Prefix.Length == 0)) continue;
                    XmlAttribute nsattrib = (XmlAttribute)elem.OwnerDocument.CreateAttribute(name);
                    nsattrib.Value = attrib.Value;
                    elem.SetAttributeNode(nsattrib);
                }
            }
        }

        // This method gets the attributes that should be propagated 
        internal static CanonicalXmlNodeList GetPropagatedAttributes(XmlElement elem)
        {
            if (elem == null)
                return null;

            CanonicalXmlNodeList namespaces = new CanonicalXmlNodeList();
            XmlNode ancestorNode = elem;

            if (ancestorNode == null) return null;

            bool bDefNamespaceToAdd = true;

            while (ancestorNode != null)
            {
                XmlElement ancestorElement = ancestorNode as XmlElement;
                if (ancestorElement == null)
                {
                    ancestorNode = ancestorNode.ParentNode;
                    continue;
                }
                if (!Utils.IsCommittedNamespace(ancestorElement, ancestorElement.Prefix, ancestorElement.NamespaceURI))
                {
                    // Add the namespace attribute to the collection if needed
                    if (!Utils.IsRedundantNamespace(ancestorElement, ancestorElement.Prefix, ancestorElement.NamespaceURI))
                    {
                        string name = ((ancestorElement.Prefix.Length > 0) ? "xmlns:" + ancestorElement.Prefix : "xmlns");
                        XmlAttribute nsattrib = elem.OwnerDocument.CreateAttribute(name);
                        nsattrib.Value = ancestorElement.NamespaceURI;
                        namespaces.Add(nsattrib);
                    }
                }
                if (ancestorElement.HasAttributes)
                {
                    XmlAttributeCollection attribs = ancestorElement.Attributes;
                    foreach (XmlAttribute attrib in attribs)
                    {
                        // Add a default namespace if necessary
                        if (bDefNamespaceToAdd && attrib.LocalName == "xmlns")
                        {
                            XmlAttribute nsattrib = elem.OwnerDocument.CreateAttribute("xmlns");
                            nsattrib.Value = attrib.Value;
                            namespaces.Add(nsattrib);
                            bDefNamespaceToAdd = false;
                            continue;
                        }
                        // retain the declarations of type 'xml:*' as well
                        if (attrib.Prefix == "xmlns" || attrib.Prefix == "xml")
                        {
                            namespaces.Add(attrib);
                            continue;
                        }
                        if (attrib.NamespaceURI.Length > 0)
                        {
                            if (!Utils.IsCommittedNamespace(ancestorElement, attrib.Prefix, attrib.NamespaceURI))
                            {
                                // Add the namespace attribute to the collection if needed
                                if (!Utils.IsRedundantNamespace(ancestorElement, attrib.Prefix, attrib.NamespaceURI))
                                {
                                    string name = ((attrib.Prefix.Length > 0) ? "xmlns:" + attrib.Prefix : "xmlns");
                                    XmlAttribute nsattrib = elem.OwnerDocument.CreateAttribute(name);
                                    nsattrib.Value = attrib.NamespaceURI;
                                    namespaces.Add(nsattrib);
                                }
                            }
                        }
                    }
                }
                ancestorNode = ancestorNode.ParentNode;
            }

            return namespaces;
        }
    }
}
