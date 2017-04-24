using BibliotecaCentral;
using BibliotecaCentral.IBGE;
using System.Collections.ObjectModel;
using System.Linq;

namespace NFeFacil.ViewModel
{
    public sealed class DadosEstadosParaView
    {
        public ObservableCollection<Estado> EstadosCompletos { get; } = Estados.EstadosCache.GerarObs();
        public ObservableCollection<string> Siglas => Estados.EstadosCache.Select(x => x.Sigla).GerarObs();
        public ObservableCollection<string> SiglasExpandida
        {
            get
            {
                var estados = Siglas;
                estados.Add("EX");
                return estados;
            }
        }
        public ObservableCollection<string> Nomes => Estados.EstadosCache.Select(x => x.Nome).GerarObs();
        public ObservableCollection<ushort> Codigos => Estados.EstadosCache.Select(x => x.Codigo).GerarObs();

        public DadosEstadosParaView()
        {

        }
    }
}
