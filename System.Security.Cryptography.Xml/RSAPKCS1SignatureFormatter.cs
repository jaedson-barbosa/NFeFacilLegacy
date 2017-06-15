// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Internal.Cryptography;

namespace System.Security.Cryptography
{
    public class RSAPKCS1SignatureFormatter
    {
        private RSA _rsaKey;
        private string _algName;

        public RSAPKCS1SignatureFormatter(RSA key)
        {
            _rsaKey = key;
        }

        public void SetHashAlgorithm(string strName)
        {
            try
            {
                // Verify the name
                Oid.FromFriendlyName(strName, OidGroup.HashAlgorithm);

                // Uppercase known names as required for BCrypt
                _algName = HashAlgorithmNames.ToUpper(strName);
            }
            catch (CryptographicException)
            {
                // For desktop compat, exception is deferred until VerifySignature
                _algName = null;
            }
        }

        public byte[] CreateSignature(byte[] rgbHash)
        {
            if (rgbHash == null)
                throw new ArgumentNullException(nameof(rgbHash));
            return _rsaKey.SignHash(rgbHash, new HashAlgorithmName(_algName), RSASignaturePadding.Pkcs1);
        }
    }
}
