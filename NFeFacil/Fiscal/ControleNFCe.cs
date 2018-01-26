using NFeFacil.Fiscal.ViewNFCe;
using NFeFacil.IBGE;
using NFeFacil.ModeloXML;
using NFeFacil.ModeloXML.PartesDetalhes;
using NFeFacil.ModeloXML.PartesDetalhes.PartesTransporte;
using System.Collections.Generic;

namespace NFeFacil.Fiscal
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
                    destinatário = new Destinatario(),
                    produtos = new List<DetalhesProdutos>(),
                    transp = new Transporte()
                    {
                        Transporta = new Motorista()
                    },
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
            using (var repo = new Repositorio.Leitura())
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
                CodigoMunicipio = DefinicoesTemporarias.EmitenteAtivo.CodigoMunicipio
            };
            identificacao.DefinirVersãoAplicativo();
            PreNota.Informacoes.identificacao = identificacao;
            PreNota.Informacoes.ChaveAcesso = null;
            MainPage.Current.Navegar<ManipulacaoNFCe>(PreNota);
        }
    }
}
