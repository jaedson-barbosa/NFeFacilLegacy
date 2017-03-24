using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;

namespace NFeFacil.ViewModel
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
                PropertyChanged(this, new PropertyChangedEventArgs(parametros[i]));
            }
        }

        public bool GrupoSimplesNacional { get; private set; } = true;
        public bool GrupoRegimeNormal { get; private set; }

        private int regimeSelecionado;
        public int RegimeSelecionado
        {
            get { return regimeSelecionado; }
            set
            {
                regimeSelecionado = value;
                switch ((Regimes)value)
                {
                    case Regimes.SimplesNacional:
                        GrupoRegimeNormal = false;
                        GrupoSimplesNacional = true;
                        break;
                    case Regimes.RegimeNormal:
                        GrupoRegimeNormal = true;
                        GrupoSimplesNacional = false;
                        break;
                }
                OnPropertyChanged(nameof(GrupoRegimeNormal), nameof(GrupoSimplesNacional));
                Simples = null;
                Normal = null;
                OnPropertyChanged(nameof(Simples), nameof(Normal));
            }
        }

        private ComboBoxItem tipoICMSSimplesNacional;
        public ComboBoxItem TipoICMSSimplesNacional
        {
            get { return tipoICMSSimplesNacional; }
            set
            {
                tipoICMSSimplesNacional = value;
                var tipoICMS = value.Content as string;
                var tipoICMSInt = int.Parse(tipoICMS.Substring(0, 3));
                switch (tipoICMSInt)
                {
                    case 101:
                        AttVisibilidadeSimplesNacional(false, false, false, false, false, false, true, true);
                        Simples = new ICMSSN101();
                        break;
                    case 102:
                        AttVisibilidadeSimplesNacional(false, false, false, false, false, false, false, false);
                        Simples = new ICMSSN102();
                        break;
                    case 103:
                        AttVisibilidadeSimplesNacional(false, false, false, false, false, false, false, false);
                        Simples = new ICMSSN102();
                        break;
                    case 201:
                        AttVisibilidadeSimplesNacional(false, false, false, false, false, true, true, true);
                        Simples = new ICMSSN201();
                        break;
                    case 202:
                        AttVisibilidadeSimplesNacional(false, false, false, false, false, true, false, false);
                        Simples = new ICMSSN202();
                        break;
                    case 203:
                        AttVisibilidadeSimplesNacional(false, false, false, false, false, true, false, false);
                        Simples = new ICMSSN202();
                        break;
                    case 300:
                        AttVisibilidadeSimplesNacional(false, false, false, false, false, false, false, false);
                        Simples = new ICMSSN102();
                        break;
                    case 400:
                        AttVisibilidadeSimplesNacional(false, false, false, false, false, false, false, false);
                        Simples = new ICMSSN102();
                        break;
                    case 500:
                        AttVisibilidadeSimplesNacional(false, false, false, false, false, false, false, false);
                        Simples = new ICMSSN500();
                        break;
                    case 900:
                        AttVisibilidadeSimplesNacional(true, true, true, true, true, true, true, true);
                        Simples = new ICMSSN900();
                        break;
                }
                Simples.CSOSN = tipoICMSInt.ToString();
                OnPropertyChanged(nameof(Simples));
            }
        }

        public bool SimplesModalidadeBC { get; private set; }
        public bool SimplesPercentualRedução { get; private set; }
        public bool SimplesValorBC { get; private set; }
        public bool SimplesAlíquota { get; private set; }
        public bool SimplesValorICMS { get; private set; }
        public bool SimplesICMSST { get; private set; }
        public bool SimplesAliquotaAplicavel { get; private set; }
        public bool SimplesCreditoAproveitável { get; private set; }
        private void AttVisibilidadeSimplesNacional(bool modBC, bool pRedBC, bool vBC, bool pICMS, bool vICMS, bool ICMSST, bool pCredSN, bool vCredICMSSN)
        {
            SimplesModalidadeBC = modBC;
            SimplesPercentualRedução = pRedBC;
            SimplesValorBC = vBC;
            SimplesAlíquota = pICMS;
            SimplesValorICMS = vICMS;
            SimplesICMSST = ICMSST;
            SimplesAliquotaAplicavel = pCredSN;
            SimplesCreditoAproveitável = vCredICMSSN;
            OnPropertyChanged(nameof(SimplesModalidadeBC), nameof(SimplesPercentualRedução), nameof(SimplesValorBC),
                nameof(SimplesAlíquota), nameof(SimplesValorICMS), nameof(SimplesICMSST), nameof(SimplesAliquotaAplicavel),
                nameof(SimplesCreditoAproveitável));
        }

        private ComboBoxItem tipoICMSRegimeNormal;
        public ComboBoxItem TipoICMSRegimeNormal
        {
            get { return tipoICMSRegimeNormal; }
            set
            {
                tipoICMSRegimeNormal = value;
                var tipoICMS = value.Content as string;
                var tipoICMSString = tipoICMS.Substring(0, 2);
                var tipoICMSInt = int.Parse(tipoICMSString);
                switch (tipoICMSInt)
                {
                    case 0:
                        Normal = new ICMS00();
                        AttCamposNormal(true, false, true, true, true, false, false, false, false, false, false, false, false);
                        break;
                    case 10:
                        Normal = new ICMS10();
                        AttCamposNormal(true, false, true, true, true, true, false, false, false, false, false, false, false);
                        break;
                    case 20:
                        Normal = new ICMS20();
                        AttCamposNormal(true, true, true, true, true, false, false, false, true, true, false, false, false);
                        break;
                    case 30:
                        Normal = new ICMS30();
                        AttCamposNormal(false, false, false, false, false, true, false, false, true, true, false, false, false);
                        break;
                    case 40:
                        Normal = new ICMS40();
                        AttCamposNormal(false, false, false, false, false, false, false, false, true, true, false, false, false);
                        break;
                    case 41:
                        Normal = new ICMS41();
                        AttCamposNormal(false, false, false, false, false, false, false, false, true, true, false, false, false);
                        break;
                    case 50:
                        Normal = new ICMS50();
                        AttCamposNormal(false, false, false, false, false, false, false, false, true, true, false, false, false);
                        break;
                    case 51:
                        Normal = new ICMS51();
                        AttCamposNormal(true, true, true, true, true, false, false, false, false, false, true, true, true);
                        break;
                    case 60:
                        Normal = new ICMS60();
                        AttCamposNormal(false, false, false, false, false, false, true, true, false, false, false, false, false);
                        break;
                    case 70:
                        Normal = new ICMS70();
                        AttCamposNormal(true, true, true, true, true, true, false, false, false, true, false, false, false);
                        break;
                    case 90:
                        Normal = new ICMS90();
                        AttCamposNormal(true, true, true, true, true, true, false, false, true, true, false, false, false);
                        break;
                }
                Normal.CST = tipoICMSString;
                OnPropertyChanged(nameof(Normal));
            }
        }

        public bool NormalValorICMSDesonerado { get; private set; }
        public bool NormalMotivoDesoneração { get; private set; }
        public bool NormalModalidadeBCNormal { get; private set; }
        public bool NormalValorBCNormal { get; private set; }
        public bool NormalPercentualReduçãoNormal { get; private set; }
        public bool NormalAlíquotaNormal { get; private set; }
        public bool NormalValorICMSNormal { get; private set; }
        public bool NormalICMSSTNormal { get; private set; }
        public bool NormalValorBCICMSSTRetido { get; private set; }
        public bool NormalValorICMSRetido { get; private set; }
        public bool NormalValorICMSOperacao { get; private set; }
        public bool NormalPercentualDiferimento { get; private set; }
        public bool NormalValorICMSDiferido { get; private set; }

        public Imposto ImpostoBruto => new ICMS()
        {
            Corpo = (ComumICMS)Simples ?? Normal
        };

        private void AttCamposNormal(bool modBC, bool pRedBC, bool vBC, bool pICMS, bool vICMS, bool ICMSST, bool vBCSTRet, bool vICMSSTRet, bool motDesICMS, bool vICMSDeson, bool vICMSOp, bool pDif, bool vICMSDif)
        {
            NormalValorICMSDesonerado = vICMSDeson;
            NormalMotivoDesoneração = motDesICMS;
            NormalModalidadeBCNormal = modBC;
            NormalValorBCNormal = vBC;
            NormalPercentualReduçãoNormal = pRedBC;
            NormalAlíquotaNormal = pICMS;
            NormalValorICMSNormal = vICMS;
            NormalICMSSTNormal = ICMSST;
            NormalValorBCICMSSTRetido = vBCSTRet;
            NormalValorICMSRetido = vICMSSTRet;
            NormalValorICMSOperacao = vICMSOp;
            NormalPercentualDiferimento = pDif;
            NormalValorICMSDiferido = vICMSDif;
            OnPropertyChanged(nameof(NormalValorICMSDesonerado), nameof(NormalMotivoDesoneração),
                nameof(NormalModalidadeBCNormal), nameof(NormalValorBCNormal), nameof(NormalPercentualReduçãoNormal),
                nameof(NormalAlíquotaNormal), nameof(NormalValorICMSNormal), nameof(NormalICMSSTNormal),
                nameof(NormalValorBCICMSSTRetido), nameof(NormalValorICMSRetido), nameof(NormalValorICMSOperacao),
                nameof(NormalPercentualDiferimento), nameof(NormalValorICMSDiferido));
        }

        private enum Regimes
        {
            SimplesNacional,
            RegimeNormal
        }
    }
}
