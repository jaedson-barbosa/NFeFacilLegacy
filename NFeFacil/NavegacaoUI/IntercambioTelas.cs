using NFeFacil.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace NFeFacil.NavegacaoUI
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

        public void AbrirFunçao(Type classe, object parametro) => Abrir(classe, parametro);

        public void AbrirFunçao(string tela, object parametro = null)
        {
            var classe = Type.GetType($"NotaFacil.View.{tela}");
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
            {
                typeof(TelaNotaFiscal),
                new NotaComDados
                {
                    dados = new NFeDataItem
                    {
                        Status = (int)StatusNFe.EdiçãoCriação
                    },
                    nota = new NFe
                    {
                        informações = new Detalhes
                        {
                            identificação= new Identificacao(),
                            emitente = new Emitente(),
                            destinatário = new Destinatário(),
                            produtos = new List<DetalhesProdutos>(),
                            transp = new Transporte(),
                            cobr = new Cobrança(),
                            infAdic = new InformaçõesAdicionais(),
                            exporta = new Exportação(),
                            compra = new Compra(),
                            cana = new RegistroAquisiçãoCana()
                        }
                    }
                }
            }
        };

        public void SeAtualizar(Telas atual, Symbol símbolo, string texto)
        {
            TelaAtual = atual;
            if (atual == Telas.Configurações)
            {
                Main.IndexFunçãoExtra = 0;
                Main.IndexFunçãoPrincipal = -1;
            }
            else
            {
                Main.IndexFunçãoExtra = -1;
                Main.IndexFunçãoPrincipal = (int)atual;
            }
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
            if (frm.Content is IVerificaAntesSair)
            {
                var retorna = frm.Content as IVerificaAntesSair;
                if (await retorna.Verificar())
                {
                    if (frm.Content is IEsconde)
                    {
                        var esconde = frm.Content as IEsconde;
                        await esconde.Esconder();
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
                await esconde.Esconder();
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
        Configurações,
    }

}
