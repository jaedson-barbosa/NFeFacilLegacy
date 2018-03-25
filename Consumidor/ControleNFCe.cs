using Consumidor;
using BaseGeral.IBGE;
using BaseGeral.ModeloXML;
using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesTransporte;
using System.Collections.Generic;
using BaseGeral;
using Fiscal;

namespace Consumidor
{
    sealed class ControleNFCe : IControleCriacao
    {
        NFCe PreNota { get; }

        public ControleNFCe()
        {
            PreNota = new NFCe()
            {
                Informacoes = new InformacoesNFCe()
                {
                    Emitente = DefinicoesTemporarias.EmitenteAtivo.ToEmitente(),
                    produtos = new List<DetalhesProdutos>(),
                    transp = new Transporte()
                    {
                        ModFrete = 9,
                        Transporta = new Motorista()
                    },
                    FormasPagamento = new List<Pagamento>(),
                    infAdic = new InformacoesAdicionais(),
                }
            };
        }

        public ControleNFCe(NFCe preNota)
        {
            PreNota = preNota;
        }

        public int ObterMaiorNumero(ushort serie, bool homologacao)
        {
            var cnpj = DefinicoesTemporarias.EmitenteAtivo.CNPJ;
            using (var repo = new BaseGeral.Repositorio.Leitura())
            {
                return repo.ObterMaiorNumeroNFCe(cnpj, serie, homologacao);
            }
        }

        public void Processar(ushort serie, int numero, bool homologacao)
        {
            var identificacao = new Identificacao(false)
            {
                Serie = serie,
                Numero = numero,
                TipoAmbiente = (ushort)(homologacao ? 2 : 1),
                CódigoUF = Estados.Buscar(DefinicoesTemporarias.EmitenteAtivo.SiglaUF).Codigo,
                CodigoMunicipio = DefinicoesTemporarias.EmitenteAtivo.CodigoMunicipio,
                TipoImpressão = 4
            };
            identificacao.DefinirVersãoAplicativo();
            PreNota.Informacoes.identificacao = identificacao;
            PreNota.Informacoes.ChaveAcesso = null;
            BasicMainPage.Current.Navegar<ManipulacaoNFCe>(PreNota);
        }
    }
}
