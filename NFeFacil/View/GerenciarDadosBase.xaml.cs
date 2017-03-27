using NFeFacil.ItensBD;
using NFeFacil.Log;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte;
using NFeFacil.ViewModel.NotaFiscal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class GerenciarDadosBase : Page
    {
        public GerenciarDadosBase()
        {
            this.InitializeComponent();
            using (var db = new AplicativoContext())
            {
                lstDestinatários.ItemsSource = db.Clientes.GerarObs();
                lstEmitentes.ItemsSource = db.Emitentes.GerarObs();
                lstMotoristas.ItemsSource = db.Motoristas.GerarObs();
                lstProdutos.ItemsSource = db.Produtos.GerarObs();
            }
            Propriedades.Intercambio.SeAtualizar(Telas.GerenciarDadosBase, Symbol.Manage, "Gerenciar dados base");
        }

        private void Adicionar_Click(object sender, RoutedEventArgs e)
        {
            Propriedades.Intercambio.AbrirFunçao(DescobrirTela().ToString());
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
                            lstDestinatários.Items.Remove(cliente);
                        }
                        else erro = true;
                        break;
                    case Pivôs.Emitente:
                        var emitente = lstEmitentes.SelectedItem as EmitenteDI;
                        if (emitente != null)
                        {
                            db.Remove(emitente);
                            lstEmitentes.Items.Remove(emitente);
                        }
                        else erro = true;
                        break;
                    case Pivôs.Motorista:
                        var motorista = lstMotoristas.SelectedItem as MotoristaDI;
                        if (motorista != null)
                        {
                            db.Remove(motorista);
                            lstMotoristas.Items.Remove(motorista);
                        }
                        else erro = true;
                        break;
                    case Pivôs.Produto:
                        var produto = lstProdutos.SelectedItem as ProdutoDI;
                        if (produto != null)
                        {
                            db.Remove(produto);
                            lstProdutos.Items.Remove(produto);
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
                    var emit = lstEmitentes.SelectedItem as Emitente;
                    Propriedades.Intercambio.AbrirFunçao(typeof(AdicionarEmitente), new EmitenteDataContext(ref emit));
                    break;
                case Pivôs.Destinatario:
                    var dest = lstDestinatários.SelectedItem as Destinatario;
                    Propriedades.Intercambio.AbrirFunçao(typeof(AdicionarDestinatario), new ClienteDataContext(ref dest));
                    break;
                case Pivôs.Motorista:
                    var mot = lstMotoristas.SelectedItem as Motorista;
                    Propriedades.Intercambio.AbrirFunçao(typeof(AdicionarMotorista), new MotoristaDataContext(ref mot));
                    break;
                case Pivôs.Produto:
                    Propriedades.Intercambio.AbrirFunçao(typeof(AdicionarProduto), lstProdutos.SelectedItem);
                    break;
            }
        }

        private Pivôs DescobrirTela()
        {
            var tela = (pivôs.SelectedItem as PivotItem).Header as string;
            return (Pivôs)Enum.Parse(typeof(Pivôs), tela);
        }

        public async Task Esconder()
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
