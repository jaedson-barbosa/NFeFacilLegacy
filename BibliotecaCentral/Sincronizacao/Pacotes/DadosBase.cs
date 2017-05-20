using BibliotecaCentral.ItensBD;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte;
using System.Collections.Generic;

namespace BibliotecaCentral.Sincronizacao.Pacotes
{
    public struct DadosBase : IPacote
    {
        public List<EmitenteDI> Emitentes { get; set; }
        public List<ClienteDI> Clientes { get; set; }
        public List<Motorista> Motoristas { get; set; }
        public List<BaseProdutoOuServico> Produtos { get; set; }
    }
}
