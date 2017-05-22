using BibliotecaCentral.ItensBD;
using BibliotecaCentral.ModeloXML.PartesProcesso;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class Inicio : Page
    {
        public Inicio()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            MainPage.Current.SeAtualizar(Symbol.Home, nameof(Inicio));
        }

        private void AbrirFunção(object sender, TappedRoutedEventArgs e)
        {
            MainPage.Current.AbrirFunçao(Type.GetType($"NFeFacil.View.{(sender as FrameworkElement).Name}"));
        }

        private void CriarNotaFiscal(object sender, TappedRoutedEventArgs e)
        {
            MainPage.Current.AbrirFunçao(typeof(ManipulacaoNotaFiscal),
                new ConjuntoManipuladorNFe
                {
                    NotaSalva = new NFe()
                    {
                        Informações = new Detalhes()
                        {
                            identificação = new Identificacao(),
                            emitente = new Emitente(),
                            destinatário = new Destinatario(),
                            produtos = new List<DetalhesProdutos>(),
                            transp = new Transporte(),
                            cobr = new Cobranca(),
                            infAdic = new InformacoesAdicionais(),
                            exporta = new Exportacao(),
                            compra = new Compra(),
                            cana = new RegistroAquisicaoCana()
                        }
                    },
                    OperacaoRequirida = TipoOperacao.Adicao,
                    StatusAtual = StatusNFe.Edição
                });
        }
    }
}
