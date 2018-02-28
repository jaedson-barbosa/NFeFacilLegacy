/*
* Copyright 2007 ZXing authors
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*      http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;

namespace OptimizedZXing
{
    /// <summary>
    /// Encapsulates the result of decoding a matrix of bits. This typically
    /// applies to 2D barcode formats. For now it contains the raw bytes obtained,
    /// as well as a String interpretation of those bytes, if applicable.
    /// <author>Sean Owen</author>
    /// </summary>
    public sealed class DecoderResult
    {
        /// <summary>
        /// raw bytes representing the result, or null if not applicable
        /// </summary>
        public byte[] RawBytes { get; private set; }

        /// <summary>
        /// text representation of the result
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Miscellanseous data value for the various decoders
        /// </summary>
        /// <value>The other.</value>
        public object Other { get; set; }

        /// <summary>
        /// initializing constructor
        /// </summary>
        /// <param name="rawBytes"></param>
        /// <param name="text"></param>
        /// <param name="byteSegments"></param>
        /// <param name="ecLevel"></param>
        /// <param name="saSequence"></param>
        /// <param name="saParity"></param>
        public DecoderResult(byte[] rawBytes, string text)
        {
            if (rawBytes == null && text == null)
            {
                throw new ArgumentException();
            }
            RawBytes = rawBytes;
            Text = text;
        }
    }
}