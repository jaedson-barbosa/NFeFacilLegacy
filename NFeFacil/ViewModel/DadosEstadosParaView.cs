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
        public ObservableCollection<ushort> Codigos { get; }

        public DadosEstadosParaView()
        {
            var estados = Estados.EstadosCache;
            EstadosCompletos = Estados.EstadosCache.GerarObs();
            Siglas = Estados.EstadosCache.Select(x => x.Sigla).GerarObs();
            Nomes = Estados.EstadosCache.Select(x => x.Nome).GerarObs();
            Codigos = Estados.EstadosCache.Select(x => x.Codigo).GerarObs();

            var siglas = Siglas;
            siglas.Add("EX");
            SiglasExpandida = siglas;
        }
    }
}
