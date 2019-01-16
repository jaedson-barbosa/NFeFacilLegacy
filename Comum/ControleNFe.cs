using BaseGeral.IBGE;
using BaseGeral.ModeloXML;
using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesTransporte;
using System.Collections.Generic;
using BaseGeral;
using Fiscal;

namespace Comum
{
    public sealed class ControleNFe : IControleCriacao
    {
        NFe PreNota { get; }

        public ControleNFe()
        {
            PreNota = new NFe()
            {
                Informacoes = new InformacoesNFe()
                {
                    Emitente = DefinicoesTemporarias.EmitenteAtivo.ToEmitente(),
                    destinatario = new Destinatario(),
                    produtos = new List<DetalhesProdutos>(),
                    transp = new Transporte()
                    {
                        Transporta = new Motorista(),
                        RetTransp = new ICMSTransporte(),
                        VeicTransp = new Veiculo()
                    },
                    cobr = new Cobranca(),
                    infAdic = new InformacoesAdicionais(),
                    cana = new RegistroAquisicaoCana(),
                    Pagamento = new DetalhamentoPagamento()
                }
            };
        }

        public ControleNFe(NFe preNota)
        {
            PreNota = preNota;
        }

        public int ObterMaiorNumero(ushort serie, bool homologacao)
        {
            var cnpj = DefinicoesTemporarias.EmitenteAtivo.CNPJ;
            using (var repo = new BaseGeral.Repositorio.Leitura())
            {
                return repo.ObterMaiorNumeroNFe(cnpj, serie, homologacao);
            }
        }

        public void Processar(ushort serie, int numero, bool homologacao)
        {
            var identificacao = new Identificacao()
            {
                Serie = serie,
                Numero = numero,
                TipoAmbiente = (ushort)(homologacao ? 2 : 1),
                CódigoUF = Estados.Buscar(DefinicoesTemporarias.EmitenteAtivo.SiglaUF).Codigo,
                CodigoMunicipio = DefinicoesTemporarias.EmitenteAtivo.CodigoMunicipio
            };
            identificacao.DefinirVersãoAplicativo();
            PreNota.Informacoes.identificacao = identificacao;
            PreNota.Informacoes.ChaveAcesso = null;
            var controle = new ControleViewProduto(PreNota);
            BasicMainPage.Current.Navegar<Venda.ViewProdutoVenda.ListaProdutos>(controle);
        }
    }
}
