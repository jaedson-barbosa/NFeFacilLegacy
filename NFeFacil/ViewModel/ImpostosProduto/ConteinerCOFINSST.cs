using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using System;
using System.Globalization;

namespace NFeFacil.ViewModel.ImpostosProduto
{
    public sealed class ConteinerCOFINSST
    {
        COFINSST original;
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

        public double PCOFINS
        {
            get
            {
                if (original != null && !string.IsNullOrEmpty(original.pCOFINS))
                {
                    return double.Parse(original.pCOFINS, culturaPadrao);
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
                    original.pCOFINS = value.ToString("F4", culturaPadrao);
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

        public double VCOFINS
        {
            get
            {
                if (original != null && !string.IsNullOrEmpty(original.vCOFINS))
                {
                    return double.Parse(original.vCOFINS, culturaPadrao);
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
                    original.vCOFINS = value.ToString("F2", culturaPadrao);
                }
            }
        }

        public ConteinerCOFINSST(Action atulizarContexto, COFINSST original, ProdutoOuServico produto)
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
            if (VBC != 0 && PCOFINS != 0)
            {
                valor = VBC * PCOFINS / 100;
            }
            else
            {
                valor = QBCProd * VAliqProd;
            }
            VCOFINS = valor;
            atulizarContexto();
        }
    }
}
