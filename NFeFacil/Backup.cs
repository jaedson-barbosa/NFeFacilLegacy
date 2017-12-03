using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NFeFacil.ItensBD;
using NFeFacil.Log;
using System;
using System.IO;
using System.Linq;
using Windows.Storage.Pickers;

namespace NFeFacil
{
    public static class Backup
    {
        public async static void SalvarBackup()
        {
            var objeto = new ConjuntoBanco();
            objeto.AtualizarPadrao();
            var json = JsonConvert.SerializeObject(objeto);

            var caixa = new FileSavePicker();
            caixa.FileTypeChoices.Add("Arquivo JSON", new string[] { ".json" });
            var arq = await caixa.PickSaveFileAsync();
            if (arq != null)
            {
                var stream = await arq.OpenStreamForWriteAsync();
                using (StreamWriter escritor = new StreamWriter(stream))
                {
                    await escritor.WriteAsync(json);
                    await escritor.FlushAsync();
                }
            }
        }

        public async static void RestaurarBackup()
        {
            var caixa = new FileOpenPicker();
            caixa.FileTypeFilter.Add(".json");
            var arq = await caixa.PickSingleFileAsync();
            if (arq != null)
            {
                var stream = await arq.OpenStreamForReadAsync();
                using (var leitor = new StreamReader(stream))
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
