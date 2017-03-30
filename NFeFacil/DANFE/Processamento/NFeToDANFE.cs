using NFeFacil.DANFE.Modelos.Global;
using NFeFacil.DANFE.Modelos.Local;
using NFeFacil.ModeloXML;
using NFeFacil.ModeloXML.PartesProcesso;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using System;
using System.Linq;

namespace NFeFacil.DANFE.Processamento
{
    public static class NFeToDANFE
    {
        public static Geral Converter(Processo proc)
        {
            return new Geral
            {
                _DadosAdicionais = GetExtras(proc.NFe.Informações.infAdic),
                _DadosCabecalho = GetCabecalho(proc.NFe.Informações.identificação, proc.NFe.Informações.emitente),
                _DadosCliente = GetCliente(proc.NFe.Informações.identificação, proc.NFe.Informações.destinatário),
                _DadosImposto = GetImposto(proc.NFe.Informações.total),
                _DadosMotorista = GetMotorista(proc.NFe.Informações.transp),
                _DadosNFe = GetNFe(proc.NFe.Informações, proc.ProtNFe),
                _DadosProdutos = (from p in proc.NFe.Informações.produtos
                                  select GetProd(p)).ToArray(),
                _Duplicatas = (from d in proc.NFe.Informações.cobr?.Dup
                               select GetDuplicata(d)).ToArray()
            };
        }

        private static DadosAdicionais GetExtras(InformacoesAdicionais extras)
        {
            return new DadosAdicionais
            {
                dados = (extras?.infCpl).Analisar(),
                fisco = (extras?.infAdFisco).Analisar()
            };
        }

        private static DadosCabecalho GetCabecalho(Identificacao ident, Emitente emit)
        {
            return new DadosCabecalho
            {
                nomeEmitente = emit.nome,
                serieNota = ident.Serie.ToString(),
                numeroNota = ident.Numero.ToString()
            };
        }

        private static DadosCliente GetCliente(Identificacao ident, Destinatario dest)
        {
            return new DadosCliente
            {
                DocCliente = dest.obterDocumento,
                dataEmissao = Convert.ToDateTime(ident.DataHoraEmissão).ToString("dd-MM-yyyy"),
                dataEntradaSaida = Convert.ToDateTime(ident.DataHoraSaídaEntrada).ToString("dd-MM-yyyy").Analisar(),
                horaEntradaSaida = Convert.ToDateTime(ident.DataHoraSaídaEntrada).ToString("hh:mm:ss").Analisar(),
                Endereco = dest.endereco,
                IECliente = dest.inscricaoEstadual.Analisar(),
                nomeCliente = dest.nome
            };
        }

        private static DadosImposto GetImposto(Total tot)
        {
            return new DadosImposto
            {
                baseCalculoICMS = tot.ICMSTot.vBC.ToString(),
                baseCalculoICMSST = tot.ICMSTot.vBCST.ToString(),
                desconto = tot.ICMSTot.vDesc.ToString(),
                despesasAcessorias = tot.ICMSTot.vOutro.ToString(),
                totalNota = tot.ICMSTot.vNF.ToString(),
                valorFrete = tot.ICMSTot.vFrete.ToString(),
                valorICMS = tot.ICMSTot.vICMS.ToString(),
                valorICMSST = tot.ICMSTot.vST.ToString(),
                valorIPI = tot.ICMSTot.vIPI.ToString(),
                valorSeguro = tot.ICMSTot.vSeg.ToString(),
                valorTotalProdutos = tot.ICMSTot.vProd.ToString()
            };
        }

        private static DadosMotorista GetMotorista(Transporte transp)
        {
            return new DadosMotorista
            {
                codigoANTT = (transp.veicTransp?.RNTC).Analisar(),
                documentoMotorista = (transp.transporta?.Documento).Analisar(),
                enderecoMotorista = (transp.transporta?.XEnder).Analisar(),
                especieVolume = (transp.vol.FirstOrDefault()?.esp).Analisar(),
                IEMotorista = (transp.transporta?.InscricaoEstadual).Analisar(),
                marcaVolume = (transp.vol.FirstOrDefault()?.marca).Analisar(),
                modalidadeFrete = ((int)transp.modFrete).ToString(),
                municipioMotorista = (transp.transporta?.XMun).Analisar(),
                nomeMotorista = (transp.transporta?.Nome).Analisar(),
                numeroVolume = (transp.vol.FirstOrDefault()?.nVol).Analisar(),
                pesoBrutoVolume = (transp.vol.FirstOrDefault()?.pesoB).Analisar(),
                pesoLiquidoVolume = (transp.vol.FirstOrDefault()?.pesoL).Analisar(),
                placa = (transp.veicTransp?.Placa).Analisar(),
                quantidadeVolume = (transp.vol.FirstOrDefault()?.qVol).Analisar(),
                ufMotorista = (transp.transporta?.UF).Analisar(),
                ufPlaca = (transp.veicTransp?.UF).Analisar()
            };
        }

        private static DadosNFe GetNFe(Detalhes detalhes, ProtocoloNFe prot)
        {
            var codigoBarras = detalhes.Id.Substring(detalhes.Id.IndexOf('e') + 1);
            return new DadosNFe
            {
                Chave = codigoBarras,
                CNPJEmit = detalhes.emitente.CNPJ,
                DataHoraRecibo = prot.InfProt.dhRecbto.Replace('T', ' '),
                Endereco = detalhes.emitente.endereco,
                IE = detalhes.emitente.inscricaoEstadual,
                NatOp = detalhes.identificação.NaturezaDaOperação,
                NomeEmitente = detalhes.emitente.nome,
                NumeroNota = detalhes.identificação.Numero.ToString(),
                NumeroProtocolo = prot.InfProt.nProt.ToString(),
                SerieNota = detalhes.identificação.Serie.ToString(),
                TipoEmissao = detalhes.identificação.TipoEmissão.ToString(),
            };
        }

        private static DadosProduto GetProd(DetalhesProdutos prod)
        {
            var consult = new ConsultarImpostos(prod.impostos.ToXElement<Impostos>());
            return new DadosProduto
            {
                CFOP = prod.Produto.CFOP,
                cProd = prod.Produto.CodigoProduto,
                CSTICMS = prod.impostos.GetCSTICMS(),
                NCM = prod.Produto.NCM,
                qCom = prod.Produto.QuantidadeComercializada.ToString(),
                uCom = prod.Produto.UnidadeComercializacao,
                vUnCom = prod.Produto.ValorUnitario.ToString(),
                vUnTrib = prod.Produto.ValorUnitarioTributo.ToString(),
                vProd = prod.Produto.ValorTotal.ToString(),
                xProd = prod.Produto.Descricao,
                pICMS = consult.AgregarValor("pICMS", 0).ToString(),
                vICMS = consult.AgregarValor("vICMS", 0).ToString(),
                pIPI = consult.AgregarValor("pIPI", 0).ToString(),
                vIPI = consult.AgregarValor("vIPI", 0).ToString()
            };
        }

        private static DadosDuplicata GetDuplicata(Duplicata dup)
        {
            return new DadosDuplicata
            {
                data = dup.DVenc,
                n = dup.NDup,
                valor = dup.DDup
            };
        }

        private static string Analisar(this object str) => str != null ? (string)str : string.Empty;
    }
}
