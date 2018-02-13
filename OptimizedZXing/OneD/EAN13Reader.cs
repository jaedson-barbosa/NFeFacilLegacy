using System.Text;

namespace OptimizedZXing.OneD
{
    internal class EAN13Reader : UPCEANReader
    {
        internal static int[] FIRST_DIGIT_ENCODINGS = { 0x00, 0x0B, 0x0D, 0xE, 0x13, 0x19, 0x1C, 0x15, 0x16, 0x1A };
        int[] decodeMiddleCounters = new int[4];
        internal override BarcodeFormat BarcodeFormat => BarcodeFormat.EAN_13;

        /// <summary>
        /// Subclasses override this to decode the portion of a barcode between the start
        /// and end guard patterns.
        /// </summary>
        /// <param name="row">row of black/white values to search</param>
        /// <param name="startRange">start/end offset of start guard pattern</param>
        /// <param name="resultString"><see cref="StringBuilder"/>to append decoded chars to</param>
        /// <returns>
        /// horizontal offset of first pixel after the "middle" that was decoded or -1 if decoding could not complete successfully
        /// </returns>
        override protected internal int DecodeMiddle(BitArray row,
                                   int[] startRange,
                                   StringBuilder resultString)
        {
            int[] counters = decodeMiddleCounters;
            counters[0] = 0;
            counters[1] = 0;
            counters[2] = 0;
            counters[3] = 0;
            int end = row.Size;
            int rowOffset = startRange[1];

            int lgPatternFound = 0;

            for (int x = 0; x < 6 && rowOffset < end; x++)
            {
                if (!DecodeDigit(row, counters, rowOffset, L_AND_G_PATTERNS, out int bestMatch))
                    return -1;
                resultString.Append((char)('0' + bestMatch % 10));
                foreach (int counter in counters)
                {
                    rowOffset += counter;
                }
                if (bestMatch >= 10)
                {
                    lgPatternFound |= 1 << (5 - x);
                }
            }

            if (!DetermineFirstDigit(resultString, lgPatternFound))
                return -1;

            int[] middleRange = FindGuardPattern(row, rowOffset, true, MIDDLE_PATTERN);
            if (middleRange == null)
                return -1;
            rowOffset = middleRange[1];

            for (int x = 0; x < 6 && rowOffset < end; x++)
            {
                if (!DecodeDigit(row, counters, rowOffset, L_PATTERNS, out int bestMatch))
                    return -1;
                resultString.Append((char)('0' + bestMatch));
                foreach (int counter in counters)
                {
                    rowOffset += counter;
                }
            }

            return rowOffset;
        }

        /// <summary>
        /// Based on pattern of odd-even ('L' and 'G') patterns used to encoded the explicitly-encoded
        /// digits in a barcode, determines the implicitly encoded first digit and adds it to the
        /// result string.
        /// </summary>
        /// <param name="resultString">string to insert decoded first digit into</param>
        /// <param name="lgPatternFound">int whose bits indicates the pattern of odd/even L/G patterns used to</param>
        ///  encode digits
        /// <return>-1 if first digit cannot be determined</return>
        static bool DetermineFirstDigit(StringBuilder resultString, int lgPatternFound)
        {
            for (int d = 0; d < 10; d++)
            {
                if (lgPatternFound == FIRST_DIGIT_ENCODINGS[d])
                {
                    resultString.Insert(0, new[] { (char)('0' + d) });
                    return true;
                }
            }
            return false;
        }
    }
}
