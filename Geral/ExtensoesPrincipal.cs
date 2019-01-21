using BaseGeral.Log;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace BaseGeral
{
    public static class ExtensoesPrincipal
    {
        public static XElement ToXElement(this object obj, string nameSpace = "http://www.portalfiscal.inf.br/nfe")
        {
            try
            {
                var doc = new XDocument();
                using (var escritor = doc.CreateWriter())
                {
                    var name = new XmlSerializerNamespaces();
                    name.Add(string.Empty, string.Empty);
                    name.Add(string.Empty, nameSpace);
                    var xmlSerializer = new XmlSerializer(obj.GetType());
                    xmlSerializer.Serialize(escritor, obj, name);
                }
                return doc.Root;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static XElement ToXElement<T>(this object obj, string nameSpace = "http://www.portalfiscal.inf.br/nfe")
        {
            try
            {
                var doc = new XDocument();
                using (var escritor = doc.CreateWriter())
                {
                    var name = new XmlSerializerNamespaces();
                    name.Add(string.Empty, string.Empty);
                    name.Add(string.Empty, nameSpace);
                    var xmlSerializer = new XmlSerializer(typeof(T));
                    xmlSerializer.Serialize(escritor, obj, name);
                }
                return doc.Root;
            }
            catch (Exception e)
            {
                throw e;
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

        public static T FromString<T>(this string str)
        {
            try
            {
                var xmlSerializer = new XmlSerializer(typeof(T));
                using (var reader = new StringReader(str))
                {
                    return (T)xmlSerializer.Deserialize(reader);
                }
            }
            catch (Exception e)
            {
                throw new ErroDesserializacao($"Ocorreu um erro ao desserializar o objeto de tipo {nameof(T)}", e, str);
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

        public static void Sort<T, K>(this ObservableCollection<T> observable, Func<T, K> keySelector, bool ignoreFirst)
        {
            int ptr = 0, adicional = ignoreFirst ? 1 : 0;
            List<T> sorted = observable.Skip(adicional).OrderBy(keySelector).ToList();
            while (ptr < sorted.Count)
            {
                if (!observable[ptr + adicional].Equals(sorted[ptr]))
                {
                    T t = observable[ptr + adicional];
                    observable.RemoveAt(ptr + adicional);
                    observable.Insert(sorted.IndexOf(t) + adicional, t);
                }
                else ptr++;
            }
        }

        public static async void ManipularErro(this Exception erro)
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
                var otherStream = stream.AsStreamForWrite();
                otherStream.Write(imagem, 0, imagem.Length);
                stream.Seek(0);
                retorno.SetSource(stream);
            }
            return retorno;
        }

        public async static Task<ImageSource> GetSourceAsync(this byte[] imagem)
        {
            var retorno = new BitmapImage();
            using (var stream = new InMemoryRandomAccessStream())
            {
                await stream.WriteAsync(imagem.AsBuffer());
                stream.Seek(0);
                retorno.SetSource(stream);
            }
            return retorno;
        }

        public static string AplicarMáscaraDocumento(string original)
        {
            if (string.IsNullOrEmpty(original)) return string.Empty;
            else if (original.Length == 14) // É CNPJ
                return $"{sub(0, 2)}.{sub(2, 3)}.{sub(5, 3)}/{sub(8, 4)}-{sub(12, 2)}";
            else if (original.Length == 11) // É CPF
                return $"{sub(0, 3)}.{sub(3, 3)}.{sub(6, 3)}-{sub(9, 2)}";
            else if (original.Length == 8) // É CEP
                return $"{sub(0, 5)}-{sub(5, 3)}";
            else return original;

            string sub(int start, int len) => original.Substring(start, len);
        }

        public static string[] Concat(this string[] original, params string[] extras)
        {
            var inicial = original.Length;
            var tot = original.Length + extras.Length;

            var retorno = new string[tot];
            for (int i = 0; i < inicial; i++) retorno[i] = original[i];
            for (int i = 0; i < extras.Length; i++) retorno[i + inicial] = extras[i];
            return retorno;
        }

        public static double CMToPixel(double CM) => CM * (96 / 2.54);
        public static GridLength CMToLength(double CM) => new GridLength(CMToPixel(CM));

        static readonly CultureInfo defCult = CultureInfo.InvariantCulture;
        public static string ToStr(double valor, string format = "F2") => valor.ToString(format, defCult);
        public static double Parse(string str) => double.Parse(str, NumberStyles.Number, defCult);
        public static bool TryParse(string str, out double valor) => double.TryParse(str, NumberStyles.Number, defCult, out valor);
        public static double TryParse(string str) { TryParse(str, out double valor); return valor; }

        public static string[] ToArray(this string str) => new string[1] { str };
        public static string[] ToArray(this string str0, string str1) => new string[2] { str0, str1 };

        public static string IPToCodigo(this string ip)
        {
            return string.Concat(ip.Split(".").Select(x => byte.Parse(x).ToString("000")));
        }

        public static string CodigoToIP(this string codigo)
        {
            string temp = string.Empty;
            for (int i = 0; i < 4; i++)
            {
                temp += byte.Parse(codigo.Substring(i * 3, 3)).ToString();
                if (i < 3) temp += '.';
            }
            return temp;
        }
    }

    public class ErroDesserializacao : Exception
    {
        XNode XML { get; }
        string StrXML { get; }

        public ErroDesserializacao(string message, Exception innerException, XNode xml) : base(message, innerException) => XML = xml;
        public ErroDesserializacao(string message, Exception innerException, string strXml) : base(message, innerException) => StrXML = strXml;

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
                    await escritor.WriteAsync(StrXML ?? XML.ToString());
                    await escritor.FlushAsync();
                }
            }
        }
    }
}
