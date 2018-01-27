using NFeFacil.ModeloXML;
using NFeFacil.ModeloXML.PartesDetalhes;
using NFeFacil.ModeloXML.PartesDetalhes.PartesTransporte;

namespace NFeFacil.Validacao
{
    public sealed class AnalisadorNFCe
    {
        private NFCe Nota { get; }

        public AnalisadorNFCe(ref NFCe nota)
        {
            Nota = nota;
        }

        public void Normalizar()
        {
            var transp = Nota.Informacoes.transp;
            transp.Transporta = ValidarMotorista(transp.Transporta) ? transp.Transporta : null;

            var total = Nota.Informacoes.total;
            total.ISSQNtot = null;

            var info = Nota.Informacoes;
            info.infAdic = ValidarInfoAdicional(info.infAdic) ? info.infAdic : null;
        }

        public void Desnormalizar()
        {
            if (Nota.Informacoes.transp.Transporta == null)
            {
                Nota.Informacoes.transp.Transporta = new Motorista();
            }

            if (Nota.Informacoes.infAdic == null)
            {
                Nota.Informacoes.infAdic = new InformacoesAdicionais();
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

        bool ValidarInfoAdicional(InformacoesAdicionais info)
        {
            if (info == null)
            {
                return false;
            }
            else
            {
                return !string.IsNullOrEmpty(info.InfCpl);
            }
        }
    }
}
