using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using System.Globalization;
using System;

namespace NFeFacil.ViewModel.ImpostosProduto
{
    public sealed class ConteinerPIS
    {
        ComumPIS original;
        Action atulizarContexto;
        CultureInfo culturaPadrao = CultureInfo.InvariantCulture;

        public double VBC
        {
            get
            {
                if (original is PISAliq aliq && !string.IsNullOrEmpty(aliq.vBC))
                {
                    return double.Parse(aliq.vBC, culturaPadrao);
                }
                else if (original is PISOutr outr && !string.IsNullOrEmpty(outr.vBC))
                {
                    return double.Parse(outr.vBC, culturaPadrao);
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (original is PISAliq aliq)
                {
                    aliq.vBC = value.ToString("F2", culturaPadrao);
                }
                else if (original is PISOutr outr)
                {
                    outr.vBC = value.ToString("F2", culturaPadrao);
                }
                CalcularValor();
            }
        }

        public double PPIS
        {
            get
            {
                if (original is PISAliq aliq && !string.IsNullOrEmpty(aliq.pPIS))
                {
                    return double.Parse(aliq.pPIS, culturaPadrao);
                }
                else if (original is PISOutr outr && !string.IsNullOrEmpty(outr.pPIS))
                {
                    return double.Parse(outr.pPIS, culturaPadrao);
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (original is PISAliq aliq)
                {
                    aliq.pPIS = value.ToString("F4", culturaPadrao);
                }
                else if (original is PISOutr outr)
                {
                    outr.pPIS = value.ToString("F4", culturaPadrao);
                }
                CalcularValor();
            }
        }

        public double QBCProd
        {
            get
            {
                if (original is PISQtde qtde && !string.IsNullOrEmpty(qtde.qBCProd))
                {
                    return double.Parse(qtde.qBCProd, culturaPadrao);
                }
                else if (original is PISOutr outr && !string.IsNullOrEmpty(outr.qBCProd))
                {
                    return double.Parse(outr.qBCProd, culturaPadrao);
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (original is PISQtde qtde)
                {
                    qtde.qBCProd = value.ToString("F4", culturaPadrao);
                }
                else if (original is PISOutr outr)
                {
                    outr.qBCProd = value.ToString("F4", culturaPadrao);
                }
                CalcularValor();
            }
        }

        public double VAliqProd
        {
            get
            {
                if (original is PISQtde qtde && !string.IsNullOrEmpty(qtde.vAliqProd))
                {
                    return double.Parse(qtde.vAliqProd, culturaPadrao);
                }
                else if (original is PISOutr outr && !string.IsNullOrEmpty(outr.vAliqProd))
                {
                    return double.Parse(outr.vAliqProd, culturaPadrao);
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (original is PISQtde qtde)
                {
                    qtde.vAliqProd = value.ToString("F4", culturaPadrao);
                }
                else if (original is PISOutr outr)
                {
                    outr.vAliqProd = value.ToString("F4", culturaPadrao);
                }
                CalcularValor();
            }
        }

        public double VPIS
        {
            get
            {
                if (original is PISAliq aliq && !string.IsNullOrEmpty(aliq.vPIS))
                {
                    return double.Parse(aliq.vPIS, culturaPadrao);
                }
                else if (original is PISQtde qtde && !string.IsNullOrEmpty(qtde.vPIS))
                {
                    return double.Parse(qtde.vPIS, culturaPadrao);
                }
                else if (original is PISOutr outr && !string.IsNullOrEmpty(outr.vPIS))
                {
                    return double.Parse(outr.vPIS, culturaPadrao);
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (original is PISAliq aliq)
                {
                    aliq.vPIS = value.ToString("F2", culturaPadrao);
                }
                else if (original is PISQtde qtde)
                {
                    qtde.vPIS = value.ToString("F2", culturaPadrao);
                }
                else if (original is PISOutr outr)
                {
                    outr.vPIS = value.ToString("F2", culturaPadrao);
                }
            }
        }

        public ConteinerPIS(Action atulizarContexto, ComumPIS original, ProdutoOuServico produto)
        {
            this.original = original;
            this.atulizarContexto = atulizarContexto;
            if (this.original is PISAliq aliq)
            {
                VBC = produto.ValorTotal;
            }
            else if (this.original is PISQtde qtde)
            {
                QBCProd = produto.QuantidadeComercializada;
            }
            else if (this.original is PISOutr outr)
            {
                VBC = produto.ValorTotal;
                QBCProd = produto.QuantidadeComercializada;
            }
            CalcularValor();
        }

        void CalcularValor()
        {
            double valor;
            if (original is PISAliq aliq)
            {
                valor = VBC * PPIS / 100;
            }
            else if (original is PISQtde qtde)
            {
                valor = QBCProd * VAliqProd;
            }
            else if (original is PISOutr outr)
            {
                if (VBC != 0 && PPIS != 0)
                {
                    valor = VBC * PPIS / 100;
                }
                else
                {
                    valor = QBCProd * VAliqProd;
                }
            }
            else
            {
                valor = 0;
            }
            VPIS = valor;
            atulizarContexto();
        }
    }
}
