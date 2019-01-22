using BaseGeral.ModeloXML;
using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesTotal;
using BaseGeral.ModeloXML.PartesDetalhes.PartesTransporte;
using System.Linq;

namespace BaseGeral.Validacao
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
            total.RetTrib = ValidarRetencaoTributaria(total.RetTrib) ? total.RetTrib : null;

            var info = Nota.Informacoes;
            info.cobr = ValidarFatura(info.cobr?.Fat) ? info.cobr : null;
            info.infAdic = ValidarInfoAdicional(info.infAdic) ? info.infAdic : null;
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
            if (Nota.Informacoes.cana == null)
            {
                Nota.Informacoes.cana = new RegistroAquisicaoCana();
            }
            if (Nota.Informacoes.Pagamento == null)
            {
                Nota.Informacoes.Pagamento = new DetalhamentoPagamento();
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
                var errados = new bool[2]
                {
                        string.IsNullOrEmpty(info.InfCpl),
                        info.ProcRef.Count == 0
                };
                return errados.Count(x => x) < 2;
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
