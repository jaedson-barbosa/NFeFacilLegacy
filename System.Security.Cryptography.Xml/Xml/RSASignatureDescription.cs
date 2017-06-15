// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


namespace System.Security.Cryptography.Xml
{
    internal class RSASignatureDescription
    {
        private string FormatterAlgorithm { get; }
        private string DigestAlgorithm { get; }

        public RSASignatureDescription()
        {
            FormatterAlgorithm = typeof(RSAPKCS1SignatureFormatter).AssemblyQualifiedName;
            DigestAlgorithm = "SHA1";
        }

        public RSAPKCS1SignatureFormatter CreateFormatter(RSA key)
        {
            var item = new RSAPKCS1SignatureFormatter(key);
            item.SetHashAlgorithm(DigestAlgorithm);
            return item;
        }

        public HashAlgorithm CreateDigest()
        {
            return SHA1.Create();
        }
    }
}
