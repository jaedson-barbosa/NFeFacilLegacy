using BibliotecaCentral;
using BibliotecaCentral.ItensBD;
using NFeFacil.ViewModel;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class ManipulacaoRegistroVenda : Page
    {
        public ManipulacaoRegistroVenda()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            RegistroVendaDataContext contexto;
            if (e.Parameter is GrupoViewBanco<RegistroVenda> grupo)
            {
                contexto = new RegistroVendaDataContext(grupo.ItemBanco, AtualizarCabecalho);
                AtualizarCabecalho(ExibicaoExtra.ExibirVendedor, grupo.ItemBanco.Vendedor);
            }
            else
            {
                contexto = new RegistroVendaDataContext(AtualizarCabecalho);
                AtualizarCabecalho(ExibicaoExtra.EscolherVendedor);
            }
            DataContext = contexto;
        }

        void AtualizarCabecalho(ExibicaoExtra exibicao, Guid idVendedor = default(Guid))
        {
            switch (exibicao)
            {
                case ExibicaoExtra.ExibirVendedor:
                    using (var db = new AplicativoContext())
                    {
                        var nomeVendedor = idVendedor != default(Guid) ? db.Vendedores.Find(idVendedor).Nome : string.Empty;
                        MainPage.Current.SeAtualizarEspecial("\uEC59", "Venda", ExibicaoExtra.ExibirVendedor, nomeVendedor);
                    }
                    break;
                case ExibicaoExtra.EscolherVendedor:
                    MainPage.Current.SeAtualizarEspecial("\uEC59", "Venda", ExibicaoExtra.EscolherVendedor, null);
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            ((IDisposable)DataContext).Dispose();
        }
    }
}
