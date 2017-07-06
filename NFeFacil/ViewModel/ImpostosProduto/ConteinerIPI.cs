using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using System;
using System.Globalization;

namespace NFeFacil.ViewModel.ImpostosProduto
{
    public sealed class ConteinerIPI
    {
        ComumIPI original;
        Action atulizarContexto;
        CultureInfo culturaPadrao = CultureInfo.InvariantCulture;

        public double VBC
        {
            get
            {
                if (original is IPITrib trib && !string.IsNullOrEmpty(trib.vBC))
                {
                    return double.Parse(trib.vBC, culturaPadrao);
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (original is IPITrib trib)
                {
                    trib.vBC = value.ToString("F2", culturaPadrao);
                }
                CalcularValor();
            }
        }

        public double PIPI
        {
            get
            {
                if (original is IPITrib trib && !string.IsNullOrEmpty(trib.pIPI))
                {
                    return double.Parse(trib.pIPI, culturaPadrao);
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (original is IPITrib trib)
                {
                    trib.pIPI = value.ToString("F4", culturaPadrao);
                }
                CalcularValor();
            }
        }

        public double QUnid
        {
            get
            {
                if (original is IPITrib trib && !string.IsNullOrEmpty(trib.qUnid))
                {
                    return double.Parse(trib.qUnid, culturaPadrao);
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (original is IPITrib trib)
                {
                    trib.qUnid = value.ToString("F4", culturaPadrao);
                }
                CalcularValor();
            }
        }

        public double VUnid
        {
            get
            {
                if (original is IPITrib trib && !string.IsNullOrEmpty(trib.vUnid))
                {
                    return double.Parse(trib.vUnid, culturaPadrao);
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (original is IPITrib trib)
                {
                    trib.vUnid = value.ToString("F4", culturaPadrao);
                }
                CalcularValor();
            }
        }

        public double VIPI
        {
            get
            {
                if (original is IPITrib trib && !string.IsNullOrEmpty(trib.vIPI))
                {
                    return double.Parse(trib.vIPI, culturaPadrao);
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (original is IPITrib trib)
                {
                    trib.vIPI = value.ToString("F2", culturaPadrao);
                }
            }
        }

        public ConteinerIPI(Action atulizarContexto, ComumIPI original, ProdutoOuServico produto)
        {
            this.original = original;
            this.atulizarContexto = atulizarContexto;
            if (this.original is IPITrib trib)
            {
                double extras = 0;
                if (!string.IsNullOrEmpty(produto.Frete))
                {
                    extras += double.Parse(produto.Frete);
                }
                if (!string.IsNullOrEmpty(produto.Seguro))
                {
                    extras += double.Parse(produto.Seguro);
                }
                if (!string.IsNullOrEmpty(produto.DespesasAcessórias))
                {
                    extras += double.Parse(produto.DespesasAcessórias);
                }
                VBC = produto.ValorTotal + extras;
                QUnid = produto.QuantidadeComercializada;
            }
            CalcularValor();
        }

        void CalcularValor()
        {
            double valor;
            if (original is IPITrib)
            {
                if (VBC != 0 && PIPI != 0)
                {
                    valor = VBC * PIPI / 100;
                }
                else
                {
                    valor = QUnid * VUnid;
                }
            }
            else
            {
                valor = 0;
            }
            VIPI = valor;
            atulizarContexto();
        }
    }
}
