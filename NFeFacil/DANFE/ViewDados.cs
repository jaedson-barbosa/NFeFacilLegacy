using NFeFacil.DANFE.Pacotes;
using NFeFacil.ModeloXML;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using NFeFacil.PacotesImpressaoGenericos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NFeFacil.DANFE
{
    public struct ViewDados
    {
        Processo Dados { get; }

        public ViewDados(Processo processo)
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
            var extras = Dados.NFe.Informações.infAdic;
            var cobr = Dados.NFe.Informações.cobr;
            var entrega = Dados.NFe.Informações.Entrega;
            var retirada = Dados.NFe.Informações.Retirada;

            var itens = new List<ItemDadosAdicionais>();
            if (retirada != null)
            {
                itens.Add(new ItemDadosAdicionais("ENDEREÇO DE RETIRADA:", $"{retirada.Logradouro}, {retirada.Numero}", retirada.Bairro, $"{retirada.NomeMunicipio} - {retirada.SiglaUF}", retirada.CNPJ != null ? $"CNPJ: {AplicatMascaraDocumento(retirada.CNPJ)}" : $"CPF: {AplicatMascaraDocumento(retirada.CPF)}"));
            }
            if (entrega != null)
            {
                itens.Add(new ItemDadosAdicionais("ENDEREÇO DE ENTREGA:", $"{entrega.Logradouro}, {entrega.Numero}", entrega.Bairro, $"{entrega.NomeMunicipio} - {entrega.SiglaUF}", entrega.CNPJ != null ? $"CNPJ: {AplicatMascaraDocumento(entrega.CNPJ)}" : $"CPF: {AplicatMascaraDocumento(entrega.CPF)}"));
            }
            if (cobr?.Dup != null)
            {
                itens.Add(new ItemDadosAdicionais("DUPLICATAS:", cobr.Dup.Select(dup => $"Duplicata - Num.: {dup.NDup}, Vec.: {dup.DVenc}, Valor: {dup.VDup.ToString("N2")}")));
            }
            if (extras?.InfCpl != null)
            {
                itens.Add(new ItemDadosAdicionais("DE INTERESSE DO CONTRIBUINTE:", extras.InfCpl));
            }
            if (extras?.InfAdFisco != null)
            {
                itens.Add(new ItemDadosAdicionais("DE INTERESSE DO FISCO:", extras.InfAdFisco));
            }
            return new DadosAdicionais(itens);
        }

        DadosCabecalho GetCabecalho()
        {
            var ident = Dados.NFe.Informações.identificação;
            var emit = Dados.NFe.Informações.emitente;
            return new DadosCabecalho
            {
                NomeEmitente = emit.Nome,
                SerieNota = ident.Serie.ToString(),
                NumeroNota = ident.Numero.ToString("000,000,000")
            };
        }

        DadosCliente GetCliente()
        {
            var ident = Dados.NFe.Informações.identificação;
            var dest = Dados.NFe.Informações.destinatário;
            return new DadosCliente
            {
                DocCliente = AplicatMascaraDocumento(dest.Documento),
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
            var tot = Dados.NFe.Informações.total;
            return new DadosImposto
            {
                BaseCalculoICMS = tot.ICMSTot.VBC.ToString("N2"),
                BaseCalculoICMSST = tot.ICMSTot.VBCST.ToString("N2"),
                Desconto = tot.ICMSTot.VDesc.ToString("N2"),
                DespesasAcessorias = tot.ICMSTot.VOutro.ToString("N2"),
                TotalNota = tot.ICMSTot.VNF.ToString("N2"),
                ValorFrete = tot.ICMSTot.VFrete.ToString("N2"),
                ValorICMS = tot.ICMSTot.VICMS.ToString("N2"),
                ValorICMSST = tot.ICMSTot.VST.ToString("N2"),
                ValorIPI = tot.ICMSTot.VIPI.ToString("N2"),
                ValorSeguro = tot.ICMSTot.VSeg.ToString("N2"),
                ValorTotalProdutos = tot.ICMSTot.VProd.ToString("N2")
            };
        }

        DadosMotorista GetMotorista()
        {
            var transp = Dados.NFe.Informações.transp;
            var retorno = new DadosMotorista
            {
                CodigoANTT = Analisar(transp.VeicTransp?.RNTC),
                DocumentoMotorista = transp.Transporta?.Documento != null ? AplicatMascaraDocumento(transp.Transporta?.Documento) : null,
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
                retorno.QuantidadeVolume = transp.Vol[0].QVol.ToString("N3");
            }
            else
            {
                retorno.EspecieVolume = string.Empty;
                retorno.MarcaVolume = string.Empty;
                retorno.NumeroVolume = string.Empty;
                retorno.PesoBrutoVolume = transp.Vol.Sum(x => x.PesoB).ToString("N3");
                retorno.PesoLiquidoVolume = transp.Vol.Sum(x => x.PesoL).ToString("N3");
                retorno.QuantidadeVolume = transp.Vol.Sum(x => x.QVol).ToString("N3");
            }

            switch (transp.ModFrete)
            {
                case 0:
                    retorno.ModalidadeFrete = "0 – Emitente";
                    break;
                case 1:
                    retorno.ModalidadeFrete = "1 – Dest/Rem";
                    break;
                case 2:
                    retorno.ModalidadeFrete = "2 – Terceiros";
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
            var detalhes = Dados.NFe.Informações;
            var prot = Dados.ProtNFe;
            var codigoBarras = detalhes.Id.Substring(detalhes.Id.IndexOf('e') + 1);
            var retorno = new DadosNFe
            {
                Chave = codigoBarras,
                ChaveComMascara = AplicarMascaraChave(codigoBarras),
                CNPJEmit = AplicatMascaraDocumento(detalhes.emitente.CNPJ),
                DataHoraRecibo = prot.InfProt.dhRecbto.Replace('T', ' '),
                Endereco = detalhes.emitente.Endereco,
                IE = detalhes.emitente.InscricaoEstadual.ToString(),
                IEST = detalhes.emitente.IEST,
                NatOp = detalhes.identificação.NaturezaDaOperação,
                NomeEmitente = detalhes.emitente.Nome,
                NumeroNota = detalhes.identificação.Numero.ToString("000,000,000"),
                NumeroProtocolo = prot.InfProt.nProt.ToString(),
                SerieNota = detalhes.identificação.Serie.ToString(),
                TipoEmissao = detalhes.identificação.TipoEmissão.ToString()
            };
            using (var db = new AplicativoContext())
            {
                var di = db.Emitentes.FirstOrDefault(x => long.Parse(x.CNPJ) == detalhes.emitente.CNPJ);
                if (di != null)
                {
                    var img = db.Imagens.Find(di.Id);
                    if (img != null)
                    {
                        var task = img.GetSourceAsync();
                        task.Wait();
                        retorno.Logotipo = task.Result;
                    }
                }
                return retorno;
            }

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
            return (from p in Dados.NFe.Informações.produtos
                    select GetProd(p)).ToArray();

            DadosProduto GetProd(DetalhesProdutos prod)
            {
                var consult = new ConsultarImpostos(prod.Impostos.ToXElement<Impostos>());
                return new DadosProduto
                {
                    CFOP = prod.Produto.CFOP.ToString(),
                    CProd = prod.Produto.CodigoProduto,
                    CSTICMS = GetCSTICMS(),
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
                    InfoAdicional = prod.InfAdProd
                };


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
            var duplicatas = Dados.NFe.Informações.cobr?.Dup;
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
            var emit = Dados.NFe.Informações.emitente;
            var issqn = Dados.NFe.Informações.total.ISSQNtot;
            return issqn != null ? new DadosISSQN
            {
                BC = issqn.VBC.ToString("N2"),
                IM = emit.IM,
                TotalServiços = issqn.VServ.ToString("N2"),
                ValorISSQN = issqn.VISS.ToString("N2")
            } : new DadosISSQN();
        }

        string GetFatura()
        {
            var cobranca = Dados.NFe.Informações.cobr;
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

        string AplicatMascaraDocumento(string original)
        {
            original = original.Trim();
            if (original.Length == 14)
            {
                // É CNPJ
                return $"{original.Substring(0, 2)}.{original.Substring(2, 3)}.{original.Substring(5, 3)}/{original.Substring(8, 4)}.{original.Substring(12, 2)}";
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

        string AplicatMascaraDocumento(long numero)
        {
            var original = numero.ToString();
            if (original.Length == 14)
            {
                // É CNPJ
                return $"{original.Substring(0, 2)}.{original.Substring(2, 3)}.{original.Substring(5, 3)}/{original.Substring(8, 4)}.{original.Substring(12, 2)}";
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
