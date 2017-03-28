using Microsoft.EntityFrameworkCore;
using NFeFacil.ItensBD;
using NFeFacil.Log;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class GerenciarDadosBase : Page, IEsconde
    {
        public GerenciarDadosBase()
        {
            this.InitializeComponent();
            using (var db = new AplicativoContext())
            {
                lstDestinatários.ItemsSource = db.Clientes.Include(x => x.endereco).ToList();
                lstEmitentes.ItemsSource = db.Emitentes.Include(x => x.endereco).ToList();
                lstMotoristas.ItemsSource = db.Motoristas.ToList();
                lstProdutos.ItemsSource = db.Produtos.ToList();
            }
            Propriedades.Intercambio.SeAtualizar(Telas.GerenciarDadosBase, Symbol.Manage, "Gerenciar dados base");
        }

        private void Adicionar_Click(object sender, RoutedEventArgs e)
        {
            Propriedades.Intercambio.AbrirFunçao($"Adicionar{DescobrirTela()}");
        }

        private void Deletar_Click(object sender, RoutedEventArgs e)
        {
            bool erro = false;
            using (var db = new AplicativoContext())
            {
                switch (DescobrirTela())
                {
                    case Pivôs.Destinatario:
                        var cliente = lstDestinatários.SelectedItem as ClienteDI;
                        if (cliente != null)
                        {
                            db.Remove(cliente);
                            db.SaveChanges();
                            lstDestinatários.ItemsSource = db.Clientes.Include(x => x.endereco).ToList();
                        }
                        else erro = true;
                        break;
                    case Pivôs.Emitente:
                        var emitente = lstEmitentes.SelectedItem as EmitenteDI;
                        if (emitente != null)
                        {
                            db.Remove(emitente);
                            db.SaveChanges();
                            lstEmitentes.ItemsSource = db.Emitentes.Include(x => x.endereco).ToList();
                        }
                        else erro = true;
                        break;
                    case Pivôs.Motorista:
                        var motorista = lstMotoristas.SelectedItem as MotoristaDI;
                        if (motorista != null)
                        {
                            db.Remove(motorista);
                            db.SaveChanges();
                            lstMotoristas.ItemsSource = db.Motoristas.ToList();
                        }
                        else erro = true;
                        break;
                    case Pivôs.Produto:
                        var produto = lstProdutos.SelectedItem as ProdutoDI;
                        if (produto != null)
                        {
                            db.Remove(produto);
                            db.SaveChanges();
                            lstProdutos.ItemsSource = db.Produtos.ToList();
                        }
                        else erro = true;
                        break;
                    default:
                        break;
                }
                db.SaveChanges();
            }
            if (erro) new Popup().Escrever(TitulosComuns.ErroSimples, "Escolha ao menos um item.");
        }

        private void Editar_Click(object sender, RoutedEventArgs e)
        {
            switch (DescobrirTela())
            {
                case Pivôs.Emitente:
                    Propriedades.Intercambio.AbrirFunçao(typeof(AdicionarEmitente), new GrupoViewBanco<EmitenteDI>
                    {
                        ItemBanco = lstEmitentes.SelectedItem as EmitenteDI,
                        OperacaoRequirida = TipoOperacao.Edicao
                    });
                    break;
                case Pivôs.Destinatario:
                    Propriedades.Intercambio.AbrirFunçao(typeof(AdicionarDestinatario), new GrupoViewBanco<ClienteDI>
                    {
                        ItemBanco = lstDestinatários.SelectedItem as ClienteDI,
                        OperacaoRequirida = TipoOperacao.Edicao
                    });
                    break;
                case Pivôs.Motorista:
                    Propriedades.Intercambio.AbrirFunçao(typeof(AdicionarMotorista), new GrupoViewBanco<MotoristaDI>
                    {
                        ItemBanco = lstMotoristas.SelectedItem as MotoristaDI,
                        OperacaoRequirida = TipoOperacao.Edicao
                    });
                    break;
                case Pivôs.Produto:
                    Propriedades.Intercambio.AbrirFunçao(typeof(AdicionarProduto), new GrupoViewBanco<ProdutoDI>
                    {
                        ItemBanco = lstProdutos.SelectedItem as ProdutoDI,
                        OperacaoRequirida = TipoOperacao.Edicao
                    });
                    break;
            }
        }

        private Pivôs DescobrirTela()
        {
            var tela = (pivôs.SelectedItem as PivotItem).Header as string;
            return (Pivôs)Enum.Parse(typeof(Pivôs), tela);
        }

        public async Task EsconderAsync()
        {
            ocultarGrid.Begin();
            await Task.Delay(250);
        }

        private enum Pivôs
        {
            Destinatario,
            Emitente,
            Motorista,
            Produto
        }
    }
}
