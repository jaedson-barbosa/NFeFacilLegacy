using BaseGeral.ModeloXML;
using System.Collections.Generic;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Fiscal
{
    public sealed class FormaPagamento
    {
        static Dictionary<string, string> DescCodigo = new Dictionary<string, string>
        {
            { "01", "Dinheiro" },
            { "02", "Cheque" },
            { "03", "Cartão de Crédito" },
            { "04", "Cartão de Débito" },
            { "05", "Crédito Loja" },
            { "10", "Vale Alimentação" },
            { "11", "Vale Refeição" },
            { "12", "Vale Presente" },
            { "13", "Vale Combustível" },
            { "15", "Boleto Bancário" },
            { "90", "Sem pagamento" },
            { "99", "Outros" }
        };

        public DetalhamentoPagamento Original { get; }
        public string Tipo { get; }
        public string Valor { get; set; }

        public FormaPagamento(DetalhamentoPagamento pagamento)
        {
            Original = pagamento;
            Tipo = DescCodigo[pagamento.Pagamento.Forma];
            Valor = pagamento.Pagamento.VPag;
        }
    }
}
