using NFeFacil.Log;
using NFeFacil.Validacao;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System;
using NFeFacil.ItensBD;

namespace NFeFacil.ViewDadosBase
{
    public sealed class BaseAdicaoDestinatario
    {
        internal ClienteDI Cliente { get; private set; }
        internal readonly ILog Log = Popup.Current;

        internal void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter == null)
            {
                Cliente = new ClienteDI();
                MainPage.Current.SeAtualizar(Symbol.Add, "Cliente");
            }
            else
            {
                Cliente = (ClienteDI)e.Parameter;
                MainPage.Current.SeAtualizar(Symbol.Edit, "Cliente");
            }
        }

        internal void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (new ValidadorDestinatario(Cliente).Validar(Log))
                {
                    using (var db = new AplicativoContext())
                    {
                        Cliente.UltimaData = DateTime.Now;
                        if (Cliente.Id == Guid.Empty)
                        {
                            db.Add(Cliente);
                            Log.Escrever(TitulosComuns.Sucesso, "Cliente salvo com sucesso.");
                        }
                        else
                        {
                            db.Update(Cliente);
                            Log.Escrever(TitulosComuns.Sucesso, "Cliente alterado com sucesso.");
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

        internal void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Current.Retornar();
        }
    }
}
