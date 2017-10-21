using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NFeFacil.ItensBD;
using NFeFacil.Log;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class Backup : Page
    {
        public Backup()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MainPage.Current.SeAtualizar("\uEA35", "Backup");
        }

        async void SalvarBackup(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var objeto = new ConjuntoBanco();
            objeto.AtualizarPadrao();
            var json = JsonConvert.SerializeObject(objeto);
            await Zipar(json);

            var caixa = new FileSavePicker();
            caixa.FileTypeChoices.Add("Arquivo comprimido", new string[] { ".zip" });
            var novo = await caixa.PickSaveFileAsync();
            if (novo != null)
            {
                var original = await RetornarArquivo();
                await original.CopyAndReplaceAsync(novo);
                await ExcluirArquivo();
            }
        }

        async void RestaurarBackup(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var caixa = new FileOpenPicker();
            caixa.FileTypeFilter.Add(".zip");
            var arq = await caixa.PickSingleFileAsync();
            if (arq != null)
            {
                var pasta = ApplicationData.Current.TemporaryFolder;
                pasta = await pasta.CreateFolderAsync("Temp", CreationCollisionOption.ReplaceExisting);
                arq = await arq.CopyAsync(pasta, "Backup.zip", NameCollisionOption.ReplaceExisting);
                var aberto = await Task.Run(() => ZipFile.Open(arq.Path, ZipArchiveMode.Read));
                pasta = ApplicationData.Current.TemporaryFolder;
                foreach (var item in await pasta.GetFilesAsync())
                {
                    await item.DeleteAsync();
                }
                aberto.ExtractToDirectory(pasta.Path);
                arq = await pasta.GetFileAsync("Backup.json");
                if (arq != null)
                {
                    using (var leitor = new StreamReader(await arq.OpenStreamForReadAsync()))
                    {
                        try
                        {
                            var texto = await leitor.ReadToEndAsync();
                            var conjunto = JsonConvert.DeserializeObject<ConjuntoBanco>(texto);
                            try
                            {
                                conjunto.AnalisarESalvar();
                                Popup.Current.Escrever(TitulosComuns.Sucesso, "Backup restaurado com sucesso.");
                            }
                            catch (Exception)
                            {
                                Popup.Current.Escrever(TitulosComuns.Erro, "Erro ao salvar os itens do backup, aparentemente já existem dados cadastrados neste aplicativo.");
                            }
                        }
                        catch (Exception)
                        {
                            Popup.Current.Escrever(TitulosComuns.Erro, "Este não é um arquivo de backup válido.");
                        }
                    }
                }
            }
        }

        const string Nome = "Backup";

        static async Task Zipar(string informacao)
        {
            var pasta = ApplicationData.Current.TemporaryFolder;
            pasta = await pasta.CreateFolderAsync("Temp", CreationCollisionOption.ReplaceExisting);
            var arquivo = await pasta.CreateFileAsync($"{Nome}.json");
            var stream = await arquivo.OpenStreamForWriteAsync();
            using (StreamWriter escritor = new StreamWriter(stream))
            {
                await escritor.WriteAsync(informacao);
                escritor.Flush();
            }

            pasta = ApplicationData.Current.TemporaryFolder;
            await Task.Run(() => ZipFile.CreateFromDirectory(pasta.Path + @"\Temp", $@"{pasta.Path}\Backup.zip", CompressionLevel.Optimal, false));
        }

        static async Task<StorageFile> RetornarArquivo()
        {
            var pasta = ApplicationData.Current.TemporaryFolder;
            return await pasta.GetFileAsync(Nome + ".zip");
        }

        static async Task ExcluirArquivo()
        {
            var arquivo = await RetornarArquivo();
            await arquivo.DeleteAsync(StorageDeleteOption.PermanentDelete).AsTask();
        }

        struct ConjuntoBanco
        {
            public ClienteDI[] Clientes { get; set; }
            public EmitenteDI[] Emitentes { get; set; }
            public MotoristaDI[] Motoristas { get; set; }
            public Vendedor[] Vendedores { get; set; }
            public ProdutoDI[] Produtos { get; set; }
            public Estoque[] Estoque { get; set; }
            public VeiculoDI[] Veiculos { get; set; }
            public NFeDI[] NotasFiscais { get; set; }
            public RegistroVenda[] Vendas { get; set; }
            public RegistroCancelamento[] Cancelamentos { get; set; }
            public CancelamentoRegistroVenda[] CancelamentosRegistroVenda { get; set; }
            public Imagem[] Imagens { get; set; }

            public void AtualizarPadrao()
            {
                using (var db = new AplicativoContext())
                {
                    Clientes = db.Clientes.ToArray();
                    Emitentes = db.Emitentes.ToArray();
                    Motoristas = db.Motoristas.ToArray();
                    Vendedores = db.Vendedores.ToArray();
                    Produtos = db.Produtos.ToArray();
                    Estoque = db.Estoque.Include(x => x.Alteracoes).ToArray();
                    Veiculos = db.Veiculos.ToArray();
                    NotasFiscais = db.NotasFiscais.ToArray();
                    Vendas = db.Vendas.Include(x => x.Produtos).ToArray();
                    Cancelamentos = db.Cancelamentos.ToArray();
                    CancelamentosRegistroVenda = db.CancelamentosRegistroVenda.ToArray();
                    Imagens = db.Imagens.ToArray();
                }
            }

            public void AnalisarESalvar()
            {
                using (var db = new AplicativoContext())
                {
                    db.AddRange(Clientes);
                    db.AddRange(Emitentes);
                    db.AddRange(Motoristas);
                    db.AddRange(Vendedores);
                    db.AddRange(Produtos);
                    db.AddRange(Estoque);
                    db.AddRange(Veiculos);
                    db.AddRange(NotasFiscais);
                    db.AddRange(Vendas);
                    db.AddRange(Cancelamentos);
                    db.AddRange(CancelamentosRegistroVenda);
                    db.AddRange(Imagens);
                    db.SaveChanges();
                }
            }
        }
    }
}
