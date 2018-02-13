namespace OptimizedZXing.OneD
{
    public abstract class OneDReader
    {
        public abstract Result DecodeRow(int rowNumber, BitArray row);

        protected static int INTEGER_MATH_SHIFT = 8;
        protected static int PATTERN_MATCH_RESULT_SCALE_FACTOR = 1 << INTEGER_MATH_SHIFT;

        protected static int PatternMatchVariance(int[] counters,
                                  int[] pattern,
                                  int maxIndividualVariance)
        {
            int numCounters = counters.Length;
            int total = 0;
            int patternLength = 0;
            for (int i = 0; i < numCounters; i++)
            {
                total += counters[i];
                patternLength += pattern[i];
            }
            if (total < patternLength)
            {
                // If we don't even have one pixel per unit of bar width, assume this is too small
                // to reliably match, so fail:
                return int.MaxValue;
            }
            // We're going to fake floating-point math in integers. We just need to use more bits.
            // Scale up patternLength so that intermediate values below like scaledCounter will have
            // more "significant digits"
            int unitBarWidth = (total << INTEGER_MATH_SHIFT) / patternLength;
            maxIndividualVariance = (maxIndividualVariance * unitBarWidth) >> INTEGER_MATH_SHIFT;

            int totalVariance = 0;
            for (int x = 0; x < numCounters; x++)
            {
                int counter = counters[x] << INTEGER_MATH_SHIFT;
                int scaledPattern = pattern[x] * unitBarWidth;
                int variance = counter > scaledPattern ? counter - scaledPattern : scaledPattern - counter;
                if (variance > maxIndividualVariance)
                {
                    return int.MaxValue;
                }
                totalVariance += variance;
            }
            return totalVariance / total;
        }

        protected static bool RecordPattern(BitArray row,
                                    int start,
                                    int[] counters,
                                    int numCounters)
        {
            for (int idx = 0; idx < numCounters; idx++)
            {
                counters[idx] = 0;
            }
            int end = row.Size;
            if (start >= end)
            {
                return false;
            }
            bool isWhite = !row[start];
            int counterPosition = 0;
            int i = start;
            while (i < end)
            {
                if (row[i] != isWhite)
                {
                    counters[counterPosition]++;
                }
                else
                {
                    counterPosition++;
                    if (counterPosition == numCounters)
                    {
                        break;
                    }
                    else
                    {
                        counters[counterPosition] = 1;
                        isWhite = !isWhite;
                    }
                }
                i++;
            }
            // If we read fully the last section of pixels and filled up our counters -- or filled
            // the last counter but ran off the side of the image, OK. Otherwise, a problem.
            return (counterPosition == numCounters || (counterPosition == numCounters - 1 && i == end));
        }
    }
}
