using NFeFacil.Log;
using System;
using System.Threading.Tasks;
using Windows.Services.Store;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil
{
    sealed class ComprasInApp
    {
        public struct Compras
        {
            public const string Personalizacao = "9P70MWLRCS54";
            public const string Doacao25 = "9NJNJTZQ85G5";
            public const string Doacao10 = "9MXJQH2JC335";

            string Value;
            private Compras(string value) => Value = value;

            public static implicit operator Compras(string str) => new Compras(str);
            public static implicit operator string(Compras compra) => compra.Value;
        }

        Compras Escolhida { get; }

        public ComprasInApp(Compras compra)
        {
            Escolhida = compra;
        }

        bool Comprado = false;
        public async Task<bool> AnalisarCompra()
        {
            try
            {
                if (Comprado == false)
                {
                    var prod = await ObterProduto();
                    if (prod.IsInUserCollection)
                    {
                        Comprado = true;
                    }
                    else
                    {
                        StoreContext storeContext = StoreContext.GetDefault();
                        var resultadoAquisicao = await storeContext.RequestPurchaseAsync(prod.StoreId);
                        Comprado = resultadoAquisicao.Status == StorePurchaseStatus.Succeeded
                            || resultadoAquisicao.Status == StorePurchaseStatus.AlreadyPurchased;
                    }
                }
                return Comprado;
            }
            catch (Exception e)
            {
                Popup.Current.Escrever(TitulosComuns.Erro, e.Message);
                return false;
            }
        }

        public async Task<bool> Comprar()
        {
            var prod = await ObterProduto();
            StoreContext storeContext = StoreContext.GetDefault();
            var resultadoAquisicao = await storeContext.RequestPurchaseAsync(prod.StoreId);
            return Comprado = resultadoAquisicao.Status == StorePurchaseStatus.Succeeded
                || resultadoAquisicao.Status == StorePurchaseStatus.AlreadyPurchased;
        }

        async Task<StoreProduct> ObterProduto()
        {
            var storeContext = StoreContext.GetDefault();
            string[] productKinds = new string[] { "Consumable", "Durable" };
            var addOns = await storeContext.GetAssociatedStoreProductsAsync(productKinds);
            var keys = addOns.Products.Keys;
            return addOns.Products[Escolhida];
        }
    }
}
