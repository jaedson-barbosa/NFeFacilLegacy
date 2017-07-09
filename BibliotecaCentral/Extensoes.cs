using BibliotecaCentral.Log;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace BibliotecaCentral
{
    public static class Extensoes
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

        public static T FromXElement<T>(this XNode xElement)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            using (var reader = xElement.CreateReader())
            {
                return (T)xmlSerializer.Deserialize(reader);
            }
        }

        public static double ToDouble(this string str)
        {
            return string.IsNullOrEmpty(str) ? 0 : double.Parse(str);
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

        static ILog Log = new Popup();

        internal static void ManipularErro(this Exception erro)
        {
            Log.Escrever(TitulosComuns.Erro, erro.Message);
        }
    }
}
