using BaseGeral.ItensBD;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Comum
{
    struct MotoristaManipulacaoNFe
    {
        public MotoristaDI Root { get; set; }
        public VeiculoDI Principal { get; set; }
        public VeiculoDI[] Secundarios { get; set; }
    }
}
