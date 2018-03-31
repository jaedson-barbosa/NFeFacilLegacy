using BaseGeral;
using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;

namespace Venda.Impostos.DetalhamentoICMS
{
    public sealed class Processamento : ProcessamentoImposto
    {
        IDadosICMS dados;

        public override ImpostoBase[] Processar(DetalhesProdutos prod)
        {
            var imposto = new ICMS
            {
                Corpo = (ComumICMS)dados.Processar(prod)
            };
            return new ImpostoBase[1] { imposto };
        }

        public override bool ValidarDados() => dados != null;

        public override void ProcessarEntradaDados(object Tela)
        {
            if (Detalhamento is Detalhamento det)
            {
                var normal = DefinicoesTemporarias.EmitenteAtivo.RegimeTributario == 3;
                var (rn, sn) = ProcessarTela(Tela, normal, det.TipoICMSSN, det.TipoICMSRN, det.Origem);
                dados = (IDadosICMS)rn ?? sn;
            }
            else if (Detalhamento is DadoPronto pronto)
            {
                ProcessarDadosProntos(pronto.ImpostoPronto);
            }
        }

        public static (DadosRN.BaseRN rn, DadosSN.BaseSN sn) ProcessarTela(object Tela, bool normal, string CSOSN, string CST, int origem)
        {
            if (!normal)
            {
                var csosn = int.Parse(CSOSN);
                DadosSN.BaseSN baseSN;
                switch (csosn)
                {
                    case 101:
                        var tipo101 = (TelasSN.Tipo101)Tela;
                        baseSN = new DadosSN.Tipo101(tipo101);
                        break;
                    case 201:
                        var tipo201 = (TelasSN.Tipo201)Tela;
                        baseSN = new DadosSN.Tipo201(tipo201);
                        break;
                    case 202:
                        var tipo202 = (TelasSN.Tipo202)Tela;
                        baseSN = new DadosSN.Tipo202(tipo202);
                        break;
                    case 203:
                        tipo202 = (TelasSN.Tipo202)Tela;
                        baseSN = new DadosSN.Tipo202(tipo202);
                        break;
                    case 500:
                        var tipo500 = (TelasSN.Tipo500)Tela;
                        baseSN = new DadosSN.Tipo500(tipo500);
                        break;
                    case 900:
                        var tipo900 = (TelasSN.Tipo900)Tela;
                        baseSN = new DadosSN.Tipo900(tipo900);
                        break;
                    default:
                        baseSN = new DadosSN.TipoNT();
                        break;
                }
                baseSN.CSOSN = CSOSN;
                baseSN.Origem = origem;
                return (null, baseSN);
            }
            else
            {
                var cst = int.Parse(CST);
                DadosRN.BaseRN baseRN;
                switch (cst)
                {
                    case 0:
                        var tipo00 = (TelasRN.Tipo0)Tela;
                        baseRN = new DadosRN.Tipo0(tipo00);
                        break;
                    case 10:
                        var tipo10 = (TelasRN.Tipo10)Tela;
                        baseRN = new DadosRN.Tipo10(tipo10);
                        break;
                    case 1010:
                        var tipoPart = (TelasRN.TipoPart)Tela;
                        baseRN = new DadosRN.TipoPart(tipoPart);
                        break;
                    case 20:
                        var tipo20 = (TelasRN.Tipo20)Tela;
                        baseRN = new DadosRN.Tipo20(tipo20);
                        break;
                    case 30:
                        var tipo30 = (TelasRN.Tipo30)Tela;
                        baseRN = new DadosRN.Tipo30(tipo30);
                        break;
                    case 40:
                        var tipo40 = (TelasRN.Tipo40_41_50)Tela;
                        baseRN = new DadosRN.Tipo40_41_50(tipo40);
                        break;
                    case 41:
                        tipo40 = (TelasRN.Tipo40_41_50)Tela;
                        baseRN = new DadosRN.Tipo40_41_50(tipo40);
                        break;
                    case 4141:
                        var tipoST = (TelasRN.TipoICMSST)Tela;
                        baseRN = new DadosRN.TipoICMSST(tipoST);
                        break;
                    case 50:
                        tipo40 = (TelasRN.Tipo40_41_50)Tela;
                        baseRN = new DadosRN.Tipo40_41_50(tipo40);
                        break;
                    case 51:
                        var tipo51 = (TelasRN.Tipo51)Tela;
                        baseRN = new DadosRN.Tipo51(tipo51);
                        break;
                    case 60:
                        var tipo60 = (TelasRN.Tipo60)Tela;
                        baseRN = new DadosRN.Tipo60(tipo60);
                        break;
                    case 70:
                        var tipo70 = (TelasRN.Tipo70)Tela;
                        baseRN = new DadosRN.Tipo70(tipo70);
                        break;
                    case 90:
                        var tipo90 = (TelasRN.Tipo90)Tela;
                        baseRN = new DadosRN.Tipo90(tipo90);
                        break;
                    case 9090:
                        tipoPart = (TelasRN.TipoPart)Tela;
                        baseRN = new DadosRN.TipoPart(tipoPart);
                        break;
                    default:
                        throw new System.Exception("CST desconhecido.");
                }
                baseRN.CST = CST.Substring(0, 2);
                baseRN.Origem = origem;
                return (baseRN, null);
            }
        }

        protected override void ProcessarDadosProntos(ImpostoArmazenado imposto)
        {
            if (imposto is ICMSArmazenado icms)
            {
                dados = icms.IsRegimeNormal ? (IDadosICMS)icms.RegimeNormal : icms.SimplesNacional;
            }
        }
    }
}
