using System.Text;

namespace OptimizedZXing.OneD
{
    internal class EAN8Reader : UPCEANReader
    {
        private readonly int[] decodeMiddleCounters = new int[4];
        internal override BarcodeFormat BarcodeFormat => BarcodeFormat.EAN_8;

        /// <summary>
        /// Decodes the middle.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="startRange">The start range.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        override protected internal int DecodeMiddle(BitArray row, int[] startRange, StringBuilder result)
        {
            int[] counters = decodeMiddleCounters;
            counters[0] = 0;
            counters[1] = 0;
            counters[2] = 0;
            counters[3] = 0;
            int end = row.Size;
            int rowOffset = startRange[1];

            for (int x = 0; x < 4 && rowOffset < end; x++)
            {
                if (!DecodeDigit(row, counters, rowOffset, L_PATTERNS, out int bestMatch))
                    return -1;
                result.Append((char)('0' + bestMatch));
                foreach (int counter in counters)
                {
                    rowOffset += counter;
                }
            }

            int[] middleRange = FindGuardPattern(row, rowOffset, true, MIDDLE_PATTERN);
            if (middleRange == null)
                return -1;
            rowOffset = middleRange[1];

            for (int x = 0; x < 4 && rowOffset < end; x++)
            {
                if (!DecodeDigit(row, counters, rowOffset, L_PATTERNS, out int bestMatch))
                    return -1;
                result.Append((char)('0' + bestMatch));
                foreach (int counter in counters)
                {
                    rowOffset += counter;
                }
            }

            return rowOffset;
        }
    }
}
