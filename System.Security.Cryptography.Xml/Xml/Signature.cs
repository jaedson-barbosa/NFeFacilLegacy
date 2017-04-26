// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Security.Cryptography.Xml
{
    public class Signature
    {
        private SignedInfo _signedInfo;
        private byte[] _signatureValue;
        private KeyInfo _keyInfo;
        private SignedXml _signedXml = null;

        internal SignedXml SignedXml
        {
            get { return _signedXml; }
            set { _signedXml = value; }
        }

        //
        // public properties
        //

        public SignedInfo SignedInfo
        {
            get { return _signedInfo; }
            set
            {
                _signedInfo = value;
                if (SignedXml != null && _signedInfo != null)
                    _signedInfo.SignedXml = SignedXml;
            }
        }

        public byte[] SignatureValue
        {
            get { return _signatureValue; }
            set { _signatureValue = value; }
        }

        public KeyInfo KeyInfo
        {
            get
            {
                if (_keyInfo == null)
                    _keyInfo = new KeyInfo();
                return _keyInfo;
            }
            set { _keyInfo = value; }
        }
    }
}

