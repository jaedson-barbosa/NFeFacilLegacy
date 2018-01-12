using System;
using Windows.Storage;

namespace NFeFacil
{
    static class AssistenteConfig
    {
        static ApplicationDataContainer Pasta = ApplicationData.Current.LocalSettings;

        public static T Get<T>(string nome, T padrao) where T : struct
        {
            try
            {
                var contem = Pasta.Values.ContainsKey(nome);
                return contem ? (T)Pasta.Values[nome] : padrao;
            }
            catch (Exception) { return padrao; }
        }

        public static void Set(string nome, object valor)
        {
            var contem = Pasta.Values.ContainsKey(nome);
            try
            {
                if (contem) Pasta.Values[nome] = valor;
                else Pasta.Values.Add(nome, valor);
            }
            catch (Exception)
            {
                try
                {
                    if (contem) Pasta.Values.Remove(nome);
                    Pasta.Values.Add(nome, valor);
                }
                catch (Exception) { }
            }
        }
    }
}
