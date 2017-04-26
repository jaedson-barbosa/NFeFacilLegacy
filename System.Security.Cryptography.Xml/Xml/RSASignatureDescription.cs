// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


namespace System.Security.Cryptography.Xml
{
    internal class RSASignatureDescription
    {
        const string HashAlgorithm = "SHA1";
        private string FormatterAlgorithm { get; }
        private string DigestAlgorithm { get; }

        public RSASignatureDescription()
        {
            FormatterAlgorithm = typeof(RSAPKCS1SignatureFormatter).AssemblyQualifiedName;
            DigestAlgorithm = HashAlgorithm;
        }

        public RSAPKCS1SignatureFormatter CreateFormatter(AsymmetricAlgorithm key)
        {
            var item = new RSAPKCS1SignatureFormatter();
            item.SetKey(key);
            item.SetHashAlgorithm(DigestAlgorithm);
            return item;
        }

        public HashAlgorithm CreateDigest()
        {
#pragma warning disable CA5350 // Do not use insecure cryptographic algorithm SHA1.
            return SHA1.Create();
#pragma warning restore CA5350 // Do not use insecure cryptographic algorithm SHA1.
        }
    }
}
