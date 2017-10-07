﻿using NFeFacil.Log;
using NFeFacil.Validacao;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using NFeFacil.ItensBD;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Login
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class AdicionarEmitente : Page
    {
        private EmitenteDI emitente;
        private ILog Log = Popup.Current;

        public AdicionarEmitente()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter == null)
            {
                emitente = new EmitenteDI();
                MainPage.Current.SeAtualizar(Symbol.Add, "Emitente");
            }
            else
            {
                emitente = (EmitenteDI)e.Parameter;
                MainPage.Current.SeAtualizar(Symbol.Edit, "Emitente");
            }
            DefinirImagem();
            DataContext = new EmitenteDataContext(ref emitente);

            async void DefinirImagem()
            {
                if (emitente.Id != null)
                {
                    using (var db = new AplicativoContext())
                    {
                        var img = db.Imagens.Find(emitente.Id);
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
                if (new ValidadorEmitente(emitente).Validar(Log))
                {
                    using (var db = new AplicativoContext())
                    {
                        emitente.UltimaData = DateTime.Now;
                        if (emitente.Id == Guid.Empty)
                        {
                            db.Add(emitente);
                            Log.Escrever(TitulosComuns.Sucesso, "Emitente salvo com sucesso.");
                        }
                        else
                        {
                            db.Update(emitente);
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

            if (emitente.Id != default(Guid))
            {
                using (var db = new AplicativoContext())
                {
                    img.Id = emitente.Id;
                    if (db.Imagens.Find(emitente.Id) != null)
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
            if (emitente.Id != default(Guid))
            {
                using (var db = new AplicativoContext())
                {
                    var img = db.Imagens.Find(emitente.Id);
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