using NFeFacil.Log;
using System;
using System.Threading.Tasks;
using Windows.Services.Store;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil
{
    static class ComprasInApp
    {
        public static async Task<bool> Comprar(Compras compra)
        {
            var prod = await ObterProduto(compra);
            var resultadoAquisicao = await prod.RequestPurchaseAsync();
            return resultadoAquisicao.Status == StorePurchaseStatus.Succeeded
                || resultadoAquisicao.Status == StorePurchaseStatus.AlreadyPurchased;
        }

        public static async Task<StoreProduct> ObterProduto(Compras compra)
        {
            var storeContext = StoreContext.GetDefault();
            string[] productKinds = { "Consumable", "Durable" };
            var addOns = await storeContext.GetAssociatedStoreProductsAsync(productKinds);
            return addOns.Products[compra];
        }
    }

    struct Compras
    {
        public const string Personalizacao = "9P70MWLRCS54";
        public const string Doacao25 = "9NJNJTZQ85G5";
        public const string Doacao10 = "9MXJQH2JC335";
        public const string NFCe = "9NPT3PV6BT0X";

        string Value;
        private Compras(string value) => Value = value;

        public static implicit operator Compras(string str) => new Compras(str);
        public static implicit operator string(Compras compra) => compra.Value;
    }
}
