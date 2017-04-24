using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace BibliotecaCentral
{
    internal static class Extensoes
    {
        public static XElement ToXElement<T>(this object obj, string nameSpace = "http://www.portalfiscal.inf.br/nfe") => ToXElement(obj, typeof(T), nameSpace);

        public static XElement ToXElement(this object obj, Type T, string nameSpace = "http://www.portalfiscal.inf.br/nfe")
        {
            var memoryStream = new MemoryStream();
            using (TextWriter streamWriter = new StreamWriter(memoryStream))
            {
                var name = new XmlSerializerNamespaces();
                name.Add(string.Empty, string.Empty);
                name.Add(string.Empty, nameSpace);
                var xmlSerializer = new XmlSerializer(T);
                xmlSerializer.Serialize(streamWriter, obj, name);
                return XElement.Parse(Encoding.UTF8.GetString(memoryStream.ToArray()));
            }
        }

        public static T FromXElement<T>(this Stream streamXMl)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            return (T)xmlSerializer.Deserialize(streamXMl);
        }

        public static T FromXElement<T>(this XElement xElement)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            return (T)xmlSerializer.Deserialize(xElement.CreateReader());
        }

        public static double ToDouble(this string str)
        {
            return string.IsNullOrEmpty(str) ? 0 : double.Parse(str);
        }

        public static string ToStringPersonalizado(this DateTime dataHora)
        {
            double horas = TimeZoneInfo.Local.BaseUtcOffset.TotalHours;
            string total = "yyyy-MM-ddThh:mm:ss";
            if (horas < 0) total += '-';
            total += $"{Math.Abs(horas).ToString().PadLeft(2, '0')}:00";
            return dataHora.ToString(total);
        }

        public static ObservableCollection<T> GerarObs<T>(this IEnumerable<T> aqui)
        {
            return new ObservableCollection<T>(aqui);
        }

        public static ObservableCollection<T> ObterItens<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().GerarObs();
        }
    }
}
