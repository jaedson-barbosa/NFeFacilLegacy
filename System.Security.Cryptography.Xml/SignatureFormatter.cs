// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Security.Cryptography
{
    public class LocalSignatureFormatter
    {
        private RSA _rsaKey;
        private HashAlgorithmName _algName;

        public LocalSignatureFormatter(RSA rsaKey, HashAlgorithmName hashAlgorithm)
        {
            _rsaKey = rsaKey;
            _algName = hashAlgorithm;
        }

        public byte[] SignHash(byte[] rgbHash)
        {
            return _rsaKey.SignHash(rgbHash, _algName, RSASignaturePadding.Pkcs1);
        }
    }
}
