using BibliotecaCentral.ModeloXML.PartesProcesso;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTotal;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte;
using System.Linq;

namespace BibliotecaCentral.Validacao
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
            Nota.Informações.transp.transporta = Nota.Informações.transp.transporta?.ToXElement<Motorista>().HasElements ?? false ? Nota.Informações.transp.transporta : null;
            Nota.Informações.transp.veicTransp = ValidarVeiculo(Nota.Informações.transp.veicTransp) ? Nota.Informações.transp.veicTransp : null;
            Nota.Informações.transp.retTransp = Nota.Informações.transp.retTransp?.ToXElement<ICMSTransporte>().HasElements ?? false ? Nota.Informações.transp.retTransp : null;

            Nota.Informações.total.ISSQNtot = ValidarISSQN(Nota.Informações.total.ISSQNtot) ? Nota.Informações.total.ISSQNtot : null;
            Nota.Informações.total.retTrib = ValidarRetencaoTributaria(Nota.Informações.total.retTrib) ? Nota.Informações.total.retTrib : null;
            Nota.Informações.cobr = ValidarFatura(Nota.Informações.cobr?.Fat) ? Nota.Informações.cobr : null;
            Nota.Informações.infAdic = ValidarInfoAdicional(Nota.Informações.infAdic) ? Nota.Informações.infAdic : null;
            Nota.Informações.exporta = new ValidadorExportacao(Nota.Informações.exporta).Validar(null) ? Nota.Informações.exporta : null;
            Nota.Informações.compra = ValidarCompra(Nota.Informações.compra) ? Nota.Informações.compra : null;
            Nota.Informações.cana = ValidarCana(Nota.Informações.cana) ? Nota.Informações.cana : null;
        }

        public void Desnormalizar()
        {
            if (Nota.Informações.transp.transporta == null)
            {
                Nota.Informações.transp.transporta = new Motorista();
            }
            if (Nota.Informações.transp.veicTransp == null)
            {
                Nota.Informações.transp.veicTransp = new Veiculo();
            }
            if (Nota.Informações.transp.retTransp == null)
            {
                Nota.Informações.transp.retTransp = new ICMSTransporte();
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

        bool ValidarISSQN(ISSQNtot tot)
        {
            if (tot == null)
            {
                return false;
            }
            else
            {
                return !string.IsNullOrEmpty(tot.dCompet);
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
                return StringsNaoNulas(ret.vBCIRRF, ret.vBCRetPrev, ret.vIRRF, ret.vRetCOFINS,
                    ret.vRetCSLL, ret.vRetPIS, ret.vRetPrev);
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

                if (string.IsNullOrEmpty(fat.VDesc)) errados++;
                else if (int.Parse(fat.VDesc) == 0) errados++;

                if (string.IsNullOrEmpty(fat.VLiq)) errados++;
                else if (int.Parse(fat.VLiq) == 0) errados++;

                if (string.IsNullOrEmpty(fat.VOrig)) errados++;
                else if (int.Parse(fat.VOrig) == 0) errados++;

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
                        string.IsNullOrEmpty(info.infCpl),
                        info.obsCont.Count == 0,
                        info.procRef.Count == 0
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
                return cana.forDia.Count > 0;
            }
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
