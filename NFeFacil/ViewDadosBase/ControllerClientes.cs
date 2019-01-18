using BaseGeral;
using BaseGeral.Buscador;
using BaseGeral.ItensBD;
using System;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace NFeFacil.ViewDadosBase
{
    public sealed class ControllerClientes : ControllerGerenciadorGeral
    {
        public ControllerClientes() : base("Inativar", DefinicoesPermanentes.ModoBuscaCliente)
        {
            using (var repo = new BaseGeral.Repositorio.Leitura())
            {
                TodosItens = repo.ObterClientes()
                    .Select(x => new ExibicaoEspecifica<ClienteDI>(x, x.Documento, x.NomeMunicipio, x.Nome))
                    .ToArray();
                Itens = TodosItens.GerarObs();
            }
        }

        public override async void Adicionar()
        {
            var caixa = new EscolherTipoCliente();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                switch (caixa.TipoCliente)
                {
                    case 0:
                        MainPage.Current.Navegar<AdicionarClienteBrasileiroPF>();
                        break;
                    case 1:
                        MainPage.Current.Navegar<AdicionarClienteBrasileiroPFContribuinte>();
                        break;
                    case 2:
                        MainPage.Current.Navegar<AdicionarClienteBrasileiroPJ>();
                        break;
                    case 3:
                        MainPage.Current.Navegar<AdicionarClienteEstrangeiro>();
                        break;
                }
            }
        }

        public override void Editar(ExibicaoGenerica contexto)
        {
            var dest = contexto.Convert<ClienteDI>();
            if (!string.IsNullOrEmpty(dest.CPF))
            {
                if (dest.IndicadorIE == 1)
                    MainPage.Current.Navegar<AdicionarClienteBrasileiroPFContribuinte>(dest);
                else
                    MainPage.Current.Navegar<AdicionarClienteBrasileiroPF>(dest);
            }
            else if (!string.IsNullOrEmpty(dest.CNPJ))
                MainPage.Current.Navegar<AdicionarClienteBrasileiroPJ>(dest);
            else
                MainPage.Current.Navegar<AdicionarClienteEstrangeiro>(dest);
        }

        public override void AcaoSecundaria(ExibicaoGenerica contexto)
        {
            var dest = contexto.Convert<ClienteDI>();
            using (var repo = new BaseGeral.Repositorio.Escrita())
            {
                repo.InativarDadoBase(dest, DefinicoesTemporarias.DateTimeNow);
                Remover(contexto);
            }
        }

        protected override (string, string) ItemComparado(ExibicaoGenerica item, int modoBusca)
            => BuscadorCliente.StaticItemComparado(item.Convert<ClienteDI>(), modoBusca);

        protected override void InvalidarItem(ExibicaoGenerica item, int modoBusca)
            => BuscadorCliente.StaticInvalidarItem(item.Convert<ClienteDI>(), modoBusca);
    }
}
