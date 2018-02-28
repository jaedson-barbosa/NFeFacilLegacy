/*
 * Copyright 2008 ZXing authors
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
using OptimizedZXing.QR.Internal;

namespace OptimizedZXing.QR
{
    /// <summary>
    /// This object renders a QR Code as a BitMatrix 2D array of greyscale values.
    ///
    /// <author>dswitkin@google.com (Daniel Switkin)</author>
    /// </summary>
    public sealed class QRCodeWriter : IWriter
    {
        /// <summary>
        /// Encode a barcode using the default settings.
        /// </summary>
        /// <param name="contents">The contents to encode in the barcode</param>
        /// <param name="format">The barcode format to generate</param>
        /// <param name="width">The preferred width in pixels</param>
        /// <param name="height">The preferred height in pixels</param>
        /// <returns>
        /// The generated barcode as a Matrix of unsigned bytes (0 == black, 255 == white)
        /// </returns>
        public BitMatrix Encode(String contents, BarcodeFormat format, int width, int height, int margin)
        {
            if (String.IsNullOrEmpty(contents))
            {
                throw new ArgumentException("Found empty contents");
            }

            if (format != BarcodeFormat.QR_CODE)
            {
                throw new ArgumentException("Can only encode QR_CODE, but got " + format);
            }

            if (width < 0 || height < 0)
            {
                throw new ArgumentException("Requested dimensions are too small: " + width + 'x' + height);
            }

            var errorCorrectionLevel = ErrorCorrectionLevel.L;
            var code = Encoder.Encode(contents, errorCorrectionLevel);
            return RenderResult(code, width, height, margin);
        }


        // Note that the input matrix uses 0 == white, 1 == black, while the output matrix uses
        // 0 == black, 255 == white (i.e. an 8 bit greyscale bitmap).
        private static BitMatrix RenderResult(QRCode code, int width, int height, int quietZone)
        {
            var input = code.Matrix;
            if (input == null)
            {
                throw new InvalidOperationException();
            }
            int inputWidth = input.Width;
            int inputHeight = input.Height;
            int qrWidth = inputWidth + (quietZone << 1);
            int qrHeight = inputHeight + (quietZone << 1);
            int outputWidth = Math.Max(width, qrWidth);
            int outputHeight = Math.Max(height, qrHeight);

            int multiple = Math.Min(outputWidth / qrWidth, outputHeight / qrHeight);
            // Padding includes both the quiet zone and the extra white pixels to accommodate the requested
            // dimensions. For example, if input is 25x25 the QR will be 33x33 including the quiet zone.
            // If the requested size is 200x160, the multiple will be 4, for a QR of 132x132. These will
            // handle all the padding from 100x100 (the actual QR) up to 200x160.
            int leftPadding = (outputWidth - (inputWidth * multiple)) / 2;
            int topPadding = (outputHeight - (inputHeight * multiple)) / 2;

            var output = new BitMatrix(outputWidth, outputHeight);

            for (int inputY = 0, outputY = topPadding; inputY < inputHeight; inputY++, outputY += multiple)
            {
                // Write the contents of this row of the barcode
                for (int inputX = 0, outputX = leftPadding; inputX < inputWidth; inputX++, outputX += multiple)
                {
                    if (input[inputX, inputY] == 1)
                    {
                        output.SetRegion(outputX, outputY, multiple, multiple);
                    }
                }
            }

            return output;
        }
    }
}
