using NFeFacil.DANFE.Modelos.Global;
using NFeFacil.DANFE.Modelos.Local;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace NFeFacil.DANFE.Processamento
{
    public sealed class ViewDados
    {
        private ConexãoDados conec;
        public int TotalPaginas { get; private set; }
        private DadosPrimeiraPagina dadosPrimeiraPagina;
        private List<DadosOutrasPaginas> dadosOutrasPaginas = new List<DadosOutrasPaginas>();

        public ViewDados(ref WebView view, Geral Danfe)
        {
            conec = new ConexãoDados(ref view);
            AdicionarFuncoesPaginacao(Danfe);
        }

        void AdicionarFuncoesPaginacao(Geral dadosDanfe)
        {
            if (dadosDanfe._DadosProdutos.Length == 0)
                throw new ArgumentNullException(nameof(dadosDanfe._DadosProdutos));
            
            var nProdutosPagina1 = 18 - (dadosDanfe._Duplicatas == null ? 0 : dadosDanfe._Duplicatas.Length);
            var nProdutosPaginaRestante = 25;

            if (dadosDanfe._DadosProdutos.Length <= nProdutosPagina1)
            {
                TotalPaginas = 1;
            }
            else if (dadosDanfe._DadosProdutos.Length <= nProdutosPagina1 + nProdutosPaginaRestante)
            {
                TotalPaginas = 2;
            }
            else
            {
                double quantRestante = dadosDanfe._DadosProdutos.Length - (nProdutosPagina1 + nProdutosPaginaRestante);
                var nExtraQuebrado = quantRestante / nProdutosPaginaRestante;
                var resto = quantRestante % nProdutosPaginaRestante;
                TotalPaginas = (int)nExtraQuebrado + 2;
                if (resto > 0) TotalPaginas++;
            }

            var nItensPorPagina = new List<int>();
            if (TotalPaginas == 1)
            {
                nItensPorPagina.Add(nProdutosPagina1);
            }
            else if (TotalPaginas == 2)
            {
                nItensPorPagina.Add(nProdutosPagina1);
                nItensPorPagina.Add(nProdutosPaginaRestante);
            }
            else
            {
                nItensPorPagina.Add(nProdutosPagina1);
                for (int i = 1; i < TotalPaginas; i++)
                {
                    nItensPorPagina.Add(nProdutosPaginaRestante);
                }
            }

            var paginas = new Dictionary<int, List<DadosProduto>>();
            for (int i = 0; i < TotalPaginas; i++) paginas.Add(i, new List<DadosProduto>());

            var pagina = 0;
            foreach (var Produto in dadosDanfe._DadosProdutos)
            {
                if (paginas[pagina].Count == nItensPorPagina[pagina]) pagina++;
                paginas[pagina].Add(Produto);
            }

            for (int i = 0; i < paginas.Keys.Count; i++)
            {
                dadosDanfe._DadosNFe.DefinirPagina(TotalPaginas, i + 1);

                if (i == 0)
                {
                    dadosPrimeiraPagina = new DadosPrimeiraPagina
                    {
                        cabec = dadosDanfe._DadosCabecalho,
                        nfe = dadosDanfe._DadosNFe,
                        cliente = dadosDanfe._DadosCliente,
                        motorista = dadosDanfe._DadosMotorista,
                        imposto = dadosDanfe._DadosImposto,
                        Produto = paginas[i],
                        extras = dadosDanfe._DadosAdicionais,
                        paginaTotal = paginas.Keys.Count,
                        duplicatas = dadosDanfe._Duplicatas
                    };
                }
                else
                {
                    dadosOutrasPaginas.Add(new DadosOutrasPaginas
                    {
                        nfe = dadosDanfe._DadosNFe,
                        cliente = dadosDanfe._DadosCliente,
                        extras = dadosDanfe._DadosAdicionais,
                        paginaAtual = i + 1,
                        paginaTotal = paginas.Keys.Count,
                        Produto = paginas[i]
                    });
                }
            }
        }

        public async Task ExibirTodasAsPáginas()
        {
            await conec.AddPrimeiraPage(dadosPrimeiraPagina);
            foreach (var item in dadosOutrasPaginas)
            {
                await conec.AddOutraPage(item);
            }
        }

        public async Task ExibirUmaPágina(int index)
        {
            await conec.ApagarCorpo();
            if (index == 0) await conec.AddPrimeiraPage(dadosPrimeiraPagina);
            else await conec.AddOutraPage(dadosOutrasPaginas[index - 1]);
        }
    }
}
