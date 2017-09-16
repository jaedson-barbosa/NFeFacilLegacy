using NFeFacil.ItensBD;
using System.Collections.Generic;

namespace NFeFacil.Sincronizacao.Pacotes
{
    public struct ConjuntoBanco : IPacote
    {
        public List<ClienteDI> Clientes { get; set; }
        public List<EmitenteDI> Emitentes { get; set; }
        public List<MotoristaDI> Motoristas { get; set; }
        public List<Vendedor> Vendedores { get; set; }
        public List<ProdutoDI> Produtos { get; set; }
        public List<Estoque> Estoque { get; set; }
        public List<VeiculoDI> Veiculos { get; set; }
        public List<NFeDI> NotasFiscais { get; set; }
        public List<RegistroVenda> Vendas { get; set; }
        public List<RegistroCancelamento> Cancelamentos { get; set; }
        public List<CancelamentoRegistroVenda> CancelamentosRegistroVenda { get; set; }
        public List<Imagem> Imagens { get; set; }
    }
}
