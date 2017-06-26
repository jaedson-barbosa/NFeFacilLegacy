using BibliotecaCentral;
using BibliotecaCentral.IBGE;
using System.Collections.ObjectModel;
using System.Linq;

namespace NFeFacil.ViewModel
{
    public sealed class DadosEstadosParaView
    {
        public ObservableCollection<Estado> EstadosCompletos { get; }
        public ObservableCollection<string> Siglas { get; }
        public ObservableCollection<string> SiglasExpandida { get; }
        public ObservableCollection<string> Nomes { get; }

        public DadosEstadosParaView()
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
