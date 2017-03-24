using System.Collections.ObjectModel;
using System.Linq;

namespace NFeFacil.ViewModel
{
    public sealed class DadosEstadosParaView
    {
        public ObservableCollection<Estado> EstadosCompletos { get; } = Estados.Buscar().GerarObs();
        public ObservableCollection<string> Siglas
        {
            get { return EstadosCompletos.Select(x => x.Sigla).GerarObs(); }
        }
        public ObservableCollection<string> SiglasExpandida
        {
            get
            {
                var estados = Siglas;
                estados.Add("EX");
                return estados;
            }
        }
        public ObservableCollection<string> Nomes
        {
            get { return EstadosCompletos.Select(x => x.Nome).GerarObs(); }
        }
        public ObservableCollection<ushort> Codigos
        {
            get { return EstadosCompletos.Select(x => x.Codigo).GerarObs(); }
        }
    }
}
