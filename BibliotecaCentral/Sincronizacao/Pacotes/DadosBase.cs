using BibliotecaCentral.ItensBD;
using System.Collections.Generic;

namespace BibliotecaCentral.Sincronizacao.Pacotes
{
    public struct DadosBase : IPacote
    {
        public List<EmitenteDI> Emitentes { get; set; }
        public List<ClienteDI> Clientes { get; set; }
        public List<MotoristaDI> Motoristas { get; set; }
        public List<ProdutoDI> Produtos { get; set; }
    }
}
