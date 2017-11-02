// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ServidorCertificacaoConsole.CodigoNet
{
    // the interface to be implemented by all subclasses of XmlNode
    // that have to provide node subsetting and canonicalization features.
    internal interface ICanonicalizableNode
    {
        bool IsInNodeSet
        {
            get;
            set;
        }

        void Write(StringBuilder strBuilder, C14NAncestralNamespaceContextManager anc);
        void WriteHash(HashAlgorithm hash, C14NAncestralNamespaceContextManager anc, List<byte> conjuntoDados);
    }
}
