using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using System.ComponentModel;

namespace NFeFacil.ViewModel.ImpostosProduto
{
    public sealed class ICMSDataContext : INotifyPropertyChanged, IImpostoDataContext
    {
        public ISimplesNacional Simples { get; private set; }
        public IRegimeNormal Normal { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(params string[] parametros)
        {
            for (int i = 0; i < parametros.Length; i++)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(parametros[i]));
            }
        }

        public bool GrupoSimplesNacional { get; private set; } = true;
        public bool GrupoRegimeNormal { get; private set; }

        private int regimeSelecionado;
        public int RegimeSelecionado
        {
            get => regimeSelecionado;
            set
            {
                regimeSelecionado = value;
                GrupoRegimeNormal = !(GrupoSimplesNacional = value == 0);
                OnPropertyChanged(nameof(GrupoRegimeNormal), nameof(GrupoSimplesNacional));
                Simples = null;
                Normal = null;
                OnPropertyChanged(nameof(Simples), nameof(Normal));
            }
        }

        public int TipoICMSSimplesNacional
        {
            get => Simples != null ? int.Parse(Simples.CSOSN) : -1;
            set
            {
                AttVisibilidadeSimplesNacional(VisibilidadesSimplesNacional.Buscar(value));
                switch (value)
                {
                    case 101:
                        Simples = new ICMSSN101();
                        break;
                    case 102:
                        Simples = new ICMSSN102();
                        break;
                    case 103:
                        Simples = new ICMSSN102();
                        break;
                    case 201:
                        Simples = new ICMSSN201();
                        break;
                    case 202:
                        Simples = new ICMSSN202();
                        break;
                    case 203:
                        Simples = new ICMSSN202();
                        break;
                    case 300:
                        Simples = new ICMSSN102();
                        break;
                    case 400:
                        Simples = new ICMSSN102();
                        break;
                    case 500:
                        Simples = new ICMSSN500();
                        break;
                    case 900:
                        Simples = new ICMSSN900();
                        break;
                }
                Simples.CSOSN = value.ToString("000");
                OnPropertyChanged(nameof(Simples));
            }
        }

        public bool SimplesGrupoInicio { get; private set; }
        public bool SimplesICMSST { get; private set; }
        public bool SimplesGrupoFim { get; private set; }
        private void AttVisibilidadeSimplesNacional(VisibilidadeSimplesNacional visibilidade)
        {
            SimplesGrupoInicio = visibilidade.GrupoInicio;
            SimplesICMSST = visibilidade.ICMSST;
            SimplesGrupoFim = visibilidade.GrupoFim;
            OnPropertyChanged(nameof(SimplesGrupoInicio), nameof(SimplesICMSST), nameof(SimplesGrupoFim));
        }

        public int TipoICMSRegimeNormal
        {
            get => Normal != null ? int.Parse(Normal.CST) : -1;
            set
            {
                AttCamposNormal(VisibilidadesRegimeNormal.Buscar(value));
                switch (value)
                {
                    case 0:
                        Normal = new ICMS00();
                        break;
                    case 10:
                        Normal = new ICMS10();
                        break;
                    case 1010:
                        Normal = new ICMSPart();
                        break;
                    case 20:
                        Normal = new ICMS20();
                        break;
                    case 30:
                        Normal = new ICMS30();
                        break;
                    case 40:
                        Normal = new ICMS40();
                        break;
                    case 41:
                        Normal = new ICMS41();
                        break;
                    case 4141:
                        Normal = new ICMSST();
                        break;
                    case 50:
                        Normal = new ICMS50();
                        break;
                    case 51:
                        Normal = new ICMS51();
                        break;
                    case 60:
                        Normal = new ICMS60();
                        break;
                    case 70:
                        Normal = new ICMS70();
                        break;
                    case 90:
                        Normal = new ICMS90();
                        break;
                    case 9090:
                        Normal = new ICMSPart();
                        break;
                }
                Normal.CST = value.ToString("00");
                if (Normal.CST.Length > 2) Normal.CST = Normal.CST.Remove(2);
                OnPropertyChanged(nameof(Normal));
            }
        }

        public bool NormalICMSDesonerado { get; private set; }
        public bool NormalGrupoInicio { get; private set; }
        public bool NormalPercentualReduçãoNormal { get; private set; }
        public bool NormalICMSSTNormal { get; private set; }
        public bool NormalGrupoMeio { get; private set; }
        public bool NormalGrupoFim { get; private set; }
        public bool NormalICMSST { get; private set; }
        public bool NormalICMSPart { get; private set; }
        private void AttCamposNormal(VisibilidadeRegimeNormal normal)
        {
            NormalICMSDesonerado = normal.IcmsDeson;
            NormalGrupoInicio = normal.GrupoInicio;
            NormalPercentualReduçãoNormal = normal.PRedBC;
            NormalICMSSTNormal = normal.ICMSST;
            NormalGrupoMeio = normal.GrupoMeio;
            NormalGrupoFim = normal.GrupoFim;
            NormalICMSST = normal.NormalICMSST;
            NormalICMSPart = normal.NormalICMSPart;
            OnPropertyChanged(nameof(NormalICMSDesonerado),
                nameof(NormalGrupoInicio), nameof(NormalPercentualReduçãoNormal),
                nameof(NormalICMSSTNormal), nameof(NormalGrupoMeio), nameof(NormalGrupoFim),
                nameof(NormalICMSST), nameof(NormalICMSPart));
        }

        public Imposto ImpostoBruto => new ICMS()
        {
            Corpo = (ComumICMS)Simples ?? (ComumICMS)Normal
        };

        public ICMSDataContext() { }
        public ICMSDataContext(ICMS pai)
        {
            if (pai.Corpo is ISimplesNacional simples)
            {
                RegimeSelecionado = (int)Modalidades.Simples;
                Simples = simples;
                var csosn = int.Parse(simples.CSOSN);
                var visibilidade = VisibilidadesSimplesNacional.Buscar(csosn);
                AttVisibilidadeSimplesNacional(visibilidade);
                OnPropertyChanged(nameof(Simples));
            }
            else if (pai.Corpo is IRegimeNormal normal)
            {
                RegimeSelecionado = (int)Modalidades.Normal;
                Normal = normal;
                var cst = int.Parse(normal.CST);
                var visibilidade = VisibilidadesRegimeNormal.Buscar(cst);
                AttCamposNormal(visibilidade);
                OnPropertyChanged(nameof(Normal));
            }
        }

        private enum Modalidades { Simples, Normal }
    }
}
