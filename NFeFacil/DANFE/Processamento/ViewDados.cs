﻿using BibliotecaCentral;
using BibliotecaCentral.ModeloXML;
using BibliotecaCentral.ModeloXML.PartesProcesso;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTotal;
using NFeFacil.DANFE.Pacotes;
using System;
using System.Linq;

namespace NFeFacil.DANFE.Processamento
{
    internal static class ViewDados
    {
        internal static Geral Converter(Processo proc)
        {
            var dadosAdicionais = GetExtras(proc.NFe.Informações.infAdic);
            var dadosCabecalho = GetCabecalho(proc.NFe.Informações.identificação, proc.NFe.Informações.emitente);
            var dadosCliente = GetCliente(proc.NFe.Informações.identificação, proc.NFe.Informações.destinatário);
            var dadosImposto = GetImposto(proc.NFe.Informações.total);
            var dadosMotorista = GetMotorista(proc.NFe.Informações.transp);
            var dadosNFe = GetNFe(proc.NFe.Informações, proc.ProtNFe);
            var dadosProdutos = (from p in proc.NFe.Informações.produtos
                                 select GetProd(p)).ToArray();
            var duplicatas = proc.NFe.Informações.cobr?.Dup;
            var dadosDuplicatas = duplicatas != null ? (from d in duplicatas
                                                        select GetDuplicata(d)).ToArray() : new DadosDuplicata[0];
            var dadosISSQN = GetISSQN(proc.NFe.Informações.emitente, proc.NFe.Informações.total.ISSQNtot);
            return new Geral
            {
                _DadosAdicionais = dadosAdicionais,
                _DadosCabecalho = dadosCabecalho,
                _DadosCliente = dadosCliente,
                _DadosImposto = dadosImposto,
                _DadosMotorista = dadosMotorista,
                _DadosNFe = dadosNFe,
                _DadosProdutos = dadosProdutos,
                _Duplicatas = dadosDuplicatas
            };

            DadosISSQN GetISSQN(Emitente emit, ISSQNtot issqn)
            {
                return issqn != null ? new DadosISSQN
                {
                    BC = issqn.vBC.ToString("N2"),
                    IM = emit.IM,
                    TotalServiços = issqn.vServ.ToString("N2"),
                    ValorISSQN = issqn.vISS.ToString("N2")
                } : new DadosISSQN();
            }

            DadosAdicionais GetExtras(InformacoesAdicionais extras)
            {
                return new DadosAdicionais
                {
                    Dados = extras?.infCpl,
                    Fisco = extras?.infAdFisco,
                };
            }

            DadosCabecalho GetCabecalho(Identificacao ident, Emitente emit)
            {
                return new DadosCabecalho
                {
                    NomeEmitente = emit.nome,
                    SerieNota = ident.Serie.ToString(),
                    NumeroNota = ident.Numero.ToString("000,000,000")
                };
            }

            DadosCliente GetCliente(Identificacao ident, Destinatario dest)
            {
                return new DadosCliente
                {
                    DocCliente = AplicatMascaraDocumento(dest.Documento),
                    DataEmissao = Convert.ToDateTime(ident.DataHoraEmissão).ToString("dd-MM-yyyy"),
                    DataEntradaSaida = !string.IsNullOrEmpty(ident.DataHoraSaídaEntrada) ? Analisar(Convert.ToDateTime(ident.DataHoraSaídaEntrada).ToString("dd-MM-yyyy")) : string.Empty,
                    HoraEntradaSaida = !string.IsNullOrEmpty(ident.DataHoraSaídaEntrada) ? Analisar(Convert.ToDateTime(ident.DataHoraSaídaEntrada).ToString("hh:mm:ss")) : string.Empty,
                    Endereco = dest.endereco,
                    IECliente = Analisar(dest.inscricaoEstadual),
                    NomeCliente = dest.nome
                };
            }

            DadosImposto GetImposto(Total tot)
            {
                return new DadosImposto
                {
                    BaseCalculoICMS = tot.ICMSTot.vBC.ToString("N2"),
                    BaseCalculoICMSST = tot.ICMSTot.vBCST.ToString("N2"),
                    Desconto = tot.ICMSTot.vDesc.ToString("N2"),
                    DespesasAcessorias = tot.ICMSTot.vOutro.ToString("N2"),
                    TotalNota = tot.ICMSTot.vNF.ToString("N2"),
                    ValorFrete = tot.ICMSTot.vFrete.ToString("N2"),
                    ValorICMS = tot.ICMSTot.vICMS.ToString("N2"),
                    ValorICMSST = tot.ICMSTot.vST.ToString("N2"),
                    ValorIPI = tot.ICMSTot.vIPI.ToString("N2"),
                    ValorSeguro = tot.ICMSTot.vSeg.ToString("N2"),
                    ValorTotalProdutos = tot.ICMSTot.vProd.ToString("N2")
                };
            }

            DadosMotorista GetMotorista(Transporte transp)
            {
                return new DadosMotorista
                {
                    CodigoANTT = Analisar(transp.veicTransp?.RNTC),
                    DocumentoMotorista = transp.transporta?.Documento != null ? AplicatMascaraDocumento(transp.transporta?.Documento) : null,
                    EnderecoMotorista = Analisar(transp.transporta?.XEnder),
                    EspecieVolume = Analisar(transp.vol.FirstOrDefault()?.esp),
                    IEMotorista = Analisar(transp.transporta?.InscricaoEstadual),
                    MarcaVolume = Analisar(transp.vol.FirstOrDefault()?.marca),
                    ModalidadeFrete = ObterModalidadeCompleta(transp.modFrete),
                    MunicipioMotorista = Analisar(transp.transporta?.XMun),
                    NomeMotorista = Analisar(transp.transporta?.Nome),
                    NumeroVolume = Analisar(transp.vol.FirstOrDefault()?.nVol),
                    PesoBrutoVolume = transp.vol.FirstOrDefault()?.pesoB.ToString("N3"),
                    PesoLiquidoVolume = transp.vol.FirstOrDefault()?.pesoL.ToString("N3"),
                    Placa = Analisar(transp.veicTransp?.Placa),
                    QuantidadeVolume = Analisar(transp.vol.FirstOrDefault()?.qVol),
                    UfMotorista = Analisar(transp.transporta?.UF),
                    UfPlaca = Analisar(transp.veicTransp?.UF)
                };

                string ObterModalidadeCompleta(int modalidade)
                {
                    switch (modalidade)
                    {
                        case 0:
                            return "0 – Emitente";
                        case 1:
                            return "1 – Dest/Rem";
                        case 2:
                            return "2 – Terceiros";
                        case 9:
                            return "9 – Sem Frete";
                        default:
                            return "Erro";
                    }
                }
            }

            DadosNFe GetNFe(Detalhes detalhes, ProtocoloNFe prot)
            {
                var codigoBarras = detalhes.Id.Substring(detalhes.Id.IndexOf('e') + 1);
                return new DadosNFe
                {
                    Chave = codigoBarras,
                    ChaveComMascara = AplicarMascaraChave(codigoBarras),
                    CNPJEmit = AplicatMascaraDocumento(detalhes.emitente.CNPJ),
                    DataHoraRecibo = prot.InfProt.dhRecbto.Replace('T', ' '),
                    Endereco = detalhes.emitente.endereco,
                    IE = detalhes.emitente.inscricaoEstadual,
                    IEST = detalhes.emitente.IEST,
                    NatOp = detalhes.identificação.NaturezaDaOperação,
                    NomeEmitente = detalhes.emitente.nome,
                    NumeroNota = detalhes.identificação.Numero.ToString("000,000,000"),
                    NumeroProtocolo = prot.InfProt.nProt.ToString(),
                    SerieNota = detalhes.identificação.Serie.ToString(),
                    TipoEmissao = detalhes.identificação.TipoEmissão.ToString(),
                };

                string AplicarMascaraChave(string original)
                {
                    var novaChave = "";
                    for (var i = 0; i < 44; i += 4)
                    {
                        novaChave += original.Substring(i, 4) + " ";
                    }
                    return novaChave;
                }
            }

            DadosProduto GetProd(DetalhesProdutos prod)
            {
                var consult = new ConsultarImpostos(prod.impostos.ToXElement<Impostos>());
                return new DadosProduto
                {
                    CFOP = prod.Produto.CFOP,
                    CProd = prod.Produto.CodigoProduto,
                    CSTICMS = prod.impostos.GetCSTICMS(),
                    NCM = prod.Produto.NCM,
                    QCom = prod.Produto.QuantidadeComercializada.ToString("N4"),
                    UCom = prod.Produto.UnidadeComercializacao,
                    VUnCom = prod.Produto.ValorUnitario.ToString("N4"),
                    BCICMS = consult.AgregarValor("vBC", 0).ToString("N2"),
                    VProd = prod.Produto.ValorTotal.ToString("N2"),
                    XProd = prod.Produto.Descricao,
                    PICMS = consult.AgregarValor("pICMS", 0).ToString("N2"),
                    VICMS = consult.AgregarValor("vICMS", 0).ToString("N2"),
                    PIPI = consult.AgregarValor("pIPI", 0).ToString("N2"),
                    VIPI = consult.AgregarValor("vIPI", 0).ToString("N2"),
                    InfoAdicional = prod.infAdProd
                };
            }

            DadosDuplicata GetDuplicata(Duplicata dup)
            {
                return new DadosDuplicata
                {
                    data = dup.DVenc,
                    n = dup.NDup,
                    valor = dup.DDup
                };
            }

            string Analisar(object str) => str != null ? (string)str : string.Empty;

            string AplicatMascaraDocumento(string original)
            {
                original = original.Trim();
                if(original.Length == 14)
                {
                    // É CNPJ
                    return $"{original.Substring(0, 2)}.{original.Substring(2,3)}.{original.Substring(5, 3)}/{original.Substring(8, 4)}.{original.Substring(12, 2)}";
                }
                else if (original.Length == 11)
                {
                    // É CPF
                    return $"{original.Substring(0, 3)}.{original.Substring(3, 3)}.{original.Substring(6, 3)}-{original.Substring(9, 2)}";
                }
                else
                {
                    // Não é nem CNPJ nem CPF
                    return original;
                }
            }
        }
    }
}
