using NFeFacil.ItensBD;
using NFeFacil.Log;
using NFeFacil.Validacao;
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
                    MainPage.Current.SeAtualizar(Symbol.Add, "Vendedor");
                    break;
                case TipoOperacao.Edicao:
                    MainPage.Current.SeAtualizar(Symbol.Edit, "Vendedor");
                    break;
            }
            DefinirImagem();
            DataContext = vendedor;

            async void DefinirImagem()
            {
                if (vendedor.Id != null)
                {
                    using (var db = new AplicativoContext())
                    {
                        var img = db.Imagens.Find(vendedor.Id);
                        if (img != null)
                        {
                            imgFoto.Source = await img.GetSourceAsync();
                        }
                    }
                }
            }
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
                            Log.Escrever(TitulosComuns.Sucesso, "Vendedor salvo com sucesso.");
                        }
                        else
                        {
                            db.Update(vendedor);
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

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Current.Retornar();
        }

        private async void EscolherImagem(object sender, RoutedEventArgs e)
        {
            var open = new Windows.Storage.Pickers.FileOpenPicker();
            open.FileTypeFilter.Add(".jpg");
            open.FileTypeFilter.Add(".jpeg");
            open.FileTypeFilter.Add(".png");
            var arq = await open.PickSingleFileAsync();

            var img = new Imagem();
            await img.FromStorageFileAsync(arq);

            if (vendedor.Id != default(Guid))
            {
                using (var db = new AplicativoContext())
                {
                    img.Id = vendedor.Id;
                    if (db.Imagens.Find(vendedor.Id) != null)
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

            imgFoto.Source = await img.GetSourceAsync(); 
        }

        private void RemoverImagem(object sender, RoutedEventArgs e)
        {
            if (vendedor.Id != default(Guid))
            {
                using (var db = new AplicativoContext())
                {
                    var img = db.Imagens.Find(vendedor.Id);
                    if (img != null)
                    {
                        db.Imagens.Remove(img);
                        db.SaveChanges();
                    }
                }
                imgFoto.Source = null;
            }
            else
            {
                Log.Escrever(TitulosComuns.Erro, "Não há como apagar o que nunca foi registrado.");
            }
        }
    }
}
