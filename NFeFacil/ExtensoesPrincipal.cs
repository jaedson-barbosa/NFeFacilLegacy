using NFeFacil.ItensBD;
using NFeFacil.Log;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Windows.Storage.Streams;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace NFeFacil
{
    internal static class ExtensoesPrincipal
    {
        public static XElement ToXElement<T>(this object obj, string nameSpace = "http://www.portalfiscal.inf.br/nfe") => ToXElement(obj, typeof(T), nameSpace);

        public static XElement ToXElement(this object obj, Type T, string nameSpace = "http://www.portalfiscal.inf.br/nfe")
        {
            using (var memoryStream = new MemoryStream())
            {
                var name = new XmlSerializerNamespaces();
                name.Add(string.Empty, string.Empty);
                name.Add(string.Empty, nameSpace);
                var xmlSerializer = new XmlSerializer(T);
                xmlSerializer.Serialize(memoryStream, obj, name);
                memoryStream.Position = 0;
                return XElement.Load(memoryStream);
            }
        }

        public static T FromXElement<T>(this Stream streamXMl)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            return (T)xmlSerializer.Deserialize(streamXMl);
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

        public static ObservableCollection<T> ObterItens<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().GerarObs();
        }

        internal static string ObterRecurso(string recurso)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            return loader.GetString(recurso);
        }

        static ILog Log = Popup.Current;

        internal static void ManipularErro(this Exception erro)
        {
            Log.Escrever(TitulosComuns.Erro, $"{erro.Message}\r\n" +
                $"Detalhes adicionais: {erro.InnerException?.Message ?? "Não há detalhes"}");
        }

        internal static double CentimeterToPixel(double Centimeter)
        {
            const double fator = 96 / 2.54;
            return Centimeter * fator;
        }

        internal static GridLength CentimeterToLength(double Centimeter)
        {
            return new GridLength(CentimeterToPixel(Centimeter));
        }

        public static void AddBloco(this RichTextBlock visualizacao, string titulo, params (string, string)[] filhos)
        {
            const string EntreLabelTexto = ": ";
            var paragrafo = new Paragraph();
            AddInline(titulo, Estilo.TituloBloco);
            for (int i = 0; i < filhos.Length; i++)
            {
                var atual = filhos[i];
                if (!string.IsNullOrEmpty(atual.Item2))
                {
                    AddInline(atual.Item1 + EntreLabelTexto, Estilo.Label);
                    AddInline(atual.Item2, Estilo.Texto);
                }
            }
            visualizacao.Blocks.Add(paragrafo);

            void AddInline(string texto, Estilo estilo)
            {
                var run = new Run() { Text = texto };
                switch (estilo)
                {
                    case Estilo.TituloBloco:
                        run.FontSize = 16;
                        run.FontWeight = FontWeights.ExtraBlack;
                        break;
                    case Estilo.Label:
                        run.FontWeight = FontWeights.Bold;
                        break;
                }
                paragrafo.Inlines.Add(run);
                if (estilo != Estilo.Label)
                {
                    CriarQuebraDeLinha();
                }
            }

            void CriarQuebraDeLinha()
            {
                paragrafo.Inlines.Add(new LineBreak());
            }
        }

        public static async Task<ImageSource> GetSourceAsync(this Imagem imagem)
        {
            var retorno = new BitmapImage();
            using (var stream = new InMemoryRandomAccessStream())
            {
                await stream.WriteAsync(imagem.Bytes.AsBuffer());
                stream.Seek(0);
                retorno.SetSource(stream);
                return retorno;
            }
        }
    }

    enum Estilo
    {
        TituloBloco,
        Label,
        Texto
    }
}
