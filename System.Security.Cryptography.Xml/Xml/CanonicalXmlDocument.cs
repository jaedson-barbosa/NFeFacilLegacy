// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Xml;
using System.Text;
using System.Collections.Generic;

namespace System.Security.Cryptography.Xml
{
    // all input types eventually lead to the creation of an XmlDocument document
    // of this type. it maintains the node subset state and performs output rendering during canonicalization
    internal class CanonicalXmlDocument : XmlDocument, ICanonicalizableNode
    {
        private bool _defaultNodeSetInclusionState;
        private bool _includeComments;
        private bool _isInNodeSet;

        public CanonicalXmlDocument(bool defaultNodeSetInclusionState, bool includeComments) : base()
        {
            PreserveWhitespace = true;
            _includeComments = includeComments;
            _isInNodeSet = _defaultNodeSetInclusionState = defaultNodeSetInclusionState;
        }

        public bool IsInNodeSet
        {
            get { return _isInNodeSet; }
            set { _isInNodeSet = value; }
        }

        public void Write(StringBuilder strBuilder, C14NAncestralNamespaceContextManager anc)
        {
            foreach (XmlNode childNode in ChildNodes)
            {
                CanonicalizationDispatcher.Write(childNode, strBuilder, anc);
            }
        }

        public void WriteHash(HashAlgorithm hash, C14NAncestralNamespaceContextManager anc, List<byte> conjuntoDados)
        {
            foreach (XmlNode childNode in ChildNodes)
            {
                CanonicalizationDispatcher.WriteHash(childNode, hash, anc, conjuntoDados);
            }
        }

        public override XmlElement CreateElement(string prefix, string localName, string namespaceURI)
        {
            return new CanonicalXmlElement(prefix, localName, namespaceURI, this, _defaultNodeSetInclusionState);
        }

        public override XmlAttribute CreateAttribute(string prefix, string localName, string namespaceURI)
        {
            return new CanonicalXmlAttribute(prefix, localName, namespaceURI, this, _defaultNodeSetInclusionState);
        }

        public override XmlText CreateTextNode(string text)
        {
            return new CanonicalXmlText(text, this, _defaultNodeSetInclusionState);
        }
    }
}
