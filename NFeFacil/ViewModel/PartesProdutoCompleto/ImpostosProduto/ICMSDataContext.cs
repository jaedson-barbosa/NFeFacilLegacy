using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using System.ComponentModel;

namespace NFeFacil.ViewModel.PartesProdutoCompleto.ImpostosProduto
{
    public sealed class ICMSDataContext : INotifyPropertyChanged, IImpostoDataContext
    {
        public SimplesNacional Simples { get; private set; }
        public RegimeNormal Normal { get; private set; }

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

        private string tipoICMSSimplesNacional;
        public string TipoICMSSimplesNacional
        {
            get => tipoICMSSimplesNacional;
            set
            {
                tipoICMSSimplesNacional = value;
                var tipoICMSInt = int.Parse(value.Substring(0, 3));
                switch (tipoICMSInt)
                {
                    case 101:
                        AttVisibilidadeSimplesNacional(false, false, true);
                        Simples = new ICMSSN101();
                        break;
                    case 102:
                        AttVisibilidadeSimplesNacional(false, false, false);
                        Simples = new ICMSSN102();
                        break;
                    case 103:
                        AttVisibilidadeSimplesNacional(false, false, false);
                        Simples = new ICMSSN102();
                        break;
                    case 201:
                        AttVisibilidadeSimplesNacional(false, true, true);
                        Simples = new ICMSSN201();
                        break;
                    case 202:
                        AttVisibilidadeSimplesNacional(false, true, false);
                        Simples = new ICMSSN202();
                        break;
                    case 203:
                        AttVisibilidadeSimplesNacional(false, true, false);
                        Simples = new ICMSSN202();
                        break;
                    case 300:
                        AttVisibilidadeSimplesNacional(false, false, false);
                        Simples = new ICMSSN102();
                        break;
                    case 400:
                        AttVisibilidadeSimplesNacional(false, false, false);
                        Simples = new ICMSSN102();
                        break;
                    case 500:
                        AttVisibilidadeSimplesNacional(false, false, false);
                        Simples = new ICMSSN500();
                        break;
                    case 900:
                        AttVisibilidadeSimplesNacional(true, true, true);
                        Simples = new ICMSSN900();
                        break;
                }
                Simples.CSOSN = tipoICMSInt.ToString();
                OnPropertyChanged(nameof(Simples));
            }
        }

        public bool SimplesGrupoInicio { get; private set; }
        public bool SimplesICMSST { get; private set; }
        public bool SimplesGrupoFim { get; private set; }
        private void AttVisibilidadeSimplesNacional(bool grupoInicio, bool ICMSST, bool grupoFim)
        {
            SimplesGrupoInicio = grupoInicio;
            SimplesICMSST = ICMSST;
            SimplesGrupoFim = grupoFim;
            OnPropertyChanged(nameof(SimplesGrupoInicio), nameof(SimplesICMSST), nameof(SimplesGrupoFim));
        }

        private string tipoICMSRegimeNormal;
        public string TipoICMSRegimeNormal
        {
            get => tipoICMSRegimeNormal;
            set
            {
                tipoICMSRegimeNormal = value;
                var tipoICMSString = value.Substring(0, 2);
                var tipoICMSInt = int.Parse(tipoICMSString);
                switch (tipoICMSInt)
                {
                    case 0:
                        Normal = new ICMS00();
                        AttCamposNormal(false, true, false, false, false, false, false);
                        break;
                    case 10:
                        Normal = new ICMS10();
                        AttCamposNormal(false, true, true, false, false, false, false);
                        break;
                    case 20:
                        Normal = new ICMS20();
                        AttCamposNormal(true, true, false, false, true, true, false);
                        break;
                    case 30:
                        Normal = new ICMS30();
                        AttCamposNormal(false, false, true, false, true, true, false);
                        break;
                    case 40:
                        Normal = new ICMS40();
                        AttCamposNormal(false, false, false, false, true, true, false);
                        break;
                    case 41:
                        Normal = new ICMS41();
                        AttCamposNormal(false, false, false, false, true, true, false);
                        break;
                    case 50:
                        Normal = new ICMS50();
                        AttCamposNormal(false, false, false, false, true, true, false);
                        break;
                    case 51:
                        Normal = new ICMS51();
                        AttCamposNormal(true, true, false, false, false, false, true);
                        break;
                    case 60:
                        Normal = new ICMS60();
                        AttCamposNormal(false, false, false, true, false, false, false);
                        break;
                    case 70:
                        Normal = new ICMS70();
                        AttCamposNormal(true, true, true, false, false, true, false);
                        break;
                    case 90:
                        Normal = new ICMS90();
                        AttCamposNormal(true, true, true, false, true, true, false);
                        break;
                }
                Normal.CST = tipoICMSString;
                OnPropertyChanged(nameof(Normal));
            }
        }

        public bool NormalValorICMSDesonerado { get; private set; }
        public bool NormalMotivoDesoneração { get; private set; }
        public bool NormalGrupoInicio { get; private set; }
        public bool NormalPercentualReduçãoNormal { get; private set; }
        public bool NormalICMSSTNormal { get; private set; }
        public bool NormalGrupoMeio { get; private set; }
        public bool NormalGrupoFim { get; private set; }

        public Imposto ImpostoBruto => new ICMS()
        {
            Corpo = (ComumICMS)Simples ?? Normal
        };

        private void AttCamposNormal(bool pRedBC, bool grupoInicio, bool ICMSST, bool grupoMeio, bool motDesICMS, bool vICMSDeson, bool grupoFim)
        {
            NormalValorICMSDesonerado = vICMSDeson;
            NormalMotivoDesoneração = motDesICMS;
            NormalGrupoInicio = grupoInicio;
            NormalPercentualReduçãoNormal = pRedBC;
            NormalICMSSTNormal = ICMSST;
            NormalGrupoMeio = grupoMeio;
            NormalGrupoFim = grupoFim;
            OnPropertyChanged(nameof(NormalValorICMSDesonerado), nameof(NormalMotivoDesoneração),
                nameof(NormalGrupoInicio), nameof(NormalPercentualReduçãoNormal),
                nameof(NormalICMSSTNormal), nameof(NormalGrupoMeio), nameof(NormalGrupoFim));
        }
    }
}
