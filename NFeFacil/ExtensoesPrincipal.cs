using NFeFacil.Log;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace NFeFacil
{
    internal static class ExtensoesPrincipal
    {
        public static XElement ToXElement<T>(this object obj, string nameSpace = "http://www.portalfiscal.inf.br/nfe")
        {
            using (var memoryStream = new MemoryStream())
            {
                var name = new XmlSerializerNamespaces();
                name.Add(string.Empty, string.Empty);
                name.Add(string.Empty, nameSpace);
                var xmlSerializer = new XmlSerializer(typeof(T));
                xmlSerializer.Serialize(memoryStream, obj, name);
                memoryStream.Position = 0;
                return XElement.Load(memoryStream);
            }
        }

        public static T FromXElement<T>(this XNode xElement)
        {
            try
            {
                var xmlSerializer = new XmlSerializer(typeof(T));
                using (var reader = xElement.CreateReader())
                {
                    return (T)xmlSerializer.Deserialize(reader);
                }
            }
            catch (Exception e)
            {
                throw new ErroDesserializacao($"Ocorreu um erro ao desserializar o objeto de tipo {nameof(T)}", e, xElement);
            }
        }

        public static string ToStringPersonalizado(this DateTime dataHora)
        {
            double horas = TimeZoneInfo.Local.BaseUtcOffset.TotalHours;
            string total = "yyyy-MM-ddTHH:mm:ss";
            total = dataHora.ToString(total);
            if (horas < 0) total += '-';
            total += $"{Math.Abs(horas).ToString("00")}:00";
            return total;
        }

        public static ObservableCollection<T> GerarObs<T>(this IEnumerable<T> aqui)
        {
            return new ObservableCollection<T>(aqui);
        }

        internal static async void ManipularErro(this Exception erro)
        {
            if (erro is ErroDesserializacao dess)
            {
                var caixa = new MessageDialog($"{dess.Message}\r\n" +
                    $"Detalhes adicionais: {dess.InnerException.Message}\r\n" +
                    $"Você deseja exportar o XML para que seja feita a análise?", "Erro de desserialização");
                caixa.Commands.Add(new UICommand("Sim", x => dess.ExportarXML()));
                caixa.Commands.Add(new UICommand("Não"));
                await caixa.ShowAsync();
            }
            else
            {
                Popup.Current.Escrever(TitulosComuns.Erro, $"{erro.Message}\r\n" +
                    $"Detalhes adicionais: {erro.InnerException?.Message ?? "Não há detalhes"}");
            }
        }

        public static ImageSource GetSource(this byte[] imagem)
        {
            var retorno = new BitmapImage();
            using (var stream = new InMemoryRandomAccessStream())
            {
                new StreamWriter(stream.AsStreamForWrite()).Write(imagem);
                stream.Seek(0);
                retorno.SetSource(stream);
                return retorno;
            }
        }

        public static string AplicarMáscaraDocumento(string original)
        {
            if (string.IsNullOrEmpty(original)) return string.Empty;
            else if (original.Length == 14) // É CNPJ
                return $"{sub(0, 2)}.{sub(2, 3)}.{sub(5, 3)}/{sub(8, 4)}.{sub(12, 2)}";
            else if (original.Length == 11) // É CPF
                return $"{sub(0, 3)}.{sub(3, 3)}.{sub(6, 3)}-{sub(9, 2)}";
            else if (original.Length == 8) // É CEP
                return $"{sub(0, 5)}-{sub(5, 3)}";
            else return original;

            string sub(int start, int len) => original.Substring(start, len);
        }

        internal static double CMToPixel(double CM) => CM * (96 / 2.54);
        internal static GridLength CMToLength(double CM) => new GridLength(CMToPixel(CM));

        static CultureInfo defCult = CultureInfo.InvariantCulture;
        public static string ToStr(double valor, string format = "F2") => valor.ToString(format, defCult);
        public static double Parse(string str) => double.Parse(str, NumberStyles.Number, defCult);
        public static bool TryParse(string str, out double valor) => double.TryParse(str, NumberStyles.Number, defCult, out valor);
    }

    public class ErroDesserializacao : Exception
    {
        XNode XML { get; }

        public ErroDesserializacao(XNode xml) => XML = xml;
        public ErroDesserializacao(string message, XNode xml) : base(message) => XML = xml;
        public ErroDesserializacao(string message, Exception innerException, XNode xml) : base(message, innerException) => XML = xml;

        public async void ExportarXML()
        {
            var caixa = new FileSavePicker();
            caixa.FileTypeChoices.Add("Arquivo XML", new string[] { ".xml" });
            var arq = await caixa.PickSaveFileAsync();
            if (arq != null)
            {
                var stream = await arq.OpenStreamForWriteAsync();
                using (StreamWriter escritor = new StreamWriter(stream))
                {
                    await escritor.WriteAsync(XML.ToString());
                    await escritor.FlushAsync();
                }
            }
        }
    }
}
