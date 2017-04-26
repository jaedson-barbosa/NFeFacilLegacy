// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


namespace System.Security.Cryptography.Xml
{
    internal static class CryptoHelpers
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA5350", Justification = "SHA1 needed for compat.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA5351", Justification = "HMACMD5 needed for compat.")]
        public static object CreateFromName(string name)
        {
            switch (name)
            {
                case "http://www.w3.org/TR/2001/REC-xml-c14n-20010315":
                    return new XmlDsigC14NTransform();
                case "http://www.w3.org/2000/09/xmldsig#enveloped-signature":
                    return new XmlDsigEnvelopedSignatureTransform();
                case "http://www.w3.org/2000/09/xmldsig# X509Data":
                    return new KeyInfoX509Data();
                case "http://www.w3.org/2000/09/xmldsig#rsa-sha1":
                    return new RSASignatureDescription();
                case "http://www.w3.org/2000/09/xmldsig#sha1":
                    return SHA1.Create();
            }
            throw new ArgumentException();
        }
    }
}
