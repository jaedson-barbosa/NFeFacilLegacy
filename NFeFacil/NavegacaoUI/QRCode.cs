using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using ZXing.Mobile;

namespace NFeFacil.NavegacaoUI
{
    internal static class QRCode
    {
        public static ImageSource GerarQR(string valor, int largura = 400, int altura = 400)
        {
            return new BarcodeWriter
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = largura,
                    Height = altura,
                    Margin = 0
                }
            }.Write(valor);
        }

        public async static Task<string> DecodificarQRAsync()
        {
            var resposta = await new MobileBarcodeScanner
            {
                UseCustomOverlay = false,
                TopText = "Coloque a câmera em frente ao código QR",
                BottomText = "A câmera irá lê-lo automaticamente"
            }.Scan();
            return resposta.Text;
        }
    }
}
