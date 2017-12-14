using NFeFacil.ModeloXML.PartesProcesso;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTotal;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte;
using System.Linq;

namespace NFeFacil.Validacao
{
    public sealed class AnalisadorNFe
    {
        private NFe Nota { get; }

        public AnalisadorNFe(ref NFe nota)
        {
            Nota = nota;
        }

        public void Normalizar()
        {
            var transp = Nota.Informacoes.transp;
            transp.Transporta = ValidarMotorista(transp.Transporta) ? transp.Transporta : null;
            transp.VeicTransp = ValidarVeiculo(transp.VeicTransp) ? transp.VeicTransp : null;
            transp.RetTransp = ValidarRetencaoTransporte(transp.RetTransp) ? transp.RetTransp : null;

            var total = Nota.Informacoes.total;
            total.ISSQNtot = ValidarISSQN(total.ISSQNtot) ? total.ISSQNtot : null;
            total.RetTrib = ValidarRetencaoTributaria(total.RetTrib) ? total.RetTrib : null;

            var info = Nota.Informacoes;
            info.cobr = ValidarFatura(info.cobr?.Fat) ? info.cobr : null;
            info.infAdic = ValidarInfoAdicional(info.infAdic) ? info.infAdic : null;
            info.exporta = new ValidadorExportacao(info.exporta).Validar(null) ? info.exporta : null;
            info.compra = ValidarCompra(info.compra) ? info.compra : null;
            info.cana = ValidarCana(info.cana) ? info.cana : null;
        }

        public void Desnormalizar()
        {
            if (Nota.Informacoes.transp.Transporta == null)
            {
                Nota.Informacoes.transp.Transporta = new Motorista();
            }
            if (Nota.Informacoes.transp.VeicTransp == null)
            {
                Nota.Informacoes.transp.VeicTransp = new Veiculo();
            }
            if (Nota.Informacoes.transp.RetTransp == null)
            {
                Nota.Informacoes.transp.RetTransp = new ICMSTransporte();
            }

            if (Nota.Informacoes.cobr == null)
            {
                Nota.Informacoes.cobr = new Cobranca();
            }
            if (Nota.Informacoes.infAdic == null)
            {
                Nota.Informacoes.infAdic = new InformacoesAdicionais();
            }
            if (Nota.Informacoes.exporta == null)
            {
                Nota.Informacoes.exporta = new Exportacao();
            }
            if (Nota.Informacoes.compra == null)
            {
                Nota.Informacoes.compra = new Compra();
            }
            if (Nota.Informacoes.cana == null)
            {
                Nota.Informacoes.cana = new RegistroAquisicaoCana();
            }

            Nota.Signature = null;
        }

        bool ValidarMotorista(Motorista mot)
        {
            if (mot == null)
            {
                return false;
            }
            else
            {
                return !string.IsNullOrEmpty(mot.Nome);
            }
        }

        bool ValidarVeiculo(Veiculo veic)
        {
            if (veic == null)
            {
                return false;
            }
            else
            {
                return StringsNaoNulas(veic.Placa, veic.UF);
            }
        }

        bool ValidarRetencaoTransporte(ICMSTransporte icms)
        {
            if (icms == null)
            {
                return false;
            }
            else
            {
                return (icms.VServ != 0 || icms.VBCRet != 0 || icms.PICMSRet != 0 || icms.VICMSRet != 0)
                    && icms.CFOP != 0 && icms.CMunFG != 0;
            }
        }

        bool ValidarISSQN(ISSQNtot tot)
        {
            if (tot == null)
            {
                return false;
            }
            else
            {
                return !string.IsNullOrEmpty(tot.DCompet);
            }
        }

        bool ValidarRetencaoTributaria(RetTrib ret)
        {
            if (ret == null)
            {
                return false;
            }
            else
            {
                return NumerosNaoNulos(ret.VBCIRRF, ret.VBCRetPrev, ret.VIRRF, ret.VRetCOFINS,
                    ret.VRetCSLL, ret.VRetPIS, ret.VRetPrev);
            }
        }

        bool ValidarFatura(Fatura fat)
        {
            if (fat == null)
            {
                return false;
            }
            else
            {
                int errados = 0;
                if (string.IsNullOrEmpty(fat.NFat)) errados++;
                else if (int.Parse(fat.NFat) == 0) errados++;

                if (fat.VDesc == 0) errados++;
                if (fat.VLiq == 0) errados++;
                if (fat.VOrig == 0) errados++;

                return errados <= 2;
            }
        }

        bool ValidarInfoAdicional(InformacoesAdicionais info)
        {
            if (info == null)
            {
                return false;
            }
            else
            {
                var errados = new bool[3]
                {
                        string.IsNullOrEmpty(info.InfCpl),
                        info.ObsCont.Count == 0,
                        info.ProcRef.Count == 0
                };
                return errados.Count(x => x) < 3;
            }
        }

        bool ValidarCompra(Compra compra)
        {
            if (compra == null)
            {
                return false;
            }
            else
            {
                return StringsNaoNulas(compra.XCont, compra.XNEmp, compra.XPed);
            }
        }

        bool ValidarCana(RegistroAquisicaoCana cana)
        {
            if (cana == null)
            {
                return false;
            }
            else
            {
                return cana.ForDia.Count > 0;
            }
        }

        bool NumerosNaoNulos(params double[] numeros)
        {
            for (int i = 0; i < numeros.Length; i++)
            {
                if (numeros[i] != 0)
                {
                    return true;
                }
            }
            return false;
        }

        bool StringsNaoNulas(params string[] strings)
        {
            for (int i = 0; i < strings.Length; i++)
            {
                if (string.IsNullOrEmpty(strings[i]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
