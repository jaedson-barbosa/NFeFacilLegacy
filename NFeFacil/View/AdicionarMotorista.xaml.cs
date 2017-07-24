using NFeFacil.Log;
using NFeFacil.Validacao;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using NFeFacil.ItensBD;
using NFeFacil.ViewModel;
using NFeFacil;
using System;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class AdicionarMotorista : Page
    {
        private MotoristaDI motorista;
        private TipoOperacao tipoRequisitado;
        private ILog Log = Popup.Current;

        public AdicionarMotorista()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            GrupoViewBanco<MotoristaDI> parametro;
            if (e.Parameter == null)
            {
                parametro = new GrupoViewBanco<MotoristaDI>
                {
                    ItemBanco = new MotoristaDI(),
                    OperacaoRequirida = TipoOperacao.Adicao
                };
            }
            else
            {
                parametro = (GrupoViewBanco<MotoristaDI>)e.Parameter;
            }
            motorista = parametro.ItemBanco;
            tipoRequisitado = parametro.OperacaoRequirida;
            switch (tipoRequisitado)
            {
                case TipoOperacao.Adicao:
                    MainPage.Current.SeAtualizar(Symbol.Add, "Motorista");
                    break;
                case TipoOperacao.Edicao:
                    MainPage.Current.SeAtualizar(Symbol.Edit, "Motorista");
                    break;
            }
            DataContext = new MotoristaDataContext(ref motorista);
        }

        private void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (new ValidadorMotorista(motorista).Validar(Log))
                {
                    using (var db = new AplicativoContext())
                    {
                        motorista.UltimaData = DateTime.Now;
                        if (tipoRequisitado == TipoOperacao.Adicao)
                        {
                            db.Add(motorista);
                            Log.Escrever(TitulosComuns.Sucesso, "Motorista salvo com sucesso.");
                        }
                        else
                        {
                            db.Update(motorista);
                            Log.Escrever(TitulosComuns.Sucesso, "Motorista alterado com sucesso.");
                        }
                        db.SaveChanges();
                    }
                    MainPage.Current.Retornar();
                }
            }
            catch (System.Exception erro)
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
