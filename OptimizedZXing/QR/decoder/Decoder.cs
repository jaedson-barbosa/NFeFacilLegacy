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

using OptimizedZXing.ReedSolomon;

namespace OptimizedZXing.QR.Internal
{
    /// <summary>
    ///   <p>The main class which implements QR Code decoding -- as opposed to locating and extracting
    /// the QR Code from an image.</p>
    /// </summary>
    /// <author>
    /// Sean Owen
    /// </author>
    public sealed class Decoder
    {
        private readonly ReedSolomonDecoder rsDecoder;

        /// <summary>
        /// Initializes a new instance of the <see cref="Decoder"/> class.
        /// </summary>
        public Decoder()
        {
            rsDecoder = new ReedSolomonDecoder(GenericGF.QR_CODE_FIELD_256);
        }

        /// <summary>
        ///   <p>Decodes a QR Code represented as a {@link BitMatrix}. A 1 or "true" is taken to mean a black module.</p>
        /// </summary>
        /// <param name="bits">booleans representing white/black QR Code modules</param>
        /// <param name="hints">decoding hints that should be used to influence decoding</param>
        /// <returns>
        /// text and bytes encoded within the QR Code
        /// </returns>
        public DecoderResult Decode(BitMatrix bits)
        {
            // Construct a parser and read version, error-correction level
            var parser = BitMatrixParser.CreateBitMatrixParser(bits);
            if (parser == null)
                return null;

            var result = Decode(parser);
            if (result == null)
            {
                // Revert the bit matrix
                parser.Remask();

                // Will be attempting a mirrored reading of the version and format info.
                parser.SetMirror(true);

                // Preemptively read the version.
                var version = parser.ReadVersion();
                if (version == null)
                    return null;

                // Preemptively read the format information.
                var formatinfo = parser.ReadFormatInformation();
                if (formatinfo == null)
                    return null;

                /*
                 * Since we're here, this means we have successfully detected some kind
                 * of version and format information when mirrored. This is a good sign,
                 * that the QR code may be mirrored, and we should try once more with a
                 * mirrored content.
                 */
                // Prepare for a mirrored reading.
                parser.Mirror();

                result = Decode(parser);

                if (result != null)
                {
                    // Success! Notify the caller that the code was mirrored.
                    result.Other = new QRCodeDecoderMetaData(true);
                }
            }

            return result;
        }

        private DecoderResult Decode(BitMatrixParser parser)
        {
            Version version = parser.ReadVersion();
            if (version == null)
                return null;
            var formatinfo = parser.ReadFormatInformation();
            if (formatinfo == null)
                return null;
            ErrorCorrectionLevel ecLevel = formatinfo.ErrorCorrectionLevel;

            // Read codewords
            byte[] codewords = parser.ReadCodewords();
            if (codewords == null)
                return null;
            // Separate into data blocks
            DataBlock[] dataBlocks = DataBlock.GetDataBlocks(codewords, version, ecLevel);

            // Count total number of data bytes
            int totalBytes = 0;
            foreach (var dataBlock in dataBlocks)
            {
                totalBytes += dataBlock.NumDataCodewords;
            }
            byte[] resultBytes = new byte[totalBytes];
            int resultOffset = 0;

            // Error-correct and copy data blocks together into a stream of bytes
            foreach (var dataBlock in dataBlocks)
            {
                byte[] codewordBytes = dataBlock.Codewords;
                int numDataCodewords = dataBlock.NumDataCodewords;
                if (!CorrectErrors(codewordBytes, numDataCodewords))
                    return null;
                for (int i = 0; i < numDataCodewords; i++)
                {
                    resultBytes[resultOffset++] = codewordBytes[i];
                }
            }

            // Decode the contents of that stream of bytes
            return DecodedBitStreamParser.Decode(resultBytes, version, ecLevel);
        }

        /// <summary>
        ///   <p>Given data and error-correction codewords received, possibly corrupted by errors, attempts to
        /// correct the errors in-place using Reed-Solomon error correction.</p>
        /// </summary>
        /// <param name="codewordBytes">data and error correction codewords</param>
        /// <param name="numDataCodewords">number of codewords that are data bytes</param>
        /// <returns></returns>
        private bool CorrectErrors(byte[] codewordBytes, int numDataCodewords)
        {
            int numCodewords = codewordBytes.Length;
            // First read into an array of ints
            int[] codewordsInts = new int[numCodewords];
            for (int i = 0; i < numCodewords; i++)
            {
                codewordsInts[i] = codewordBytes[i] & 0xFF;
            }
            int numECCodewords = codewordBytes.Length - numDataCodewords;

            if (!rsDecoder.Decode(codewordsInts, numECCodewords))
                return false;

            // Copy back into array of bytes -- only need to worry about the bytes that were data
            // We don't care about errors in the error-correction codewords
            for (int i = 0; i < numDataCodewords; i++)
            {
                codewordBytes[i] = (byte)codewordsInts[i];
            }

            return true;
        }
    }
}