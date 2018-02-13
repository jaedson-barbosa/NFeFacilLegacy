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

using System;

namespace OptimizedZXing.QR.Internal
{
    /// <summary>
    /// <p>Encapsulates a finder pattern, which are the three square patterns found in
    /// the corners of QR Codes. It also encapsulates a count of similar finder patterns,
    /// as a convenience to the finder's bookkeeping.</p>
    /// </summary>
    /// <author>Sean Owen</author>
    public sealed class FinderPattern : ResultPoint
    {
        internal FinderPattern(float posX, float posY, float estimatedModuleSize)
           : this(posX, posY, estimatedModuleSize, 1)
        {
            EstimatedModuleSize = estimatedModuleSize;
            Count = 1;
        }

        internal FinderPattern(float posX, float posY, float estimatedModuleSize, int count)
           : base(posX, posY)
        {
            EstimatedModuleSize = estimatedModuleSize;
            Count = count;
        }

        /// <summary>
        /// Gets the size of the estimated module.
        /// </summary>
        /// <value>
        /// The size of the estimated module.
        /// </value>
        public float EstimatedModuleSize { get; }
        internal int Count { get; }

        /// <summary> <p>Determines if this finder pattern "about equals" a finder pattern at the stated
        /// position and size -- meaning, it is at nearly the same center with nearly the same size.</p>
        /// </summary>
        internal bool AboutEquals(float moduleSize, float i, float j)
        {
            if (Math.Abs(i - Y) <= moduleSize && Math.Abs(j - X) <= moduleSize)
            {
                float moduleSizeDiff = Math.Abs(moduleSize - EstimatedModuleSize);
                return moduleSizeDiff <= 1.0f || moduleSizeDiff <= EstimatedModuleSize;

            }
            return false;
        }

        /// <summary>
        /// Combines this object's current estimate of a finder pattern position and module size
        /// with a new estimate. It returns a new {@code FinderPattern} containing a weighted average
        /// based on count.
        /// </summary>
        /// <param name="i">The i.</param>
        /// <param name="j">The j.</param>
        /// <param name="newModuleSize">New size of the module.</param>
        /// <returns></returns>
        internal FinderPattern CombineEstimate(float i, float j, float newModuleSize)
        {
            int combinedCount = Count + 1;
            float combinedX = (Count * X + j) / combinedCount;
            float combinedY = (Count * Y + i) / combinedCount;
            float combinedModuleSize = (Count * EstimatedModuleSize + newModuleSize) / combinedCount;
            return new FinderPattern(combinedX, combinedY, combinedModuleSize, combinedCount);
        }
    }
}