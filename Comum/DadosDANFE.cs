using Comum.PacotesDANFE;
using BaseGeral.ModeloXML;
using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;
using System;
using System.Collections.Generic;
using System.Linq;
using static BaseGeral.ExtensoesPrincipal;
using BaseGeral;

namespace Comum
{
    public struct DadosDANFE
    {
        ProcessoNFe Dados { get; }

        public DadosDANFE(ProcessoNFe processo)
        {
            Dados = processo;
        }

        public Geral ObterDadosConvertidos()
        {
            var dadosAdicionais = GetExtras();
            var dadosCabecalho = GetCabecalho();
            var dadosCliente = GetCliente();
            var dadosImposto = GetImposto();
            var dadosMotorista = GetMotorista();
            var dadosNFe = GetNFe();
            var dadosProdutos = ObterProdutos();
            var dadosDuplicatas = ObterDuplicatas();
            var dadosISSQN = GetISSQN();
            var fatura = GetFatura();

            return new Geral
            {
                _DadosAdicionais = dadosAdicionais,
                _DadosCabecalho = dadosCabecalho,
                _DadosCliente = dadosCliente,
                _DadosImposto = dadosImposto,
                _DadosMotorista = dadosMotorista,
                _DadosNFe = dadosNFe,
                _DadosProdutos = dadosProdutos,
                _Duplicatas = dadosDuplicatas,
                _DadosISSQN = dadosISSQN,
                Fatura = fatura
            };
        }

        DadosAdicionais GetExtras()
        {
            var extras = Dados.NFe.Informacoes.infAdic;
            var cobr = Dados.NFe.Informacoes.cobr;
            var entrega = Dados.NFe.Informacoes.Entrega;
            var retirada = Dados.NFe.Informacoes.Retirada;
            var total = Dados.NFe.Informacoes.total.ICMSTot;

            var itens = new List<ItemDadosAdicionais>();
            if (Dados.NFe.AmbienteTestes)
            {
                itens.Add(new ItemDadosAdicionais("SEM VALOR FISCAL"));
            }
            if (retirada != null)
            {
                itens.Add(new ItemDadosAdicionais("ENDEREÇO DE RETIRADA:", $"{retirada.Logradouro}, {retirada.Numero}", retirada.Bairro, $"{retirada.NomeMunicipio} - {retirada.SiglaUF}", retirada.CNPJ != null ? $"CNPJ: {AplicarMáscaraDocumento(retirada.CNPJ)}" : $"CPF: {AplicarMáscaraDocumento(retirada.CPF)}"));
            }
            if (entrega != null)
            {
                itens.Add(new ItemDadosAdicionais("ENDEREÇO DE ENTREGA:", $"{entrega.Logradouro}, {entrega.Numero}", entrega.Bairro, $"{entrega.NomeMunicipio} - {entrega.SiglaUF}", entrega.CNPJ != null ? $"CNPJ: {AplicarMáscaraDocumento(entrega.CNPJ)}" : $"CPF: {AplicarMáscaraDocumento(entrega.CPF)}"));
            }
            if (cobr?.Dup != null)
            {
                itens.Add(new ItemDadosAdicionais("DUPLICATAS:", cobr.Dup.Select(dup => $"Duplicata - Num.: {dup.NDup}, Vec.: {dup.DVenc}, Valor: {dup.VDup.ToString("N2")}")));
            }
            if (extras?.InfCpl != null)
            {
                itens.Add(new ItemDadosAdicionais("DE INTERESSE DO CONTRIBUINTE:", extras.InfCpl));
            }
            double vFCP = total.vFCP, vFCPST = total.vFCPST;
            if (extras?.InfAdFisco != null)
            {
                if (vFCP != 0 && vFCPST != 0)
                    itens.Add(new ItemDadosAdicionais(
                        "DE INTERESSE DO FISCO:",
                        $"{extras.InfAdFisco}\nValor FCP: R$ {total.vFCP}\nValor FCPS: R$ {total.vFCPST}"));
                else if (vFCP != 0)
                    itens.Add(new ItemDadosAdicionais(
                        "DE INTERESSE DO FISCO:",
                        $"{extras.InfAdFisco}\nValor FCP: R$ {total.vFCP}"));
                else if (vFCPST != 0)
                    itens.Add(new ItemDadosAdicionais(
                        "DE INTERESSE DO FISCO:",
                        $"{extras.InfAdFisco}\nValor FCPS: R$ {total.vFCPST}"));
            }
            else
            {
                if (vFCP != 0 && vFCPST != 0)
                    itens.Add(new ItemDadosAdicionais("DE INTERESSE DO FISCO:", $"Valor FCP: R$ {total.vFCP}\nValor FCPS: R$ {total.vFCPST}"));
                else if (vFCP != 0)
                    itens.Add(new ItemDadosAdicionais("DE INTERESSE DO FISCO:", $"Valor FCP: R$ {total.vFCP}"));
                else if (vFCPST != 0)
                    itens.Add(new ItemDadosAdicionais("DE INTERESSE DO FISCO:", $"Valor FCPS: R$ {total.vFCPST}"));
            }
            if (extras?.ProcRef?.Count > 0)
            {
                var proc = new ItemDadosAdicionais("PROCESSOS REFERENCIADOS:", extras.ProcRef.Select(x => x.ToString()));
                itens.Add(proc);
            }
            if (total.vICMSDeson != 0)
            {
                var proc = new ItemDadosAdicionais("ICMS DESONERADO:", total.vICMSDeson.ToString("C"));
                itens.Add(proc);
            }
            return new DadosAdicionais(itens);
        }

        DadosCabecalho GetCabecalho()
        {
            var ident = Dados.NFe.Informacoes.identificacao;
            var emit = Dados.NFe.Informacoes.Emitente;
            return new DadosCabecalho
            {
                NomeEmitente = emit.Nome,
                SerieNota = ident.Serie.ToString(),
                NumeroNota = ident.Numero.ToString("000,000,000")
            };
        }

        DadosCliente GetCliente()
        {
            var ident = Dados.NFe.Informacoes.identificacao;
            var dest = Dados.NFe.Informacoes.destinatario;
            return new DadosCliente
            {
                DocCliente = AplicarMáscaraDocumento(dest.Documento),
                DataEmissao = Convert.ToDateTime(ident.DataHoraEmissão).ToString("dd-MM-yyyy"),
                DataEntradaSaida = !string.IsNullOrEmpty(ident.DataHoraSaídaEntrada) ? Analisar(Convert.ToDateTime(ident.DataHoraSaídaEntrada).ToString("dd-MM-yyyy")) : string.Empty,
                HoraEntradaSaida = !string.IsNullOrEmpty(ident.DataHoraSaídaEntrada) ? Analisar(Convert.ToDateTime(ident.DataHoraSaídaEntrada).ToString("HH:mm:ss")) : string.Empty,
                Endereco = dest.Endereco,
                IECliente = Analisar(dest.InscricaoEstadual),
                NomeCliente = dest.Nome
            };
        }

        DadosImposto GetImposto()
        {
            var tot = Dados.NFe.Informacoes.total;
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

        DadosMotorista GetMotorista()
        {
            var transp = Dados.NFe.Informacoes.transp;
            var retorno = new DadosMotorista
            {
                CodigoANTT = Analisar(transp.VeicTransp?.RNTC),
                DocumentoMotorista = transp.Transporta?.Documento != null ? AplicarMáscaraDocumento(transp.Transporta?.Documento) : null,
                EnderecoMotorista = Analisar(transp.Transporta?.XEnder),
                EspecieVolume = Analisar(transp.Vol.FirstOrDefault()?.Esp),
                IEMotorista = Analisar(transp.Transporta?.InscricaoEstadual),
                MunicipioMotorista = Analisar(transp.Transporta?.XMun),
                NomeMotorista = Analisar(transp.Transporta?.Nome),
                Placa = Analisar(transp.VeicTransp?.Placa),
                UfMotorista = Analisar(transp.Transporta?.UF),
                UfPlaca = Analisar(transp.VeicTransp?.UF)
            };

            if (transp.Vol.Count == 0)
            {
                retorno.EspecieVolume = string.Empty;
                retorno.MarcaVolume = string.Empty;
                retorno.NumeroVolume = string.Empty;
                retorno.PesoBrutoVolume = string.Empty;
                retorno.PesoLiquidoVolume = string.Empty;
                retorno.QuantidadeVolume = string.Empty;
            }
            else if (transp.Vol.Count == 1)
            {
                retorno.EspecieVolume = transp.Vol[0].Esp;
                retorno.MarcaVolume = transp.Vol[0].Marca;
                retorno.NumeroVolume = transp.Vol[0].NVol;
                retorno.PesoBrutoVolume = transp.Vol[0].PesoB.ToString("N3");
                retorno.PesoLiquidoVolume = transp.Vol[0].PesoL.ToString("N3");
                retorno.QuantidadeVolume = transp.Vol[0].QVol != null ? long.Parse(transp.Vol[0].QVol).ToString("N3") : string.Empty;
            }
            else
            {
                retorno.EspecieVolume = string.Empty;
                retorno.MarcaVolume = string.Empty;
                retorno.NumeroVolume = string.Empty;
                retorno.PesoBrutoVolume = transp.Vol.Sum(x => x.PesoB).ToString("N3");
                retorno.PesoLiquidoVolume = transp.Vol.Sum(x => x.PesoL).ToString("N3");
                retorno.QuantidadeVolume = transp.Vol.Sum(x => x.QVol != null ? long.Parse(x.QVol) : 0).ToString("N3");
            }

            switch (transp.ModFrete)
            {
                case 0:
                    retorno.ModalidadeFrete = "0 – Rementente";
                    break;
                case 1:
                    retorno.ModalidadeFrete = "1 – Destinatário";
                    break;
                case 2:
                    retorno.ModalidadeFrete = "2 – Terceiros";
                    break;
                case 3:
                    retorno.ModalidadeFrete = "3 – Remetente";
                    break;
                case 4:
                    retorno.ModalidadeFrete = "4 - Destinatário";
                    break;
                case 9:
                    retorno.ModalidadeFrete = "9 – Sem Frete";
                    break;
                default:
                    retorno.ModalidadeFrete = "Erro";
                    break;
            }

            return retorno;
        }

        DadosNFe GetNFe()
        {
            var detalhes = Dados.NFe.Informacoes;
            var prot = Dados.ProtNFe;
            var codigoBarras = detalhes.Id.Substring(detalhes.Id.IndexOf('e') + 1);
            return new DadosNFe
            {
                Chave = codigoBarras,
                ChaveComMascara = AplicarMascaraChave(codigoBarras),
                CNPJEmit = AplicarMáscaraDocumento(detalhes.Emitente.CNPJ),
                DataHoraRecibo = prot.InfProt.dhRecbto.Replace('T', ' '),
                Endereco = detalhes.Emitente.Endereco,
                IE = detalhes.Emitente.InscricaoEstadual.ToString(),
                IEST = detalhes.Emitente.IEST,
                NatOp = detalhes.identificacao.NaturezaDaOperacao,
                NomeEmitente = detalhes.Emitente.Nome,
                NumeroNota = detalhes.identificacao.Numero.ToString("000,000,000"),
                NumeroProtocolo = prot.InfProt.nProt.ToString(),
                SerieNota = detalhes.identificacao.Serie.ToString(),
                TipoEmissao = detalhes.identificacao.TipoEmissão.ToString(),
                Logotipo = DefinicoesTemporarias.Logotipo
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

        DadosProduto[] ObterProdutos()
        {
            return (from p in Dados.NFe.Informacoes.produtos
                    select GetProd(p)).ToArray();

            DadosProduto GetProd(DetalhesProdutos prod)
            {
                var corpo = prod.Impostos;
                var tipo = corpo.GetType();
                return new DadosProduto
                {
                    CFOP = prod.Produto.CFOP.ToString(),
                    CProd = prod.Produto.CodigoProduto,
                    CSTICMS = GetCSTICMS(),
                    NCM = prod.Produto.NCM,
                    QCom = prod.Produto.QuantidadeComercializada.ToString("N4"),
                    UCom = prod.Produto.UnidadeComercializacao,
                    VUnCom = prod.Produto.ValorUnitario.ToString("N4"),
                    BCICMS = AgregarValor("vBC").ToString("N2"),
                    VProd = prod.Produto.ValorTotal.ToString("N2"),
                    XProd = prod.Produto.Descricao,
                    PICMS = AgregarValor("pICMS").ToString("N2"),
                    VICMS = AgregarValor("vICMS").ToString("N2"),
                    PIPI = AgregarValor("pIPI").ToString("N2"),
                    VIPI = AgregarValor("vIPI").ToString("N2"),
                    InfoAdicional = prod.InfAdProd
                };

                double AgregarValor(string nomeElemento)
                {
                    var valor = tipo.GetProperty(nomeElemento)?.GetValue(corpo) ?? 0d;
                    return (double)valor;
                }

                string GetCSTICMS()
                {
                    foreach (var item in prod.Impostos.impostos)
                    {
                        if (item is ICMS icms)
                        {
                            if (icms.Corpo is IRegimeNormal normal) return normal.CST;
                            else if (icms.Corpo is ISimplesNacional simples) return simples.CSOSN;
                        }
                    }
                    return null;
                }
            }
        }

        DadosDuplicata[] ObterDuplicatas()
        {
            var duplicatas = Dados.NFe.Informacoes.cobr?.Dup;
            return duplicatas != null ? (from d in duplicatas
                                         select GetDuplicata(d)).ToArray() : new DadosDuplicata[0];

            DadosDuplicata GetDuplicata(Duplicata dup)
            {
                return new DadosDuplicata
                {
                    data = dup.DVenc,
                    n = dup.NDup,
                    valor = dup.VDup
                };
            }
        }

        DadosISSQN GetISSQN()
        {
            var emit = Dados.NFe.Informacoes.Emitente;
            var issqn = Dados.NFe.Informacoes.total.ISSQNtot;
            return issqn != null ? new DadosISSQN
            {
                BC = issqn.vBC.ToString("N2"),
                IM = emit.IM,
                TotalServiços = issqn.vServ.ToString("N2"),
                ValorISSQN = issqn.vISS.ToString("N2")
            } : new DadosISSQN();
        }

        string GetFatura()
        {
            var cobranca = Dados.NFe.Informacoes.cobr;
            if (cobranca?.Fat == null)
            {
                return "PAGAMENTO A VISTA";
            }
            else
            {
                var fat = cobranca.Fat;
                return $"PAGAMENTO A PRAZO - Num.: {fat.NFat}, V. orig.: {fat.VOrig.ToString("N2")}, V. desc.: {fat.VDesc.ToString("N2")}, V. liq.: {fat.VLiq.ToString("N2")}";
            }
        }

        string Analisar(object str) => str != null ? (string)str : string.Empty;
    }
}
