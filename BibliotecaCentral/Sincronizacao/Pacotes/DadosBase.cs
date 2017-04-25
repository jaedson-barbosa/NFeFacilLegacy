using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte;
using System.Collections.Generic;

namespace BibliotecaCentral.Sincronizacao.Pacotes
{
    public sealed class DadosBase : PacoteBase
    {
        public IEnumerable<Emitente> Emitentes { get; set; }
        public IEnumerable<Destinatario> Clientes { get; set; }
        public IEnumerable<Motorista> Motoristas { get; set; }
        public IEnumerable<BaseProdutoOuServico> Produtos { get; set; }
    }
}
