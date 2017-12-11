using NFeFacil.Log;
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
            Personalizacao,
            Doacao25,
            Doacao10
        }

        public Compras Escolhida { get; }

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
            StoreContext storeContext = StoreContext.GetDefault();
            string[] productKinds = new string[] { "Consumable", "Durable" };
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
