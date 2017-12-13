using NFeFacil.Log;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoICMS
{
    public sealed class Processamento : ProcessamentoImposto
    {
        IDadosICMS dados;

        public override Imposto[] Processar(DetalhesProdutos prod)
        {
            var imposto = new ICMS { Corpo = (ComumICMS)dados.Processar(prod) };
            return new Imposto[1] { imposto };
        }

        public override bool ValidarDados(ILog log) => true;

        public override bool ValidarEntradaDados(object Tela)
        {
            if (Detalhamento is Detalhamento detalhamento)
            {
                var normal = Propriedades.EmitenteAtivo.RegimeTributario == 3;
                if (!normal)
                {
                    var csosn = int.Parse(detalhamento.TipoICMSSN);
                    DadosSN.BaseSN baseSN;
                    if (Tela?.GetType() == AssociacoesSimples.ICMSSN[csosn])
                    {
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
                        baseSN.CSOSN = detalhamento.TipoICMSSN;
                        baseSN.Origem = detalhamento.Origem;
                        dados = baseSN;
                        return true;
                    }
                }
                else
                {
                    var cst = int.Parse(detalhamento.TipoICMSRN);
                    DadosRN.BaseRN baseRN;
                    if(Tela.GetType() == AssociacoesSimples.ICMSRN[cst])
                    {
                        switch (int.Parse(detalhamento.TipoICMSRN))
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
                        baseRN.CST = detalhamento.TipoICMSRN.Substring(0, 2);
                        baseRN.Origem = detalhamento.Origem;
                        dados = baseRN;
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
