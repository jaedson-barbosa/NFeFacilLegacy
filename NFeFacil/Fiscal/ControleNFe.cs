using NFeFacil.Fiscal.ViewNFe;
using NFeFacil.IBGE;
using NFeFacil.ModeloXML;
using NFeFacil.ModeloXML.PartesDetalhes;
using NFeFacil.ModeloXML.PartesDetalhes.PartesTransporte;
using System.Collections.Generic;

namespace NFeFacil.Fiscal
{
    sealed class ControleNFe : IControleCriacao
    {
        NFe PreNota { get; }

        public ControleNFe()
        {
            PreNota = new NFe()
            {
                Informacoes = new InformacoesNFe()
                {
                    Emitente = DefinicoesTemporarias.EmitenteAtivo.ToEmitente(),
                    destinatário = new Destinatario(),
                    produtos = new List<DetalhesProdutos>(),
                    transp = new Transporte()
                    {
                        Transporta = new Motorista(),
                        RetTransp = new ICMSTransporte(),
                        VeicTransp = new Veiculo()
                    },
                    cobr = new Cobranca(),
                    infAdic = new InformacoesAdicionais(),
                    exporta = new Exportacao(),
                    compra = new Compra(),
                    cana = new RegistroAquisicaoCana()
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
            using (var repo = new Repositorio.Leitura())
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
            MainPage.Current.Navegar<ManipulacaoNotaFiscal>(PreNota);
        }
    }
}
