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
            Nota.Informacoes.transp.Transporta = Nota.Informacoes.transp.Transporta?.ToXElement<Motorista>().HasElements ?? false ? Nota.Informacoes.transp.Transporta : null;
            Nota.Informacoes.transp.VeicTransp = ValidarVeiculo(Nota.Informacoes.transp.VeicTransp) ? Nota.Informacoes.transp.VeicTransp : null;
            Nota.Informacoes.transp.RetTransp = ValidarRetencaoTransporte(Nota.Informacoes.transp.RetTransp) ? Nota.Informacoes.transp.RetTransp : null;

            Nota.Informacoes.total.ISSQNtot = ValidarISSQN(Nota.Informacoes.total.ISSQNtot) ? Nota.Informacoes.total.ISSQNtot : null;
            Nota.Informacoes.total.RetTrib = ValidarRetencaoTributaria(Nota.Informacoes.total.RetTrib) ? Nota.Informacoes.total.RetTrib : null;
            Nota.Informacoes.cobr = ValidarFatura(Nota.Informacoes.cobr?.Fat) ? Nota.Informacoes.cobr : null;
            Nota.Informacoes.infAdic = ValidarInfoAdicional(Nota.Informacoes.infAdic) ? Nota.Informacoes.infAdic : null;
            Nota.Informacoes.exporta = new ValidadorExportacao(Nota.Informacoes.exporta).Validar(null) ? Nota.Informacoes.exporta : null;
            Nota.Informacoes.compra = ValidarCompra(Nota.Informacoes.compra) ? Nota.Informacoes.compra : null;
            Nota.Informacoes.cana = ValidarCana(Nota.Informacoes.cana) ? Nota.Informacoes.cana : null;
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
