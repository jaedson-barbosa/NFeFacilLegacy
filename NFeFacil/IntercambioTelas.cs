using BibliotecaCentral.ItensBD;
using BibliotecaCentral.Log;
using BibliotecaCentral.ModeloXML.PartesProcesso;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte;
using NFeFacil.View;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace NFeFacil
{
    internal class IntercambioTelas
    {
        private MainPage Main => MainPage.Current;
        private ILog Log = new Saida();

        public async Task AbrirFunçaoAsync(string tela, object parametro = null)
        {
            await AbrirFunçaoAsync(Type.GetType($"NFeFacil.View.{tela}"), parametro);
        }

        public async Task AbrirFunçaoAsync(Type tela, object parametro = null)
        {
            if (TelasComParametroObrigatorio.Contains(tela) && parametro == null)
            {
                ObterValorPadrao(tela, out parametro);
            }

            if (Main.FramePrincipal.Content != null)
            {
                if (Main.FramePrincipal.Content is IEsconde esconde)
                {
                    await esconde.EsconderAsync();
                }
                else
                {
                    Log.Escrever(TitulosComuns.ErroSimples, $"A tela {Main.FramePrincipal.Content} ainda precisa implementar IEsconde!");
                }
            }
            Main.FramePrincipal.Navigate(tela, parametro);
        }

        private List<Type> TelasComParametroObrigatorio = new List<Type>
        {
            typeof(AdicionarDestinatario),
            typeof(AdicionarEmitente),
            typeof(AdicionarMotorista),
            typeof(AdicionarProduto),
            typeof(ManipulacaoNotaFiscal)
        };

        private void ObterValorPadrao(Type tipo, out object retorno)
        {
            if (tipo == typeof(AdicionarDestinatario))
            {
                retorno = new GrupoViewBanco<Destinatario>();
            }
            else if (tipo == typeof(AdicionarEmitente))
            {
                retorno = new GrupoViewBanco<Emitente>();
            }
            else if (tipo == typeof(AdicionarMotorista))
            {
                retorno = new GrupoViewBanco<Motorista>();
            }
            else if (tipo == typeof(AdicionarProduto))
            {
                retorno = new GrupoViewBanco<BaseProdutoOuServico>();
            }
            else if (tipo == typeof(ManipulacaoNotaFiscal))
            {
                retorno = new ConjuntoManipuladorNFe
                {
                    NotaSalva = new NFe()
                    {
                        Informações = new Detalhes()
                        {
                            identificação = new Identificacao(),
                            emitente = new Emitente(),
                            destinatário = new Destinatario(),
                            produtos = new List<DetalhesProdutos>(),
                            transp = new Transporte(),
                            cobr = new Cobranca(),
                            infAdic = new InformacoesAdicionais(),
                            exporta = new Exportacao(),
                            compra = new Compra(),
                            cana = new RegistroAquisicaoCana()
                        }
                    },
                    OperacaoRequirida = TipoOperacao.Adicao,
                    StatusAtual = StatusNFe.Edição
                };
            }
            else
            {
                retorno = null;
                Log.Escrever(TitulosComuns.ErroSimples, "Valor padrão desse tipo não cadastrado");
            }
        }

        public void SeAtualizar(Telas atual, Symbol símbolo, string texto)
        {
            Main.Titulo = texto;
            Main.AvisoOrentacaoHabilitado = TelasHorizontais.Contains(atual);
            Main.Icone = new SymbolIcon(símbolo);
        }

        public void SeAtualizar(Telas atual, string glyph, string texto)
        {
            Main.Titulo = texto;
            Main.AvisoOrentacaoHabilitado = TelasHorizontais.Contains(atual);
            Main.Icone = new FontIcon
            {
                Glyph = glyph,
            };
        }

        public void RetornoEvento(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            Retornar();
        }

        public async void Retornar()
        {
            var frm = Main.FramePrincipal;
            if (frm.Content is IValida retorna)
            {
                if (await retorna.Verificar())
                {
                    if (frm.Content is IEsconde esconde)
                    {
                        await esconde.EsconderAsync();
                    }
                }
                else
                {
                    return;
                }
            }
            else if (frm.Content is IEsconde esconde)
            {
                await esconde.EsconderAsync();
            }

            if (Main.FramePrincipal.CanGoBack)
            {
                Main.FramePrincipal.GoBack();
            }
            else
            {
                Log.Escrever(TitulosComuns.ErroSimples, "Não é possível voltar para a tela anterior.");
            }
        }

        private List<Telas> TelasHorizontais = new List<Telas>(3)
        {
            Telas.GerenciarDadosBase,
            Telas.HistoricoSincronizacao,
            Telas.NotasSalvas
        };
    }
}
