using NFeFacil.ItensBD;
using NFeFacil.Log;
using NFeFacil.ModeloXML.PartesProcesso;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
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
        private MainPage Main;
        private Telas TelaAtual;
        private ILog Log;

        public IntercambioTelas(MainPage main)
        {
            Main = main;
            TelaAtual = Telas.Início;
            Log = new Saida();
        }

        public async Task AbrirFunçaoAsync(Type classe, object parametro = null)
        {
            await AbrirAsync(classe, parametro);
        }

        public async Task AbrirFunçaoAsync(string tela, object parametro = null)
        {
            await AbrirAsync(Type.GetType($"NFeFacil.View.{tela}"), parametro);
        }

        private async Task AbrirAsync(Type tela, object parametro)
        {
            if (TelasComParametroObrigatorio.ContainsKey(tela) && parametro == null)
                TelasComParametroObrigatorio.TryGetValue(tela, out parametro);
            if (Main.FramePrincipal.Content != null)
            {
                if (Main.FramePrincipal.Content is IEsconde esconde)
                {
                    await esconde.EsconderAsync();
                }
                else
                {
                    ILog log = new Saida();
                    log.Escrever(TitulosComuns.ErroSimples, $"A tela {Main.FramePrincipal.Content} ainda precisa implementar IEsconde!");
                }
            }
            Main.FramePrincipal.Navigate(tela, parametro);
        }

        private Dictionary<Type, object> TelasComParametroObrigatorio = new Dictionary<Type, object>
        {
            { typeof(AdicionarDestinatario), new GrupoViewBanco<ClienteDI>() },
            { typeof(AdicionarEmitente), new GrupoViewBanco<EmitenteDI>() },
            { typeof(AdicionarMotorista), new GrupoViewBanco<MotoristaDI>() },
            { typeof(AdicionarProduto), new GrupoViewBanco<ProdutoDI>() },
            {
                typeof(ManipulacaoNotaFiscal),
                new NotaComDados
                {
                    dados = new NFeDI
                    {
                        Status = (int)StatusNFe.EdiçãoCriação
                    },
                    nota = new NFe
                    {
                       Informações = new Detalhes
                        {
                            identificação= new Identificacao(),
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
                    }
                }
            }
        };

        public void SeAtualizar(Telas atual, Symbol símbolo, string texto)
        {
            TelaAtual = atual;
            Main.IndexHamburguer = (int)atual;
            Main.Símbolo = símbolo;
            Main.Título = texto;
        }

        public virtual void RetornoEvento(object sender, BackRequestedEventArgs e)
        {
            if (TelaAtual != Telas.Início)
            {
                e.Handled = true;
                Retornar();
            }
            else
            {
                Log.Escrever(TitulosComuns.Log, "O usuário já está no início.");
            }
        }

        public async void Retornar()
        {
            var frm = Main.FramePrincipal;
            if (frm.Content is IValida)
            {
                var retorna = frm.Content as IValida;
                if (await retorna.Verificar())
                {
                    if (frm.Content is IEsconde)
                    {
                        var esconde = frm.Content as IEsconde;
                        await esconde.EsconderAsync();
                    }
                }
                else
                {
                    return;
                }
            }
            else if (frm.Content is IEsconde)
            {
                var esconde = frm.Content as IEsconde;
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
    }

    public enum Telas
    {
        Início,
        Consulta,
        GerenciarDadosBase,
        ManipularNota,
        NotasEmitidas,
        VendasAnuais,
        Configurações
    }
}
