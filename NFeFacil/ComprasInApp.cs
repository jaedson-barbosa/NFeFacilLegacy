using BaseGeral;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Services.Store;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil
{
    static class ComprasInApp
    {
        public static Dictionary<Compras, bool> Resumo { get; private set; }

        public static async Task AnalisarCompras()
        {
            try
            {
                var storeContext = StoreContext.GetDefault();
                string[] productKinds = { "Durable" };
                var addOns = await storeContext.GetAssociatedStoreProductsAsync(productKinds);
                if (addOns.ExtendedError != null) throw addOns.ExtendedError;
                if (Resumo == null)
                {
                    Resumo = new Dictionary<Compras, bool>(2)
                    {
                        { Compras.NFCe, addOns.Products[Compras.NFCe].IsInUserCollection },
                        { Compras.Personalizacao, addOns.Products[Compras.Personalizacao].IsInUserCollection },
                        { Compras.RelatorioProdutos01, true/*addOns.Products[Compras.RelatorioProdutos01].IsInUserCollection*/ }
                    };
                }
                else
                {
                    Resumo[Compras.NFCe] = addOns.Products[Compras.NFCe].IsInUserCollection;
                    Resumo[Compras.Personalizacao] = addOns.Products[Compras.Personalizacao].IsInUserCollection;
                    Resumo[Compras.RelatorioProdutos01] = addOns.Products[Compras.RelatorioProdutos01].IsInUserCollection;
                }
            }
            catch (Exception e)
            {
                if (Resumo == null)
                {
                    Resumo = new Dictionary<Compras, bool>(2)
                    {
                        { Compras.NFCe, false },
                        { Compras.Personalizacao, false },
                        { Compras.RelatorioProdutos01, false }
                    };
                }
                new Exception("Erro ao obter as informações das compras dentro do aplicativo.", e).ManipularErro();
            }
        }

        public static async Task<bool> Comprar(Compras compra)
        {
            var prod = await ObterProduto(compra);
            var resultadoAquisicao = await prod.RequestPurchaseAsync();
            if (resultadoAquisicao.Status == StorePurchaseStatus.Succeeded
                || resultadoAquisicao.Status == StorePurchaseStatus.AlreadyPurchased)
            {
                if (Resumo.ContainsKey(compra)) Resumo[compra] = true;
                return true;
            }
            return false;
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
        public const string RelatorioProdutos01 = "9NWMBJMKT6VX";

        string Value;
        private Compras(string value) => Value = value;

        public static implicit operator Compras(string str) => new Compras(str);
        public static implicit operator string(Compras compra) => compra.Value;
    }
}
