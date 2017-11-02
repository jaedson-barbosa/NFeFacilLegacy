// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Security.Cryptography;
using System.Xml;

namespace ServidorCertificacaoConsole.CodigoNet
{
    public class XmlDsigEnvelopedSignatureTransform : Transform
    {
        private Type[] _inputTypes = { typeof(Stream), typeof(XmlNodeList), typeof(XmlDocument) };
        private Type[] _outputTypes = { typeof(XmlNodeList), typeof(XmlDocument) };
        private XmlNodeList _inputNodeList;
        private bool _includeComments = false;
        private XmlNamespaceManager _nsm = null;
        private XmlDocument _containingDocument = null;
        private int _signaturePosition = 0;

        internal int SignaturePosition
        {
            set { _signaturePosition = value; }
        }

        public XmlDsigEnvelopedSignatureTransform()
        {
            Algorithm = SignedXml.XmlDsigEnvelopedSignatureTransformUrl;
        }

        /// <internalonly/>
        public XmlDsigEnvelopedSignatureTransform(bool includeComments)
        {
            _includeComments = includeComments;
            Algorithm = SignedXml.XmlDsigEnvelopedSignatureTransformUrl;
        }

        // An enveloped signature has no inner XML elements
        protected override XmlNodeList GetInnerXml()
        {
            return null;
        }

        public override void LoadInput(XmlDocument doc)
        {
            _containingDocument = doc ?? throw new ArgumentNullException(nameof(doc));
            _nsm = new XmlNamespaceManager(_containingDocument.NameTable);
            _nsm.AddNamespace("dsig", SignedXml.XmlDsigNamespaceUrl);
        }

        private void LoadXmlNodeListInput(XmlNodeList nodeList)
        {
            // Empty node list is not acceptable
            if (nodeList == null)
                throw new ArgumentNullException(nameof(nodeList));
            _containingDocument = Utils.GetOwnerDocument(nodeList);

            _nsm = new XmlNamespaceManager(_containingDocument.NameTable);
            _nsm.AddNamespace("dsig", SignedXml.XmlDsigNamespaceUrl);
            _inputNodeList = nodeList;
        }

        public override object GetOutput()
        {
            return _containingDocument;
        }

        public override byte[] GetDigestedOutput(HashAlgorithm hash)
        {
            throw new NotImplementedException();
        }
    }
}
