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
using System.Text;

namespace OptimizedZXing.QR.Internal
{
    /// <summary>
    /// JAVAPORT: The original code was a 2D array of ints, but since it only ever gets assigned
    /// 0, 1 and 2 I'm going to use less memory and go with bytes.
    /// </summary>
    /// <author>dswitkin@google.com (Daniel Switkin)</author>
    public sealed class ByteMatrix
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ByteMatrix"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public ByteMatrix(int width, int height)
        {
            Array = new byte[height][];
            for (var i = 0; i < height; i++)
                Array[i] = new byte[width];
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Gets the height.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Gets the width.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Gets or sets the <see cref="System.Int32"/> with the specified x.
        /// </summary>
        public int this[int x, int y]
        {
            get { return Array[y][x]; }
            set { Array[y][x] = (byte)value; }
        }

        /// <summary>
        /// an internal representation as bytes, in row-major order. array[y][x] represents point (x,y)
        /// </summary>
        public byte[][] Array { get; }

        /// <summary>
        /// Clears the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Clear(byte value)
        {
            for (int y = 0; y < Height; ++y)
            {
                var bytesY = Array[y];
                for (int x = 0; x < Width; ++x)
                {
                    bytesY[x] = value;
                }
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        override public String ToString()
        {
            var result = new StringBuilder(2 * Width * Height + 2);
            for (int y = 0; y < Height; ++y)
            {
                var bytesY = Array[y];
                for (int x = 0; x < Width; ++x)
                {
                    switch (bytesY[x])
                    {
                        case 0:
                            result.Append(" 0");
                            break;
                        case 1:
                            result.Append(" 1");
                            break;
                        default:
                            result.Append("  ");
                            break;
                    }
                }
                result.Append('\n');
            }
            return result.ToString();
        }
    }
}