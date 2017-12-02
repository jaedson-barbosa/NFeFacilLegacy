using NFeFacil.ItensBD;
using NFeFacil.Log;
using NFeFacil.Validacao;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class AdicionarComprador : Page
    {
        Comprador Comprador;
        ILog Log = Popup.Current;
        ObservableCollection<ClienteDI> ClientesDisponiveis { get; }

        public AdicionarComprador()
        {
            InitializeComponent();
            using (var db = new AplicativoContext())
            {
                ClientesDisponiveis = (from cli in db.Clientes
                                       where cli.Ativo && !string.IsNullOrEmpty(cli.CNPJ)
                                       select cli).GerarObs();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter == null)
            {
                Comprador = new Comprador();
                MainPage.Current.SeAtualizar(Symbol.Add, "Comprador");
            }
            else
            {
                Comprador = (Comprador)e.Parameter;
                MainPage.Current.SeAtualizar(Symbol.Edit, "Comprador");
            }
            DataContext = Comprador;
        }

        private void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (new ValidadorComprador(Comprador).Validar(Log))
                {
                    using (var db = new AplicativoContext())
                    {
                        Comprador.UltimaData = Propriedades.DateTimeNow;
                        if (Comprador.Id == Guid.Empty)
                        {
                            db.Add(Comprador);
                            Log.Escrever(TitulosComuns.Sucesso, "Vendedor salvo com sucesso.");
                        }
                        else
                        {
                            db.Update(Comprador);
                            Log.Escrever(TitulosComuns.Sucesso, "Vendedor alterado com sucesso.");
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
    }
}
