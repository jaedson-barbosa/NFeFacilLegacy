using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using System;
using System.Globalization;

namespace NFeFacil.ViewModel.ImpostosProduto
{
    public sealed class ConteinerCOFINS
    {
        ComumCOFINS original;
        Action atulizarContexto;
        CultureInfo culturaPadrao = CultureInfo.InvariantCulture;

        public double VBC
        {
            get
            {
                if (original is COFINSAliq aliq && !string.IsNullOrEmpty(aliq.vBC))
                {
                    return double.Parse(aliq.vBC, culturaPadrao);
                }
                else if (original is COFINSOutr outr && !string.IsNullOrEmpty(outr.vBC))
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
                if (original is COFINSAliq aliq)
                {
                    aliq.vBC = value.ToString("F2", culturaPadrao);
                }
                else if (original is COFINSOutr outr)
                {
                    outr.vBC = value.ToString("F2", culturaPadrao);
                }
                CalcularValor();
            }
        }

        public double PCOFINS
        {
            get
            {
                if (original is COFINSAliq aliq && !string.IsNullOrEmpty(aliq.pCOFINS))
                {
                    return double.Parse(aliq.pCOFINS, culturaPadrao);
                }
                else if (original is COFINSOutr outr && !string.IsNullOrEmpty(outr.pCOFINS))
                {
                    return double.Parse(outr.pCOFINS, culturaPadrao);
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (original is COFINSAliq aliq)
                {
                    aliq.pCOFINS = value.ToString("F4", culturaPadrao);
                }
                else if (original is COFINSOutr outr)
                {
                    outr.pCOFINS = value.ToString("F4", culturaPadrao);
                }
                CalcularValor();
            }
        }

        public double QBCProd
        {
            get
            {
                if (original is COFINSQtde qtde && !string.IsNullOrEmpty(qtde.qBCProd))
                {
                    return double.Parse(qtde.qBCProd, culturaPadrao);
                }
                else if (original is COFINSOutr outr && !string.IsNullOrEmpty(outr.qBCProd))
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
                if (original is COFINSQtde qtde)
                {
                    qtde.qBCProd = value.ToString("F4", culturaPadrao);
                }
                else if (original is COFINSOutr outr)
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
                if (original is COFINSQtde qtde && !string.IsNullOrEmpty(qtde.vAliqProd))
                {
                    return double.Parse(qtde.vAliqProd, culturaPadrao);
                }
                else if (original is COFINSOutr outr && !string.IsNullOrEmpty(outr.vAliqProd))
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
                if (original is COFINSQtde qtde)
                {
                    qtde.vAliqProd = value.ToString("F4", culturaPadrao);
                }
                else if (original is COFINSOutr outr)
                {
                    outr.vAliqProd = value.ToString("F4", culturaPadrao);
                }
                CalcularValor();
            }
        }

        public double VCOFINS
        {
            get
            {
                if (original is COFINSAliq aliq && !string.IsNullOrEmpty(aliq.vCOFINS))
                {
                    return double.Parse(aliq.vCOFINS, culturaPadrao);
                }
                else if (original is COFINSQtde qtde && !string.IsNullOrEmpty(qtde.vCOFINS))
                {
                    return double.Parse(qtde.vCOFINS, culturaPadrao);
                }
                else if (original is COFINSOutr outr && !string.IsNullOrEmpty(outr.vCOFINS))
                {
                    return double.Parse(outr.vCOFINS, culturaPadrao);
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (original is COFINSAliq aliq)
                {
                    aliq.vCOFINS = value.ToString("F2", culturaPadrao);
                }
                else if (original is COFINSQtde qtde)
                {
                    qtde.vCOFINS = value.ToString("F2", culturaPadrao);
                }
                else if (original is COFINSOutr outr)
                {
                    outr.vCOFINS = value.ToString("F2", culturaPadrao);
                }
            }
        }

        public ConteinerCOFINS(Action atulizarContexto, ComumCOFINS original, ProdutoOuServico produto)
        {
            this.original = original;
            this.atulizarContexto = atulizarContexto;
            if (this.original is COFINSAliq aliq)
            {
                VBC = produto.ValorTotal;
            }
            else if (this.original is COFINSQtde qtde)
            {
                QBCProd = produto.QuantidadeComercializada;
            }
            else if (this.original is COFINSOutr outr)
            {
                VBC = produto.ValorTotal;
                QBCProd = produto.QuantidadeComercializada;
            }
            CalcularValor();
        }

        void CalcularValor()
        {
            double valor;
            if (original is COFINSAliq aliq)
            {
                valor = VBC * PCOFINS / 100;
            }
            else if (original is COFINSQtde qtde)
            {
                valor = QBCProd * VAliqProd;
            }
            else if (original is COFINSOutr outr)
            {
                if (VBC != 0 && PCOFINS != 0)
                {
                    valor = VBC * PCOFINS / 100;
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
            VCOFINS = valor;
            atulizarContexto();
        }
    }
}
