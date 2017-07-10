using BibliotecaCentral;
using BibliotecaCentral.ItensBD;
using BibliotecaCentral.Log;
using BibliotecaCentral.Validacao;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class AdicionarVendedor : Page
    {
        private Vendedor vendedor;
        private TipoOperacao tipoRequisitado;
        private ILog Log = Popup.Current;

        public AdicionarVendedor()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            GrupoViewBanco<Vendedor> parametro;
            if (e.Parameter == null)
            {
                parametro = new GrupoViewBanco<Vendedor>
                {
                    ItemBanco = new Vendedor(),
                    OperacaoRequirida = TipoOperacao.Adicao
                };
            }
            else
            {
                parametro = (GrupoViewBanco<Vendedor>)e.Parameter;
            }
            vendedor = parametro.ItemBanco;
            tipoRequisitado = parametro.OperacaoRequirida;
            switch (tipoRequisitado)
            {
                case TipoOperacao.Adicao:
                    MainPage.Current.SeAtualizar(Symbol.Add, "Emitente");
                    break;
                case TipoOperacao.Edicao:
                    MainPage.Current.SeAtualizar(Symbol.Edit, "Emitente");
                    break;
            }
            DataContext = vendedor;
        }

        private void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (new ValidadorVendedor(vendedor).Validar(Log))
                {
                    using (var db = new AplicativoContext())
                    {
                        vendedor.UltimaData = DateTime.Now;
                        if (tipoRequisitado == TipoOperacao.Adicao)
                        {
                            db.Add(vendedor);
                            Log.Escrever(TitulosComuns.Sucesso, "Emitente salvo com sucesso.");
                        }
                        else
                        {
                            db.Update(vendedor);
                            Log.Escrever(TitulosComuns.Sucesso, "Emitente alterado com sucesso.");
                        }
                        db.SaveChanges();
                    }
                    MainPage.Current.Retornar();
                }
            }
            catch (Exception erro)
            {
                erro.ManipularErro();
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Current.Retornar();
        }
    }
}
