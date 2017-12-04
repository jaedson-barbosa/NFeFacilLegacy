using NFeFacil.IBGE;
using System.Collections.ObjectModel;
using System.Linq;

namespace NFeFacil.ViewModel
{
    public sealed class DadosEstadosParaView
    {
        public static ObservableCollection<Estado> EstadosCompletos { get; private set; }
        public static ObservableCollection<string> Siglas { get; private set; }
        public static ObservableCollection<string> SiglasExpandida { get; private set; }
        public static ObservableCollection<string> Nomes { get; private set; }

        public static void Iniciar()
        {
            var estados = Estados.EstadosCache;
            EstadosCompletos = Estados.EstadosCache.GerarObs();
            Siglas = Estados.EstadosCache.Select(x => x.Sigla).GerarObs();
            Nomes = Estados.EstadosCache.Select(x => x.Nome).GerarObs();

            var siglas = new string[Siglas.Count + 1];
            Siglas.CopyTo(siglas, 0);
            siglas[siglas.Length - 1] = "EX";
            SiglasExpandida = siglas.GerarObs();
        }
    }
}
