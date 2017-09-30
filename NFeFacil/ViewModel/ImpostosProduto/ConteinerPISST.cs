using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using System;
using System.Globalization;

namespace NFeFacil.ViewModel.ImpostosProduto
{
    public sealed class ConteinerPISST
    {
        PISST original;
        Action atulizarContexto;
        CultureInfo culturaPadrao = CultureInfo.InvariantCulture;

        public double VBC
        {
            get
            {
                if (original != null && !string.IsNullOrEmpty(original.vBC))
                {
                    return double.Parse(original.vBC, culturaPadrao);
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (original != null)
                {
                    original.vBC = value.ToString("F2", culturaPadrao);
                    CalcularValor();
                }
            }
        }

        public double PPIS
        {
            get
            {
                if (original != null && !string.IsNullOrEmpty(original.pPIS))
                {
                    return double.Parse(original.pPIS, culturaPadrao);
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (original != null)
                {
                    original.pPIS = value.ToString("F4", culturaPadrao);
                    CalcularValor();
                }
            }
        }

        public double QBCProd
        {
            get
            {
                if (original != null && !string.IsNullOrEmpty(original.qBCProd))
                {
                    return double.Parse(original.qBCProd, culturaPadrao);
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (original != null)
                {
                    original.qBCProd = value.ToString("F4", culturaPadrao);
                    CalcularValor();
                }
            }
        }

        public double VAliqProd
        {
            get
            {
                if (original != null && !string.IsNullOrEmpty(original.vAliqProd))
                {
                    return double.Parse(original.vAliqProd, culturaPadrao);
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (original != null)
                {
                    original.vAliqProd = value.ToString("F4", culturaPadrao);
                    CalcularValor();
                }
            }
        }

        public double VPIS
        {
            get
            {
                if (original != null && !string.IsNullOrEmpty(original.vPIS))
                {
                    return double.Parse(original.vPIS, culturaPadrao);
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (original != null)
                {
                    original.vPIS = value.ToString("F2", culturaPadrao);
                }
            }
        }

        public ConteinerPISST(Action atulizarContexto, PISST original, ProdutoOuServico produto)
        {
            this.original = original;
            this.atulizarContexto = atulizarContexto;
            VBC = produto.ValorTotal;
            QBCProd = produto.QuantidadeComercializada;
            CalcularValor();
        }

        void CalcularValor()
        {
            double valor;
            if (VBC != 0 && PPIS != 0)
            {
                valor = VBC * PPIS / 100;
            }
            else
            {
                valor = QBCProd * VAliqProd;
            }
            VPIS = valor;
            atulizarContexto();
        }
    }
}
