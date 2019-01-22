using BaseGeral;
using BaseGeral.ItensBD;
using BaseGeral.View;
using System;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace NFeFacil.ViewDadosBase
{
    public sealed class ControllerCategorias : ControllerGerenciadorGeral
    {
        public ControllerCategorias() : base("Inativar", 0)
        {
            using (var repo = new BaseGeral.Repositorio.Leitura())
            {
                TodosItens = repo.ObterCategorias().Select(Convert).ToArray();
                Itens = TodosItens.GerarObs();
            }
        }

        ExibicaoGenerica Convert(CategoriaDI old) => new ExibicaoEspecifica<CategoriaDI>(old, old.Nome, null, null);

        public override async void Adicionar()
        {
            var caixa = new AdicionarCategoria();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var newCategoria = new CategoriaDI() { Nome = caixa.Nome };
                using (var repo = new BaseGeral.Repositorio.Escrita())
                {
                    repo.SalvarItemSimples(newCategoria, DefinicoesTemporarias.DateTimeNow);
                }
                Itens.Add(Convert(newCategoria));
            }
        }

        public override async void Editar(ExibicaoGenerica contexto)
        {
            var categoria = contexto.Convert<CategoriaDI>();
            var index = Itens.IndexOf(contexto);
            var caixa = new AdicionarCategoria(categoria.Nome);
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                categoria.Nome = caixa.Nome;
                using (var repo = new BaseGeral.Repositorio.Escrita())
                {
                    repo.SalvarItemSimples(categoria, DefinicoesTemporarias.DateTimeNow);
                }
                Itens.RemoveAt(index);
                Itens.Insert(index, Convert(categoria));
            }
        }

        public override async void AcaoSecundaria(ExibicaoGenerica contexto)
        {
            var categoria = contexto.Convert<CategoriaDI>();
            var caixa = new AssociarCategoriaFornecedor(categoria);
            await caixa.ShowAsync();
        }

        protected override (string, string) ItemComparado(ExibicaoGenerica item, int modoBusca)
        {
            var categoria = item.Convert<CategoriaDI>();
            return (categoria.Nome, categoria.Nome);
        }

        protected override void InvalidarItem(ExibicaoGenerica item, int modoBusca)
        {
            var categoria = item.Convert<CategoriaDI>();
            categoria.Nome = InvalidItem;
        }
    }
}
