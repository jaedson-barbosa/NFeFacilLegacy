// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Xml;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using System;

namespace ServidorCertificacao.CodigoNet
{
    // the central dispatcher for canonicalization writes. not all node classes
    // implement ICanonicalizableNode; so a manual dispatch is sometimes necessary.
    internal static class CanonicalizationDispatcher
    {
        public static void Write(XmlNode node, StringBuilder strBuilder, C14NAncestralNamespaceContextManager anc)
        {
            if (node is ICanonicalizableNode canonicalizableNode)
            {
                canonicalizableNode.Write(strBuilder, anc);
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public static void WriteHash(XmlNode node, HashAlgorithm hash, C14NAncestralNamespaceContextManager anc, List<byte> conjuntoDados)
        {
            if (node is ICanonicalizableNode canonicalizableNode)
            {
                canonicalizableNode.WriteHash(hash, anc, conjuntoDados);
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}
