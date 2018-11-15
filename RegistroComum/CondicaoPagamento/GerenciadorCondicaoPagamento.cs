using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace RegistroComum.CondicaoPagamento
{
    public static class GerenciadorCondicaoPagamento
    {
        public async static Task Salvar(IEnumerable<string> condicoes)
        {
            var pasta = ApplicationData.Current.RoamingFolder;
            var arquivo = await pasta.CreateFileAsync("condicoes", CreationCollisionOption.ReplaceExisting);
            var stream = await arquivo.OpenStreamForWriteAsync();
            using (var escritor = new StreamWriter(stream))
            {
                foreach (var linha in condicoes)
                    escritor.WriteLine(linha);
                await escritor.FlushAsync();
                escritor.Close();
            }
        }

        public async static Task<IEnumerable<string>> Obter()
        {
            var pasta = ApplicationData.Current.RoamingFolder;
            try
            {
                var arquivo = await pasta.GetFileAsync("condicoes");
                var stream = await arquivo.OpenStreamForReadAsync();
                List<string> condicoes = new List<string>();
                using (var leitor = new StreamReader(stream))
                {
                    while (!leitor.EndOfStream)
                        condicoes.Add(leitor.ReadLine());
                    leitor.Close();
                }
                return condicoes;
            }
            catch (Exception)
            {
                return new string[0];
            }
        }
    }
}
