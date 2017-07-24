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

        public AnalisadorNFe(NFe nota)
        {
            Nota = nota;
        }

        public void Normalizar()
        {
            Nota.Informações.transp.Transporta = Nota.Informações.transp.Transporta?.ToXElement<Motorista>().HasElements ?? false ? Nota.Informações.transp.Transporta : null;
            Nota.Informações.transp.VeicTransp = ValidarVeiculo(Nota.Informações.transp.VeicTransp) ? Nota.Informações.transp.VeicTransp : null;
            Nota.Informações.transp.RetTransp = ValidarRetencaoTransporte(Nota.Informações.transp.RetTransp) ? Nota.Informações.transp.RetTransp : null;

            Nota.Informações.total.ISSQNtot = ValidarISSQN(Nota.Informações.total.ISSQNtot) ? Nota.Informações.total.ISSQNtot : null;
            Nota.Informações.total.RetTrib = ValidarRetencaoTributaria(Nota.Informações.total.RetTrib) ? Nota.Informações.total.RetTrib : null;
            Nota.Informações.cobr = ValidarFatura(Nota.Informações.cobr?.Fat) ? Nota.Informações.cobr : null;
            Nota.Informações.infAdic = ValidarInfoAdicional(Nota.Informações.infAdic) ? Nota.Informações.infAdic : null;
            Nota.Informações.exporta = new ValidadorExportacao(Nota.Informações.exporta).Validar(null) ? Nota.Informações.exporta : null;
            Nota.Informações.compra = ValidarCompra(Nota.Informações.compra) ? Nota.Informações.compra : null;
            Nota.Informações.cana = ValidarCana(Nota.Informações.cana) ? Nota.Informações.cana : null;
        }

        public void Desnormalizar()
        {
            if (Nota.Informações.transp.Transporta == null)
            {
                Nota.Informações.transp.Transporta = new Motorista();
            }
            if (Nota.Informações.transp.VeicTransp == null)
            {
                Nota.Informações.transp.VeicTransp = new Veiculo();
            }
            if (Nota.Informações.transp.RetTransp == null)
            {
                Nota.Informações.transp.RetTransp = new ICMSTransporte();
            }

            if (Nota.Informações.cobr == null)
            {
                Nota.Informações.cobr = new Cobranca();
            }
            if (Nota.Informações.infAdic == null)
            {
                Nota.Informações.infAdic = new InformacoesAdicionais();
            }
            if (Nota.Informações.exporta == null)
            {
                Nota.Informações.exporta = new Exportacao();
            }
            if (Nota.Informações.compra == null)
            {
                Nota.Informações.compra = new Compra();
            }
            if (Nota.Informações.cana == null)
            {
                Nota.Informações.cana = new RegistroAquisicaoCana();
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
