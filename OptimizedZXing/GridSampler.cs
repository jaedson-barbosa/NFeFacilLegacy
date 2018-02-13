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

namespace OptimizedZXing
{
    /// <summary> Implementations of this class can, given locations of finder patterns for a QR code in an
    /// image, sample the right points in the image to reconstruct the QR code, accounting for
    /// perspective distortion. It is abstracted since it is relatively expensive and should be allowed
    /// to take advantage of platform-specific optimized implementations, like Sun's Java Advanced
    /// Imaging library, but which may not be available in other environments such as J2ME, and vice
    /// versa.
    /// 
    /// The implementation used can be controlled by calling {@link #setGridSampler(GridSampler)}
    /// with an instance of a class which implements this interface.
    /// </summary>
    /// <author> Sean Owen</author>
    public static class GridSampler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        /// <param name="dimensionX"></param>
        /// <param name="dimensionY"></param>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static BitMatrix SampleGrid(BitMatrix image, int dimensionX, int dimensionY, PerspectiveTransform transform)
        {
            if (dimensionX <= 0 || dimensionY <= 0)
            {
                return null;
            }
            BitMatrix bits = new BitMatrix(dimensionX, dimensionY);
            float[] points = new float[dimensionX << 1];
            for (int y = 0; y < dimensionY; y++)
            {
                int max = points.Length;
                //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                float iValue = (float)y + 0.5f;
                for (int x = 0; x < max; x += 2)
                {
                    //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                    points[x] = (float)(x >> 1) + 0.5f;
                    points[x + 1] = iValue;
                }
                transform.TransformPoints(points);
                // Quick check to see if points transformed to something inside the image;
                // sufficient to check the endpoints
                if (!CheckAndNudgePoints(image, points))
                    return null;
                try
                {
                    var imageWidth = image.Width;
                    var imageHeight = image.Height;

                    for (int x = 0; x < max; x += 2)
                    {
                        var imagex = (int)points[x];
                        var imagey = (int)points[x + 1];

                        if (imagex < 0 || imagex >= imageWidth || imagey < 0 || imagey >= imageHeight)
                        {
                            return null;
                        }

                        bits[x >> 1, y] = image[imagex, imagey];
                    }
                }
                catch (System.IndexOutOfRangeException)
                {
                    // java version:
                    // 
                    // This feels wrong, but, sometimes if the finder patterns are misidentified, the resulting
                    // transform gets "twisted" such that it maps a straight line of points to a set of points
                    // whose endpoints are in bounds, but others are not. There is probably some mathematical
                    // way to detect this about the transformation that I don't know yet.
                    // This results in an ugly runtime exception despite our clever checks above -- can't have
                    // that. We could check each point's coordinates but that feels duplicative. We settle for
                    // catching and wrapping ArrayIndexOutOfBoundsException.
                    return null;
                }
            }
            return bits;
        }


        /// <summary> <p>Checks a set of points that have been transformed to sample points on an image against
        /// the image's dimensions to see if the point are even within the image.</p>
        /// 
        /// <p>This method will actually "nudge" the endpoints back onto the image if they are found to be
        /// barely (less than 1 pixel) off the image. This accounts for imperfect detection of finder
        /// patterns in an image where the QR Code runs all the way to the image border.</p>
        /// 
        /// <p>For efficiency, the method will check points from either end of the line until one is found
        /// to be within the image. Because the set of points are assumed to be linear, this is valid.</p>
        /// 
        /// </summary>
        /// <param name="image">image into which the points should map
        /// </param>
        /// <param name="points">actual points in x1,y1,...,xn,yn form
        /// </param>
        static bool CheckAndNudgePoints(BitMatrix image, float[] points)
        {
            int width = image.Width;
            int height = image.Height;
            // Check and nudge points from start until we see some that are OK:
            bool nudged = true;
            for (int offset = 0; offset < points.Length && nudged; offset += 2)
            {
                //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                int x = (int)points[offset];
                //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                int y = (int)points[offset + 1];
                if (x < -1 || x > width || y < -1 || y > height)
                {
                    return false;
                }
                nudged = false;
                if (x == -1)
                {
                    points[offset] = 0.0f;
                    nudged = true;
                }
                else if (x == width)
                {
                    points[offset] = width - 1;
                    nudged = true;
                }
                if (y == -1)
                {
                    points[offset + 1] = 0.0f;
                    nudged = true;
                }
                else if (y == height)
                {
                    points[offset + 1] = height - 1;
                    nudged = true;
                }
            }
            // Check and nudge points from end:
            nudged = true;
            for (int offset = points.Length - 2; offset >= 0 && nudged; offset -= 2)
            {
                //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                int x = (int)points[offset];
                //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                int y = (int)points[offset + 1];
                if (x < -1 || x > width || y < -1 || y > height)
                {
                    return false;
                }
                nudged = false;
                if (x == -1)
                {
                    points[offset] = 0.0f;
                    nudged = true;
                }
                else if (x == width)
                {
                    points[offset] = width - 1;
                    nudged = true;
                }
                if (y == -1)
                {
                    points[offset + 1] = 0.0f;
                    nudged = true;
                }
                else if (y == height)
                {
                    points[offset + 1] = height - 1;
                    nudged = true;
                }
            }

            return true;
        }
    }
}