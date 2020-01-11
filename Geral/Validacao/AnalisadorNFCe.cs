using BaseGeral.ModeloXML;
using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesTransporte;

namespace BaseGeral.Validacao
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

            var info = Nota.Informacoes;
            info.infAdic = ValidarInfoAdicional(info.infAdic) ? info.infAdic : null;

            if (Nota.AmbienteTestes)
            {
                var prod = Nota.Informacoes.produtos[0];
                prod.Produto.Descricao = "NOTA FISCAL EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL";
            }
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
