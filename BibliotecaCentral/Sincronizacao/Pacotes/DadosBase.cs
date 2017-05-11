using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte;
using System.Collections.Generic;

namespace BibliotecaCentral.Sincronizacao.Pacotes
{
    public struct DadosBase : IPacote
    {
        public List<Emitente> Emitentes { get; set; }
        public List<Destinatario> Clientes { get; set; }
        public List<Motorista> Motoristas { get; set; }
        public List<BaseProdutoOuServico> Produtos { get; set; }
    }
}
