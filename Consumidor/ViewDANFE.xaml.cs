using BaseGeral.ModeloXML;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Navigation;
using OptimizedZXing;
using static BaseGeral.ExtensoesPrincipal;
using BaseGeral.View;
using NFeFacil;
using Venda;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Consumidor
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    [DetalhePagina(Symbol.View, "DANFE")]
    public sealed partial class ViewDANFE : Page
    {
        double Largura { get; set; }
        Thickness Margem { get; set; }
        Thickness MargemQR { get; set; }
        bool Estreito => Largura <= 500;
        bool Largo => Largura > 500;

        ProcessoNFCe Processo { get; set; }
        NFCe NFCe => Processo.NFe;
        ProdutoDANFE[] Produtos { get; set; }
        string QuantidadeProdutos => Produtos.Length.ToString();
        string[] FormasPagamento { get; set; }
        string[] ValoresPagamento { get; set; }
        string TotalBruto { get; set; }
        string TotalLiquido { get; set; }
        string Troco { get; set; }

        string UriConsultaChave { get; set; }
        string ChaveAcesso { get; set; }

        string Numero { get; set; }
        string Serie { get; set; }
        string Data { get; set; }
        string Hora { get; set; }

        string DataHoraAutorizacao { get; set; }
        Visibility InformacoesFisco { get; set; }
        Visibility InformacoesContr { get; set; }

        GridLength Largura1 { get; } = CMToLength(1);
        GridLength LarguraReduzida { get; } = CMToLength(0.5);

        GerenciadorImpressao gerenciadorImpressão;

        public ViewDANFE()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var dados = (DadosImpressao)e.Parameter;
            Processo = dados;
            Largura = dados;
            Margem = dados;

            var totLiquido = ProcessarProdutos();
            ProcessarFormasPagamentoETroco(totLiquido);
            ProcessarConsultaChave();
            ProcessarQR();
            ProcessarConsumidor();
            ProcessarIdentificacao();
            ProcessarInformacoesAdicionais();

            gerenciadorImpressão = new GerenciadorImpressao();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) => gerenciadorImpressão.Dispose();

        private async void Imprimir(object sender, RoutedEventArgs e)
        {
            var paginas = new UIElement[1]
            {
                pagina
            };
            await gerenciadorImpressão.Imprimir(paginas);
        }

        double ProcessarProdutos()
        {
            var prods = NFCe.Informacoes.produtos;
            Produtos = new ProdutoDANFE[prods.Count];
            double totalBruto = 0, acrescimos = 0, desconto = 0;
            for (int i = 0; i < prods.Count; i++)
            {
                var at = prods[i].Produto;
                var totItem = at.ValorTotal;
                totalBruto += totItem;
                acrescimos += TryParse(at.Frete) + TryParse(at.Seguro) + TryParse(at.DespesasAcessorias);
                desconto += TryParse(at.Desconto);
                Produtos[i] = new ProdutoDANFE(at.CodigoProduto, at.Descricao,
                    at.QuantidadeComercializada, at.UnidadeComercializacao,
                    at.ValorUnitario, totItem);
            }
            TotalBruto = totalBruto.ToString("N2");
            var totLiquido = totalBruto + acrescimos - desconto;
            TotalLiquido = totLiquido.ToString("N2");
            const string strAcrescimos = "Acréscimos (frete, seguro e outras despesas) R$";
            const string strDesconto = "Desconto R$";
            if (acrescimos != 0 && desconto != 0)
            {
                AddDetalheTotal(strDesconto, desconto.ToString("N2"));
                AddDetalheTotal(strAcrescimos, acrescimos.ToString("N2"));
            }
            else if (acrescimos != 0)
            {
                AddDetalheTotal(strAcrescimos, acrescimos.ToString("N2"));
            }
            else if (desconto != 0)
            {
                AddDetalheTotal(strDesconto, desconto.ToString("N2"));
            }
            return totLiquido;
            void AddDetalheTotal(string esq, string dir)
            {
                txtEsq.Inlines.Insert(4, new Run() { Text = esq });
                txtDir.Inlines.Insert(4, new Run() { Text = dir });
            }
        }

        void ProcessarFormasPagamentoETroco(double totLiquido)
        {
            Dictionary<string, string> descCodigo = new Dictionary<string, string>
            {
                { "01", "Dinheiro" },
                { "02", "Cheque" },
                { "03", "Cartão de Crédito" },
                { "04", "Cartão de Débito" },
                { "05", "Crédito Loja" },
                { "10", "Vale Alimentação" },
                { "11", "Vale Refeição" },
                { "12", "Vale Presente" },
                { "13", "Vale Combustível" },
                { "99", "Outros" }
            };

            double totPago = 0;
            var formas = NFCe.Informacoes.FormasPagamento;
            var nForma = formas.Count;
            FormasPagamento = new string[nForma];
            ValoresPagamento = new string[nForma];
            for (int i = 0; i < nForma; i++)
            {
                FormasPagamento[i] = descCodigo[formas[i].Forma];
                var vPag = formas[i].vPag;
                totPago += vPag;
                ValoresPagamento[i] = vPag.ToString("N2");
            }
            Troco = (totPago - totLiquido).ToString("N2");
        }

        void ProcessarConsultaChave()
        {
            bool homologacao = NFCe.AmbienteTestes;
            var urls = homologacao ? UrlsQR.Homologacao : UrlsQR.Producao;
            UriConsultaChave = urls[NFCe.Informacoes.Emitente.Endereco.SiglaUF];
            ChaveAcesso = AplicarMascaraChave(NFCe.Informacoes.ChaveAcesso);

            string AplicarMascaraChave(string original)
            {
                var novaChave = "";
                for (var i = 0; i < 44; i += 4)
                {
                    novaChave += original.Substring(i, 4) + " ";
                }
                return novaChave;
            }
        }

        void ProcessarQR()
        {
            var dim = 130;
            var writer = new BarcodeWriter(BarcodeFormat.QR_CODE)
            {
                Options = new EncodingOptions
                {
                    Width = dim,
                    Height = dim,
                    Margin = 0
                }
            };
            var encoded = writer.Encode(NFCe.InfoSuplementares.Uri);
            MargemQR = new Thickness(13);
            foreach (var item in writer.WriteToUI(encoded))
            {
                imgQR.Children.Add(item);
            }
        }

        void ProcessarConsumidor()
        {
            var dest = NFCe.Informacoes.destinatário;
            var end = dest?.Endereco;
            if (dest == null)
            {
                AddDetalhe("CONSUMIDOR NÃO IDENTIFICADO", true);
            }
            else if (!string.IsNullOrEmpty(dest.CPF))
            {
                AddDetalhe($"CONSUMIDOR - CPF {AplicarMáscaraDocumento(dest.CPF)}", true);
                AddDetalhe($" - {dest.Nome} - {end.Logradouro}, {end.Numero}, {end.Bairro}, {end.NomeMunicipio} - {end.SiglaUF}", false);
            }
            else if (!string.IsNullOrEmpty(dest.CNPJ))
            {
                AddDetalhe($"CONSUMIDOR - CNPJ {AplicarMáscaraDocumento(dest.CPF)}", true);
                AddDetalhe($" - {dest.Nome} - {end.Logradouro}, {end.Numero}, {end.Bairro}, {end.NomeMunicipio} - {end.SiglaUF}", false);
            }
            else if (!string.IsNullOrEmpty(dest.IdEstrangeiro))
            {
                AddDetalhe($"CONSUMIDOR - Id. Estrangeiro {AplicarMáscaraDocumento(dest.CPF)}", true);
            }

            void AddDetalhe(string txt, bool negrito)
            {
                if (negrito)
                {
                    var bold = new Bold();
                    bold.Inlines.Add(new Run() { Text = txt });
                    txtConsumidor.Inlines.Add(bold);
                }
                else
                {
                    txtConsumidor.Inlines.Add(new Run() { Text = txt });
                }
            }
        }

        void ProcessarIdentificacao()
        {
            var ident = NFCe.Informacoes.identificacao;
            Numero = ident.Numero.ToString("000000000");
            Serie = ident.Serie.ToString("000");
            var dataHora = DateTime.Parse(ident.DataHoraEmissão);
            Data = dataHora.ToString("dd/MM/yyyy");
            Hora = dataHora.ToString("HH:mm:ss");
            dataHora = DateTime.Parse(Processo.ProtNFe.InfProt.dhRecbto);
            DataHoraAutorizacao = dataHora.ToString("dd/MM/yyyy HH:mm:ss");
        }

        void ProcessarInformacoesAdicionais()
        {
            var info = NFCe.Informacoes.infAdic;
            InformacoesFisco = string.IsNullOrEmpty(info?.InfAdFisco) ? Visibility.Collapsed : Visibility.Visible;
            InformacoesContr = string.IsNullOrEmpty(info?.InfCpl) ? Visibility.Collapsed : Visibility.Visible;
        }
    }

    public sealed class ProdutoDANFE
    {
        public GridLength Largura1 { get; }
        public GridLength LarguraReduzida { get; }

        public ProdutoDANFE(string codigo, string descricao, double quantidade, string unidade, double valorUnitario, double valorTotal)
        {
            Largura1 = CMToLength(1);
            LarguraReduzida = CMToLength(0.5);

            Codigo = codigo;
            Descricao = descricao;
            Quantidade = quantidade.ToString("N2");
            Unidade = unidade;
            ValorUnitario = valorUnitario.ToString("N2");
            ValorTotal = valorTotal.ToString("N2");
        }

        public string Codigo { get; }
        public string Descricao { get; }
        public string Quantidade { get; }
        public string Unidade { get; set; }
        public string ValorUnitario { get; set; }
        public string ValorTotal { get; set; }
    }
}
