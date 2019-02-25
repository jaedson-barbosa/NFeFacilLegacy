using BaseGeral;
using BaseGeral.ModeloXML.PartesDetalhes;
using System.Linq;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.Impostos
{
    public sealed class RoteiroAdicaoImpostos
    {
        public UserControl[] Telas { get; }
        IProcessamentoImposto[] Processamentos { get; }
        DetalhesProdutos Produto { get; }

        public RoteiroAdicaoImpostos(IDetalhamentoImposto[] impostos, DetalhesProdutos prod)
        {
            Produto = prod;
            Telas = new UserControl[impostos.Length];
            Processamentos = new IProcessamentoImposto[impostos.Length];
            for (int i = 0; i < impostos.Length; i++)
            {
                var atual = impostos[i];
                if (atual is DetalhamentoCOFINS.Detalhamento cofins)
                {
                    switch (cofins.TipoCalculo)
                    {
                        case TiposCalculo.PorAliquota:
                            var aliq = new DetalhamentoCOFINS.DetalharAliquota(cofins);
                            Telas[i] = aliq;
                            Processamentos[i] = aliq;
                            break;
                        case TiposCalculo.PorValor:
                            var qtde = new DetalhamentoCOFINS.DetalharQtde(cofins);
                            Telas[i] = qtde;
                            Processamentos[i] = qtde;
                            break;
                        default:
                            Telas[i] = null;
                            Processamentos[i] = new DetalhamentoCOFINS.DetalharVazio(cofins.CST);
                            break;
                    }
                }
                else if (atual is DetalhamentoPIS.Detalhamento pis)
                {
                    switch (pis.TipoCalculo)
                    {
                        case TiposCalculo.PorAliquota:
                            var aliq = new DetalhamentoPIS.DetalharAliquota(pis);
                            Telas[i] = aliq;
                            Processamentos[i] = aliq;
                            break;
                        case TiposCalculo.PorValor:
                            var qtde = new DetalhamentoPIS.DetalharQtde(pis);
                            Telas[i] = qtde;
                            Processamentos[i] = qtde;
                            break;
                        default:
                            Telas[i] = null;
                            Processamentos[i] = new DetalhamentoPIS.DetalharVazio(pis.CST);
                            break;
                    }
                }
                else if (atual is DetalhamentoIPI.Detalhamento ipi)
                {
                    switch (ipi.TipoCalculo)
                    {
                        case TiposCalculo.PorAliquota:
                            var aliq = new DetalhamentoIPI.DetalharAliquota(ipi);
                            Telas[i] = aliq;
                            Processamentos[i] = aliq;
                            break;
                        case TiposCalculo.PorValor:
                            var qtde = new DetalhamentoIPI.DetalharQtde(ipi);
                            Telas[i] = qtde;
                            Processamentos[i] = qtde;
                            break;
                        case TiposCalculo.Inexistente:
                            var simp = new DetalhamentoIPI.DetalharSimples(ipi);
                            Telas[i] = simp;
                            Processamentos[i] = simp;
                            break;
                    }
                }
                else if (atual is DetalhamentoICMSUFDest.Detalhamento icmsUFDest)
                {
                    var icms = new DetalhamentoICMSUFDest.Detalhar();
                    Telas[i] = icms;
                    Processamentos[i] = icms;
                }
                else if (atual is DetalhamentoICMS.Detalhamento icms)
                {
                    var normal = DefinicoesTemporarias.EmitenteAtivo.RegimeTributario == 3;
                    if (normal)
                    {
                        var cst = int.Parse(icms.TipoICMSRN);
                        var norm = AssociacoesICMS.GetRegimeNormal(cst, icms);
                        Telas[i] = norm;
                        Processamentos[i] = (IProcessamentoImposto)norm;
                    }
                    else
                    {
                        var csosn = int.Parse(icms.TipoICMSSN);
                        var simp = AssociacoesICMS.GetSimplesNacional(csosn, icms);
                        Telas[i] = simp;
                        Processamentos[i] = (IProcessamentoImposto)simp;
                    }
                }
                else if (atual is ImpostoArmazenado pronto)
                {
                    switch (pronto.Tipo)
                    {
                        case PrincipaisImpostos.ICMS:
                            Processamentos[i] = new DetalhamentoICMS.DetalharArmazenado((ICMSArmazenado)pronto);
                            break;
                        case PrincipaisImpostos.IPI:
                            Processamentos[i] = new DetalhamentoIPI.DetalharArmazenado((ImpSimplesArmazenado)pronto);
                            break;
                        case PrincipaisImpostos.PIS:
                            Processamentos[i] = new DetalhamentoPIS.DetalharArmazenado((ImpSimplesArmazenado)pronto);
                            break;
                        case PrincipaisImpostos.COFINS:
                            Processamentos[i] = new DetalhamentoCOFINS.DetalharArmazenado((ImpSimplesArmazenado)pronto);
                            break;
                    }
                }
            }
        }

        public DetalhesProdutos Finalizar()
        {
            var impostos = Produto.Impostos.impostos;
            impostos.Clear();
            foreach (var item in Processamentos.OrderBy(x => (int)x.Tipo))
                impostos.AddRange(item.Processar(Produto));
            return Produto;
        }
    }
}
