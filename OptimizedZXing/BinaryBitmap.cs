namespace OptimizedZXing
{
    internal class BinaryBitmap
    {
        private SoftwareBitmapLuminanceSource luminanceSource;

        private const int BLOCK_SIZE_POWER = 3;
        private const int BLOCK_SIZE = 1 << BLOCK_SIZE_POWER; // ...0100...00
        private const int BLOCK_SIZE_MASK = BLOCK_SIZE - 1;   // ...0011...11
        private const int MINIMUM_DIMENSION = 40;
        private const int MIN_DYNAMIC_RANGE = 24;

        const int LUMINANCE_BITS = 5;
        const int LUMINANCE_SHIFT = 8 - LUMINANCE_BITS;
        const int LUMINANCE_BUCKETS = 1 << LUMINANCE_BITS;

        public BinaryBitmap(SoftwareBitmapLuminanceSource luminanceSource)
        {
            this.luminanceSource = luminanceSource;
        }

        public int Width => luminanceSource.Width;
        public int Height => luminanceSource.Height;

        public BitMatrix GetMatrix()
        {
            var source = luminanceSource;
            int width = source.Width;
            int height = source.Height;
            byte[] luminances = source.Matrix;

            int subWidth = width >> BLOCK_SIZE_POWER;
            if ((width & BLOCK_SIZE_MASK) != 0)
            {
                subWidth++;
            }
            int subHeight = height >> BLOCK_SIZE_POWER;
            if ((height & BLOCK_SIZE_MASK) != 0)
            {
                subHeight++;
            }
            int[][] blackPoints = CalculateBlackPoints(luminances, subWidth, subHeight, width, height);

            var newMatrix = new BitMatrix(width, height);
            CalculateThresholdForBlock(luminances, subWidth, subHeight, width, height, blackPoints, newMatrix);
            return newMatrix;
        }

        private static int[][] CalculateBlackPoints(byte[] luminances, int subWidth, int subHeight, int width, int height)
        {
            int maxYOffset = height - BLOCK_SIZE;
            int maxXOffset = width - BLOCK_SIZE;
            int[][] blackPoints = new int[subHeight][];
            for (int i = 0; i < subHeight; i++)
            {
                blackPoints[i] = new int[subWidth];
            }

            for (int y = 0; y < subHeight; y++)
            {
                int yoffset = y << BLOCK_SIZE_POWER;
                if (yoffset > maxYOffset)
                {
                    yoffset = maxYOffset;
                }
                var blackPointsY = blackPoints[y];
                var blackPointsY1 = y > 0 ? blackPoints[y - 1] : null;
                for (int x = 0; x < subWidth; x++)
                {
                    int xoffset = x << BLOCK_SIZE_POWER;
                    if (xoffset > maxXOffset)
                    {
                        xoffset = maxXOffset;
                    }
                    int sum = 0;
                    int min = 0xFF;
                    int max = 0;
                    for (int yy = 0, offset = yoffset * width + xoffset; yy < BLOCK_SIZE; yy++, offset += width)
                    {
                        for (int xx = 0; xx < BLOCK_SIZE; xx++)
                        {
                            int pixel = luminances[offset + xx] & 0xFF;
                            // still looking for good contrast
                            sum += pixel;
                            if (pixel < min)
                            {
                                min = pixel;
                            }
                            if (pixel > max)
                            {
                                max = pixel;
                            }
                        }
                        // short-circuit min/max tests once dynamic range is met
                        if (max - min > MIN_DYNAMIC_RANGE)
                        {
                            // finish the rest of the rows quickly
                            for (yy++, offset += width; yy < BLOCK_SIZE; yy++, offset += width)
                            {
                                for (int xx = 0; xx < BLOCK_SIZE; xx++)
                                {
                                    sum += luminances[offset + xx] & 0xFF;
                                }
                            }
                        }
                    }

                    // The default estimate is the average of the values in the block.
                    int average = sum >> (BLOCK_SIZE_POWER * 2);
                    if (max - min <= MIN_DYNAMIC_RANGE)
                    {
                        // If variation within the block is low, assume this is a block with only light or only
                        // dark pixels. In that case we do not want to use the average, as it would divide this
                        // low contrast area into black and white pixels, essentially creating data out of noise.
                        //
                        // The default assumption is that the block is light/background. Since no estimate for
                        // the level of dark pixels exists locally, use half the min for the block.
                        average = min >> 1;

                        if (blackPointsY1 != null && x > 0)
                        {
                            // Correct the "white background" assumption for blocks that have neighbors by comparing
                            // the pixels in this block to the previously calculated black points. This is based on
                            // the fact that dark barcode symbology is always surrounded by some amount of light
                            // background for which reasonable black point estimates were made. The bp estimated at
                            // the boundaries is used for the interior.

                            // The (min < bp) is arbitrary but works better than other heuristics that were tried.
                            int averageNeighborBlackPoint = (blackPointsY1[x] + (2 * blackPointsY[x - 1]) + blackPointsY1[x - 1]) >> 2;
                            if (min < averageNeighborBlackPoint)
                            {
                                average = averageNeighborBlackPoint;
                            }
                        }
                    }
                    blackPointsY[x] = average;
                }
            }
            return blackPoints;
        }

        private static void CalculateThresholdForBlock(byte[] luminances, int subWidth, int subHeight, int width, int height, int[][] blackPoints, BitMatrix matrix)
        {
            int maxYOffset = height - BLOCK_SIZE;
            int maxXOffset = width - BLOCK_SIZE;

            for (int y = 0; y < subHeight; y++)
            {
                int yoffset = y << BLOCK_SIZE_POWER;
                if (yoffset > maxYOffset)
                {
                    yoffset = maxYOffset;
                }
                int top = cap(y, 2, subHeight - 3);
                for (int x = 0; x < subWidth; x++)
                {
                    int xoffset = x << BLOCK_SIZE_POWER;
                    if (xoffset > maxXOffset)
                    {
                        xoffset = maxXOffset;
                    }
                    int left = cap(x, 2, subWidth - 3);
                    int sum = 0;
                    for (int z = -2; z <= 2; z++)
                    {
                        int[] blackRow = blackPoints[top + z];
                        sum += blackRow[left - 2];
                        sum += blackRow[left - 1];
                        sum += blackRow[left];
                        sum += blackRow[left + 1];
                        sum += blackRow[left + 2];
                    }
                    int average = sum / 25;
                    ThresholdBlock(luminances, xoffset, yoffset, average, width, matrix);
                }
            }

            int cap(int value, int min, int max)
            {
                return value < min ? min : value > max ? max : value;
            }
        }

        private static void ThresholdBlock(byte[] luminances, int xoffset, int yoffset, int threshold, int stride, BitMatrix matrix)
        {
            int offset = (yoffset * stride) + xoffset;
            for (int y = 0; y < BLOCK_SIZE; y++, offset += stride)
            {
                for (int x = 0; x < BLOCK_SIZE; x++)
                {
                    int pixel = luminances[offset + x] & 0xff;
                    // Comparison needs to be <= so that black == 0 pixels are black even if the threshold is 0.
                    matrix[xoffset + x, yoffset + y] = (pixel <= threshold);
                }
            }
        }

        public BitArray GetBlackRow(int y, BitArray row)
        {
            var source = luminanceSource;
            int width = source.Width;
            if (row == null || row.Size < width)
            {
                row = new BitArray(width);
            }
            else
            {
                row.Clear();
            }

            byte[] luminances = new byte[0];
            int[] buckets = new int[LUMINANCE_BUCKETS];
            if (luminances.Length < width)
            {
                luminances = new byte[width];
            }
            for (int x = 0; x < LUMINANCE_BUCKETS; x++)
            {
                buckets[x] = 0;
            }

            byte[] localLuminances = source.GetRow(y, luminances);
            int[] localBuckets = buckets;
            for (int x = 0; x < width; x++)
            {
                localBuckets[(localLuminances[x] & 0xff) >> LUMINANCE_SHIFT]++;
            }
            if (!EstimateBlackPoint(localBuckets, out int blackPoint))
                return null;

            if (width < 3)
            {
                // Special case for very small images
                for (int x = 0; x < width; x++)
                {
                    if ((localLuminances[x] & 0xff) < blackPoint)
                    {
                        row[x] = true;
                    }
                }
            }
            else
            {
                int left = localLuminances[0] & 0xff;
                int center = localLuminances[1] & 0xff;
                for (int x = 1; x < width - 1; x++)
                {
                    int right = localLuminances[x + 1] & 0xff;
                    // A simple -1 4 -1 box filter with a weight of 2.
                    // ((center << 2) - left - right) >> 1
                    if (((center * 4) - left - right) / 2 < blackPoint)
                    {
                        row[x] = true;
                    }
                    left = center;
                    center = right;
                }
            }

            return row;
        }

        private static bool EstimateBlackPoint(int[] buckets, out int blackPoint)
        {
            blackPoint = 0;
            // Find the tallest peak in the histogram.
            int numBuckets = buckets.Length;
            int maxBucketCount = 0;
            int firstPeak = 0;
            int firstPeakSize = 0;
            for (int x = 0; x < numBuckets; x++)
            {
                if (buckets[x] > firstPeakSize)
                {
                    firstPeak = x;
                    firstPeakSize = buckets[x];
                }
                if (buckets[x] > maxBucketCount)
                {
                    maxBucketCount = buckets[x];
                }
            }

            // Find the second-tallest peak which is somewhat far from the tallest peak.
            int secondPeak = 0;
            int secondPeakScore = 0;
            for (int x = 0; x < numBuckets; x++)
            {
                int distanceToBiggest = x - firstPeak;
                // Encourage more distant second peaks by multiplying by square of distance.
                int score = buckets[x] * distanceToBiggest * distanceToBiggest;
                if (score > secondPeakScore)
                {
                    secondPeak = x;
                    secondPeakScore = score;
                }
            }

            // Make sure firstPeak corresponds to the black peak.
            if (firstPeak > secondPeak)
            {
                int temp = firstPeak;
                firstPeak = secondPeak;
                secondPeak = temp;
            }

            // If there is too little contrast in the image to pick a meaningful black point, throw rather
            // than waste time trying to decode the image, and risk false positives.
            // TODO: It might be worth comparing the brightest and darkest pixels seen, rather than the
            // two peaks, to determine the contrast.
            if (secondPeak - firstPeak <= numBuckets >> 4)
            {
                return false;
            }

            // Find a valley between them that is low and closer to the white peak.
            int bestValley = secondPeak - 1;
            int bestValleyScore = -1;
            for (int x = secondPeak - 1; x > firstPeak; x--)
            {
                int fromFirst = x - firstPeak;
                int score = fromFirst * fromFirst * (secondPeak - x) * (maxBucketCount - buckets[x]);
                if (score > bestValleyScore)
                {
                    bestValley = x;
                    bestValleyScore = score;
                }
            }

            blackPoint = bestValley << LUMINANCE_SHIFT;
            return true;
        }
    }
}