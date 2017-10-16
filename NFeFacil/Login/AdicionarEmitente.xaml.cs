using NFeFacil.Log;
using NFeFacil.Validacao;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using NFeFacil.ItensBD;
using NFeFacil.IBGE;
using System.Collections.ObjectModel;
using System.Linq;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Login
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class AdicionarEmitente : Page
    {
        EmitenteDI Emit { get; set; }

        string EstadoSelecionado
        {
            get => Emit.SiglaUF;
            set
            {
                Emit.SiglaUF = value;
                ListaMunicipios.Clear();
                foreach (var item in Municipios.Get(value))
                {
                    ListaMunicipios.Add(item);
                }
            }
        }

        ObservableCollection<Municipio> ListaMunicipios { get; } = new ObservableCollection<Municipio>();
        Municipio ConjuntoMunicipio
        {
            get => Municipios.Get(Emit.SiglaUF).FirstOrDefault(x => x.Codigo == Emit.CodigoMunicipio);
            set
            {
                Emit.NomeMunicipio = value?.Nome;
                Emit.CodigoMunicipio = value?.Codigo ?? 0;
            }
        }

        ILog Log = Popup.Current;

        public AdicionarEmitente()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter == null)
            {
                Emit = new EmitenteDI();
                MainPage.Current.SeAtualizar(Symbol.Add, "Emitente");
            }
            else
            {
                Emit = (EmitenteDI)e.Parameter;
                MainPage.Current.SeAtualizar(Symbol.Edit, "Emitente");
            }
            DefinirImagem();

            async void DefinirImagem()
            {
                if (Emit.Id != null)
                {
                    using (var db = new AplicativoContext())
                    {
                        var img = db.Imagens.Find(Emit.Id);
                        if (img != null && img.Bytes != null)
                        {
                            imagem.Source = await img.GetSourceAsync();
                        }
                    }
                }
            }
        }

        private void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (new ValidadorEmitente(Emit).Validar(Log))
                {
                    using (var db = new AplicativoContext())
                    {
                        Emit.UltimaData = DateTime.Now;
                        if (Emit.Id == Guid.Empty)
                        {
                            db.Add(Emit);
                            Log.Escrever(TitulosComuns.Sucesso, "Emitente salvo com sucesso.");
                        }
                        else
                        {
                            db.Update(Emit);
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

        private async void ImportarLogotipo_Click(object sender, RoutedEventArgs e)
        {
            var open = new Windows.Storage.Pickers.FileOpenPicker();
            open.FileTypeFilter.Add(".jpg");
            open.FileTypeFilter.Add(".jpeg");
            open.FileTypeFilter.Add(".png");
            var arq = await open.PickSingleFileAsync();

            var img = new Imagem();
            await img.FromStorageFileAsync(arq);

            if (Emit.Id != default(Guid))
            {
                using (var db = new AplicativoContext())
                {
                    img.Id = Emit.Id;
                    if (db.Imagens.Find(Emit.Id) != null)
                    {
                        // Imagem cadastrada
                        db.Imagens.Update(img);
                    }
                    else
                    {
                        // Imagem inexistente
                        db.Imagens.Add(img);
                    }
                    db.SaveChanges();
                }
            }
            else
            {
                Log.Escrever(TitulosComuns.Erro, "O emitente precisa primeiro estar cadastrado para ser cadastrado o logotipo. Após salvar ele, edite-o e refaça a importação de logotipo.");
            }

            imagem.Source = await img.GetSourceAsync();
        }

        private void ApagarLogotipo_Click(object sender, RoutedEventArgs e)
        {
            if (Emit.Id != default(Guid))
            {
                using (var db = new AplicativoContext())
                {
                    var img = db.Imagens.Find(Emit.Id);
                    if (img != null)
                    {
                        img.Bytes = null;
                        db.Imagens.Update(img);
                        db.SaveChanges();
                    }
                }
                imagem.Source = null;
            }
            else
            {
                Log.Escrever(TitulosComuns.Erro, "Não há como apagar o que nunca foi registrado.");
            }
        }
    }
}
