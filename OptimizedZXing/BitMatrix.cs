using System;
using System.Text;

namespace OptimizedZXing
{
    public sealed partial class BitMatrix
    {
        private readonly int rowSize;
        private readonly int[] bits;

        /// <returns> The width of the matrix
        /// </returns>
        public int Width { get; }

        /// <returns> The height of the matrix
        /// </returns>
        public int Height { get; }

        /// <summary>
        /// Creates an empty square <see cref="BitMatrix"/>.
        /// </summary>
        /// <param name="dimension">height and width</param>
        public BitMatrix(int dimension) : this(dimension, dimension)
        {
        }

        /// <summary>
        /// Creates an empty square <see cref="BitMatrix"/>.
        /// </summary>
        /// <param name="width">bit matrix width</param>
        /// <param name="height">bit matrix height</param>
        public BitMatrix(int width, int height)
        {
            if (width < 1 || height < 1)
            {
                throw new ArgumentException("Both dimensions must be greater than 0");
            }
            Width = width;
            Height = height;
            rowSize = (width + 31) >> 5;
            bits = new int[rowSize * height];
        }

        /// <summary> <p>Gets the requested bit, where true means black.</p>
        /// 
        /// </summary>
        /// <param name="x">The horizontal component (i.e. which column)
        /// </param>
        /// <param name="y">The vertical component (i.e. which row)
        /// </param>
        /// <returns> value of given bit in matrix
        /// </returns>
        public bool this[int x, int y]
        {
            get
            {
                int offset = y * rowSize + (x >> 5);
                return (((int)((uint)(bits[offset]) >> (x & 0x1f))) & 1) != 0;
            }
            set
            {
                if (value)
                {
                    int offset = y * rowSize + (x >> 5);
                    bits[offset] |= 1 << (x & 0x1f);
                }
                else
                {
                    int offset = y * rowSize + (x / 32);
                    bits[offset] &= ~(1 << (x & 0x1f));
                }
            }
        }

        /// <summary>
        /// <p>Flips the given bit.</p>
        /// </summary>
        /// <param name="x">The horizontal component (i.e. which column)</param>
        /// <param name="y">The vertical component (i.e. which row)</param>
        public void Flip(int x, int y)
        {
            int offset = y * rowSize + (x >> 5);
            bits[offset] ^= 1 << (x & 0x1f);
        }

        /// <summary>
        /// flip all of the bits, if shouldBeFlipped is true for the coordinates
        /// </summary>
        /// <param name="shouldBeFlipped">should return true, if the bit at a given coordinate should be flipped</param>
        public void FlipWhen(Func<int, int, bool> shouldBeFlipped)
        {
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    if (shouldBeFlipped(y, x))
                    {
                        int offset = y * rowSize + (x >> 5);
                        bits[offset] ^= 1 << (x & 0x1f);
                    }
                }
            }
        }

        /// <summary> Clears all bits (sets to false).</summary>
        public void Clear()
        {
            int max = bits.Length;
            for (int i = 0; i < max; i++)
            {
                bits[i] = 0;
            }
        }

        /// <summary> <p>Sets a square region of the bit matrix to true.</p>
        /// 
        /// </summary>
        /// <param name="left">The horizontal position to begin at (inclusive)
        /// </param>
        /// <param name="top">The vertical position to begin at (inclusive)
        /// </param>
        /// <param name="width">The width of the region
        /// </param>
        /// <param name="height">The height of the region
        /// </param>
        public void SetRegion(int left, int top, int width, int height)
        {
            if (top < 0 || left < 0)
            {
                throw new ArgumentException("Left and top must be nonnegative");
            }
            if (height < 1 || width < 1)
            {
                throw new ArgumentException("Height and width must be at least 1");
            }
            int right = left + width;
            int bottom = top + height;
            if (bottom > this.Height || right > this.Width)
            {
                throw new ArgumentException("The region must fit inside the matrix");
            }
            for (int y = top; y < bottom; y++)
            {
                int offset = y * rowSize;
                for (int x = left; x < right; x++)
                {
                    bits[offset + (x >> 5)] |= 1 << (x & 0x1f);
                }
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is BitMatrix))
            {
                return false;
            }
            var other = (BitMatrix)obj;
            if (Width != other.Width || Height != other.Height ||
                rowSize != other.rowSize || bits.Length != other.bits.Length)
            {
                return false;
            }
            for (int i = 0; i < bits.Length; i++)
            {
                if (bits[i] != other.bits[i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            int hash = Width;
            hash = 31 * hash + Width;
            hash = 31 * hash + Width;
            hash = 31 * hash + rowSize;
            foreach (var bit in bits)
            {
                hash = 31 * hash + bit.GetHashCode();
            }
            return hash;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override String ToString()
        {
            return ToString("X ", "  ", Environment.NewLine);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="setString">The set string.</param>
        /// <param name="unsetString">The unset string.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public String ToString(String setString, String unsetString)
        {
            return BuildToString(setString, unsetString, Environment.NewLine);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="setString">The set string.</param>
        /// <param name="unsetString">The unset string.</param>
        /// <param name="lineSeparator">The line separator.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public String ToString(String setString, String unsetString, String lineSeparator)
        {
            return BuildToString(setString, unsetString, lineSeparator);
        }

        private String BuildToString(String setString, String unsetString, String lineSeparator)
        {
            var result = new StringBuilder(Height * (Width + 1));
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    result.Append(this[x, y] ? setString : unsetString);
                }
                result.Append(lineSeparator);
            }
            return result.ToString();
        }
    }
}
