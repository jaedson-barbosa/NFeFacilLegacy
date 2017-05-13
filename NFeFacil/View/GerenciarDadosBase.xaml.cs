using BibliotecaCentral.Log;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using BibliotecaCentral.Repositorio;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;

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
            InitializeComponent();
            using (var clientes = new Clientes())
            using (var emitentes = new Emitentes())
            using (var motoristas = new Motoristas())
            using (var produtos = new Produtos())
            {
                lstDestinatários.ItemsSource = clientes.Registro.ToList();
                lstEmitentes.ItemsSource = emitentes.Registro.ToList();
                lstMotoristas.ItemsSource = motoristas.Registro.ToList();
                lstProdutos.ItemsSource = produtos.Registro.ToList();
            }
            MainPage.Current.SeAtualizar(Telas.GerenciarDadosBase, Symbol.Manage, "Gerenciar dados base");
        }

        private async void Adicionar_ClickAsync(object sender, RoutedEventArgs e)
        {
            await MainPage.Current.AbrirFunçaoAsync($"Adicionar{DescobrirTela()}");
        }

        private void Deletar_Click(object sender, RoutedEventArgs e)
        {
            var telaEscolhida = DescobrirTela();
            if (telaEscolhida == Pivôs.Destinatario)
            {
                var cliente = lstDestinatários.SelectedItem as Destinatario;
                if (cliente != null)
                {
                    using (var db = new Clientes())
                    {
                        db.Remover(cliente);
                        db.SalvarMudancas();
                        lstDestinatários.ItemsSource = db.Registro.ToList();
                        return;
                    }
                }
            }
            else if (telaEscolhida == Pivôs.Emitente)
            {
                var emitente = lstEmitentes.SelectedItem as Emitente;
                if (emitente != null)
                {
                    using (var db = new Emitentes())
                    {
                        db.Remover(emitente);
                        db.SalvarMudancas();
                        lstEmitentes.ItemsSource = db.Registro.ToList();
                        return;
                    }
                }
            }
            else if (telaEscolhida == Pivôs.Motorista)
            {
                var motorista = lstMotoristas.SelectedItem as Motorista;
                if (motorista != null)
                {
                    using (var db = new Motoristas())
                    {
                        db.Remover(motorista);
                        db.SalvarMudancas();
                        lstMotoristas.ItemsSource = db.Registro.ToList();
                        return;
                    }
                }
            }
            else
            {
                var Produto = lstProdutos.SelectedItem as BaseProdutoOuServico;
                if (Produto != null)
                {
                    using (var db = new Produtos())
                    {
                        db.Remover(Produto);
                        db.SalvarMudancas();
                        lstProdutos.ItemsSource = db.Registro.ToList();
                        return;
                    }
                }
            }
            new Popup().Escrever(TitulosComuns.ErroSimples, "Escolha ao menos um item.");
        }

        private async void Editar_ClickAsync(object sender, RoutedEventArgs e)
        {
            switch (DescobrirTela())
            {
                case Pivôs.Emitente:
                    await MainPage.Current.AbrirFunçaoAsync(typeof(AdicionarEmitente), new GrupoViewBanco<Emitente>
                    {
                        ItemBanco = lstEmitentes.SelectedItem as Emitente,
                        OperacaoRequirida = TipoOperacao.Edicao
                    });
                    break;
                case Pivôs.Destinatario:
                    await MainPage.Current.AbrirFunçaoAsync(typeof(AdicionarDestinatario), new GrupoViewBanco<Destinatario>
                    {
                        ItemBanco = lstDestinatários.SelectedItem as Destinatario,
                        OperacaoRequirida = TipoOperacao.Edicao
                    });
                    break;
                case Pivôs.Motorista:
                    await MainPage.Current.AbrirFunçaoAsync(typeof(AdicionarMotorista), new GrupoViewBanco<Motorista>
                    {
                        ItemBanco = lstMotoristas.SelectedItem as Motorista,
                        OperacaoRequirida = TipoOperacao.Edicao
                    });
                    break;
                case Pivôs.Produto:
                    await MainPage.Current.AbrirFunçaoAsync(typeof(AdicionarProduto), new GrupoViewBanco<BaseProdutoOuServico>
                    {
                        ItemBanco = lstProdutos.SelectedItem as BaseProdutoOuServico,
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
