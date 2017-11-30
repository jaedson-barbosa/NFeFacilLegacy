// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Security.Cryptography;
using System.Xml;

namespace ServidorCertificacao.CodigoNet
{
    public class XmlDsigC14NTransform : Transform
    {
        private Type[] _inputTypes = { typeof(Stream), typeof(XmlDocument), typeof(XmlNodeList) };
        private Type[] _outputTypes = { typeof(Stream) };
        private CanonicalXml _cXml;
        private bool _includeComments = false;

        public XmlDsigC14NTransform()
        {
            Algorithm = SignedXml.XmlDsigC14NTransformUrl;
        }

        public XmlDsigC14NTransform(bool includeComments)
        {
            _includeComments = includeComments;
            Algorithm = SignedXml.XmlDsigC14NTransformUrl;
        }

        protected override XmlNodeList GetInnerXml()
        {
            return null;
        }

        public override void LoadInput(XmlDocument obj)
        {
            _cXml = new CanonicalXml(obj, _includeComments);
        }

        public override object GetOutput()
        {
            return new MemoryStream(_cXml.GetBytes());
        }

        public override byte[] GetDigestedOutput(HashAlgorithm hash)
        {
            return _cXml.GetDigestedBytes(hash);
        }
    }
}
