using OptimizedZXing.QR.Internal;

namespace OptimizedZXing.QR
{
    internal class QRCodeReader : IReader
    {
        public Result Decode(BinaryBitmap image)
        {
            var blackMatrix = image.GetMatrix();
            if (image == null || blackMatrix == null)
            {
                // something is wrong with the image
                return null;
            }
            var detectorResult = new Detector(blackMatrix).Detect();
            if (detectorResult == null)
                return null;
            var decoder = new Decoder();
            DecoderResult decoderResult = decoder.Decode(detectorResult.Bits);
            ResultPoint[]  points = detectorResult.Points;
            if (decoderResult == null)
                return null;

            // If the code was mirrored: swap the bottom-left and the top-right points.
            if (decoderResult.Other is QRCodeDecoderMetaData data)
            {
                data.ApplyMirroredCorrection(points);
            }

            return new Result(decoderResult.Text, decoderResult.RawBytes, points, BarcodeFormat.QR_CODE);
        }
    }
}