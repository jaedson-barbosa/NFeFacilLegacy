using NFeFacil.ItensBD;
using NFeFacil.Log;
using NFeFacil.View;
using NFeFacil.ViewModel.NotaFiscal;
using System;
using System.Collections.Generic;
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

        public void AbrirFunçao(Type classe, object parametro = null) => Abrir(classe, parametro);

        public void AbrirFunçao(string tela, object parametro = null)
        {
            var classe = Type.GetType($"NFeFacil.View.{tela}");
            if (classe == null)
            {
                new Saida().Escrever(TitulosComuns.ErroSimples, $"Tela ainda nao cadastrada. Nome: {tela}");
                return;
            }
            Abrir(classe, parametro);
        }

        private void Abrir(Type tela, object parametro)
        {
            if (tela == null) throw new ArgumentNullException(nameof(tela));
            if (TelasComParametroObrigatorio.ContainsKey(tela) && parametro == null)
                TelasComParametroObrigatorio.TryGetValue(tela, out parametro);
            Main.FramePrincipal.Navigate(tela, parametro);
        }

        private Dictionary<Type, object> TelasComParametroObrigatorio = new Dictionary<Type, object>
        {
            { typeof(AdicionarDestinatario), new ClienteDataContext() },
            { typeof(AdicionarEmitente), new EmitenteDataContext() },
            { typeof(AdicionarMotorista), new MotoristaDataContext() },
            { typeof(AdicionarProduto), new ProdutoDI() }
            //{
            //    typeof(TelaNotaFiscal),
            //    new NotaComDados
            //    {
            //        dados = new NFeDataItem
            //        {
            //            Status = (int)StatusNFe.EdiçãoCriação
            //        },
            //        nota = new NFe
            //        {
            //            informações = new Detalhes
            //            {
            //                identificação= new Identificacao(),
            //                emitente = new Emitente(),
            //                destinatário = new Destinatário(),
            //                produtos = new List<DetalhesProdutos>(),
            //                transp = new Transporte(),
            //                cobr = new Cobrança(),
            //                infAdic = new InformaçõesAdicionais(),
            //                exporta = new Exportação(),
            //                compra = new Compra(),
            //                cana = new RegistroAquisiçãoCana()
            //            }
            //        }
            //    }
            //}
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
                Main.FramePrincipal.GoBack();
            else
                Log.Escrever(TitulosComuns.ErroSimples, "Não é possível voltar para a tela anterior.");
        }
    }

    public enum Telas
    {
        Início,
        Consulta,
        GerenciarDadosBase,
        EmitirNovaNota,
        NotasEmitidas,
        AnaliseVendasAnuais,
        Configurações
    }

}
