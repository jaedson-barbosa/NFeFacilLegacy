using OptimizedZXing.OneD;
using System;

namespace OptimizedZXing
{
    internal class MultiFormatOneDReader : IReader
    {
        OneDReader Reader { get; }

        public MultiFormatOneDReader(BarcodeFormat format)
        {
            if (format == BarcodeFormat.EAN_8)
            {
                Reader = new EAN8Reader();
            }
            else if (format == BarcodeFormat.EAN_13)
            {
                Reader = new EAN13Reader();
            }
            else if (format == BarcodeFormat.CODE_128)
            {
                Reader = new Code128Reader();
            }
        }

        public Result Decode(BinaryBitmap image)
        {
            int width = image.Width;
            int height = image.Height;
            BitArray row = new BitArray(width);

            int rowStep = Math.Max(1, height >> 5);
            int maxLines = 15; // 15 rows spaced 1/32 apart is roughly the middle half of the image

            int middle = height >> 1;
            for (int x = 0; x < maxLines; x++)
            {

                // Scanning from the middle out. Determine which row we're looking at next:
                int rowStepsAboveOrBelow = (x + 1) >> 1;
                bool isAbove = (x & 0x01) == 0; // i.e. is x even?
                int rowNumber = middle + rowStep * (isAbove ? rowStepsAboveOrBelow : -rowStepsAboveOrBelow);
                if (rowNumber < 0 || rowNumber >= height)
                {
                    // Oops, if we run off the top or bottom, stop
                    break;
                }

                // Estimate black point for this row and load it:
                row = image.GetBlackRow(rowNumber, row);
                if (row == null)
                    continue;

                // While we have the image data in a BitArray, it's fairly cheap to reverse it in place to
                // handle decoding upside down barcodes.
                for (int attempt = 0; attempt < 2; attempt++)
                {
                    if (attempt == 1)
                    {
                        // trying again?
                        row.Reverse(); // reverse the row and continue
                                       // This means we will only ever draw result points *once* in the life of this method
                                       // since we want to avoid drawing the wrong points after flipping the row, and,
                                       // don't want to clutter with noise from every single row scan -- just the scans
                                       // that start on the center line.
                    }
                    // Look for a barcode
                    Result result = Reader.DecodeRow(rowNumber, row);
                    if (result == null)
                        continue;

                    // We found our barcode
                    if (attempt == 1)
                    {
                        // And remember to flip the result points horizontally.
                        ResultPoint[] points = result.ResultPoints;
                        if (points != null)
                        {
                            points[0] = new ResultPoint(width - points[0].X - 1, points[0].Y);
                            points[1] = new ResultPoint(width - points[1].X - 1, points[1].Y);
                        }
                    }
                    return result;
                }
            }

            return null;
        }
    }
}