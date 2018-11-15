using BaseGeral.ModeloXML;
using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesTransporte;
using System;
using System.Collections.Generic;

namespace BaseGeral.ItensBD
{
    public sealed class RegistroVenda : IUltimaData, IGuidId
    {
        public Guid Id { get; set; }
        public DateTime UltimaData { get; set; }
        public string NotaFiscalRelacionada { get; set; }

        public Guid Emitente { get; set; }
        public Guid Vendedor { get; set; }
        public Guid Cliente { get; set; }
        public Guid Motorista { get; set; }
        public List<ProdutoSimplesVenda> Produtos { get; set; }
        public DateTime DataHoraVenda { get; set; }
        public string Observações { get; set; }
        public double DescontoTotal { get; set; }
        public bool Cancelado { get; set; }

        public string TipoFrete { get; set; }
        public DateTime PrazoEntrega { get; set; }
        public string PrazoPagamento { get; set; }
        public string FormaPagamento { get; set; }
        public Guid Comprador { get; set; }
        public string CondicaoPagamento { get; set; }

        public string MotivoEdicao { get; set; }

        public NFe ToNFe()
        {
            using (var db = new AplicativoContext())
            {
                var prods = ObterProdutosProcessados();
                var motDI = Motorista != default(Guid) ? db.Motoristas.Find(Motorista) : null;
                return new NFe()
                {
                    Informacoes = new InformacoesNFe
                    {
                        destinatario = db.Clientes.Find(Cliente).ToDestinatario(),
                        Emitente = db.Emitentes.Find(Emitente).ToEmitente(),
                        infAdic = new InformacoesAdicionais
                        {
                            InfCpl = Observações
                        },
                        produtos = prods,
                        total = new Total(prods),
                        transp = new Transporte()
                        {
                            Transporta = motDI != null ? motDI.ToMotorista() : new Motorista(),
                            RetTransp = new ICMSTransporte(),
                            VeicTransp = motDI != null && motDI.Veiculo != default(Guid)
                                ? db.Veiculos.Find(motDI.Veiculo).ToVeiculo() 
                                : new Veiculo()
                        },
                        cobr = new Cobranca(),
                        exporta = new Exportacao(),
                        compra = new Compra(),
                        cana = new RegistroAquisicaoCana()
                    }
                };
            }

            List<DetalhesProdutos> ObterProdutosProcessados()
            {
                var retorno = new List<DetalhesProdutos>(Produtos.Count);
                for (int i = 0; i < Produtos.Count; i++)
                {
                    retorno.Add(new DetalhesProdutos
                    {
                        Número = i + 1,
                        Produto = Produtos[i].ToProdutoOuServico()
                    });
                }
                return retorno;
            }
        }
    }
}
