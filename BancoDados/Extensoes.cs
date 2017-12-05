using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace NFeFacil
{
    static class Extensoes
    {
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
            return string.IsNullOrEmpty(str) ? 0 : double.Parse(str, CultureInfo.InvariantCulture);
        }

        public static Stream Retornar(object origem, string caminho)
        {
            var assembly = origem.GetType().GetTypeInfo().Assembly;
            return assembly.GetManifestResourceStream(caminho);
        }

        static CultureInfo culturaPadrao = CultureInfo.InvariantCulture;
        public static string ToStr(double valor) => valor.ToString("F2", culturaPadrao);
        public static double Parse(string str) => double.Parse(str, NumberStyles.Number, culturaPadrao);
    }
}
