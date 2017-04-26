using BibliotecaCentral.ItensBD;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BibliotecaCentral.Repositorio
{
    internal class MudancaOtimizadaBancoDados : ConexaoBanco
    {
        internal void AdicionarEmitentes(IEnumerable<Emitente> emitentes)
        {
            var analise = from emit in emitentes
                          group emit by Contexto.Emitentes.Count(x => x.CNPJ == emit.CNPJ) == 0;
            foreach (var item in analise)
            {
                if (item.Key)
                {
                    Contexto.AddRange(item);
                }
                else
                {
                    Contexto.UpdateRange(item);
                }
            }
        }

        internal void AdicionarClientes(IEnumerable<Destinatario> clientes)
        {
            var analise = from cli in clientes
                          group cli by Contexto.Clientes.Count(x => x.Documento == cli.Documento) == 0;
            foreach (var item in analise)
            {
                if (item.Key)
                {
                    Contexto.AddRange(item);
                }
                else
                {
                    Contexto.UpdateRange(item);
                }
            }
        }

        internal void AdicionarMotoristas(IEnumerable<Motorista> motoristas)
        {
            var analise = from mot in motoristas
                          group mot by Contexto.Motoristas.Count(x => x.Documento == mot.Documento) == 0;
            foreach (var item in analise)
            {
                if (item.Key)
                {
                    Contexto.AddRange(item);
                }
                else
                {
                    Contexto.UpdateRange(item);
                }
            }
        }

        internal void AdicionarProdutos(IEnumerable<BaseProdutoOuServico> produtos)
        {
            var analise = from prod in produtos
                          group prod by Contexto.Produtos.Count(x => x.Descricao == prod.Descricao) == 0;
            foreach (var item in analise)
            {
                if (item.Key)
                {
                    Contexto.AddRange(item);
                }
                else
                {
                    Contexto.UpdateRange(item);
                }
            }
        }

        internal async Task AdicionarNotasFiscais(IDictionary<NFeDI, XElement> notas)
        {
            var analise = from nota in notas
                          group nota.Key by Contexto.NotasFiscais.Count(x => x.Id == nota.Key.Id) == 0;
            foreach (var item in analise)
            {
                if (item.Key) Contexto.AddRange(item);
                else Contexto.UpdateRange(item);
            }
            foreach (var item in notas)
            {
                PastaNotasFiscais pasta = new PastaNotasFiscais();
                await pasta.AdicionarOuAtualizar(item.Value, item.Key.Id);
            }
        }
    }
}
