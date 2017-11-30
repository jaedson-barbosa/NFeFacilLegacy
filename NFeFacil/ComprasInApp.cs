using System;
using System.Threading.Tasks;
using Windows.Services.Store;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil
{
    sealed class ComprasInApp
    {
        public enum Compras
        {
            Personalizacao
        }

        public Compras Escolhida { get; }

        public ComprasInApp(Compras compra)
        {
            Escolhida = compra;
        }

        bool Comprado = false;
        public async Task<bool> AnalisarCompra()
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

        async Task<StoreProduct> ObterProduto()
        {
            StoreContext storeContext = StoreContext.GetDefault();
            string[] productKinds = new string[] { "Consumable", "Durable", "UnmanagedConsumable" };
            StoreProductQueryResult addOns = await storeContext.GetAssociatedStoreProductsAsync(productKinds);
            foreach (var item in addOns.Products.Values)
            {
                if (item.InAppOfferToken == Escolhida.ToString())
                {
                    return item;
                }
            }
            throw new Exception("Erro ao buscar o produto especificado.");
        }
    }
}
