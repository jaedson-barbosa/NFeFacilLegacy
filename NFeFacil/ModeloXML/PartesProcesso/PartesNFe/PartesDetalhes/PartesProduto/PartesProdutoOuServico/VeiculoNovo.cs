using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesProdutoOuServico
{
    public sealed class VeiculoNovo
    {
        /// <summary>
        /// Tipo da operação.
        /// </summary>
        public ushort tpOp { get; set; }

        /// <summary>
        /// Chassi do veículo.
        /// </summary>
        public string chassi { get; set; }

        /// <summary>
        /// Código de cada montadora.
        /// </summary>
        public string cCor { get; set; }

        /// <summary>
        /// Descrição da cor
        /// </summary>
        public string xCor { get; set; }

        /// <summary>
        /// Potência máxima do motor do veículo em cavalo vapor (CV).
        /// </summary>
        public string pot { get; set; }

        /// <summary>
        /// Capacidade voluntária do motor expressa em cilindradas.
        /// </summary>
        public string cilin { get; set; }

        /// <summary>
        /// Em toneladas - 4 casas decimais.
        /// </summary>
        public string pesoL { get; set; }

        /// <summary>
        /// Peso Bruto Total - em tonelada - 4 casas decimais.
        /// </summary>
        public string pesoB { get; set; }

        /// <summary>
        /// Número de série.
        /// </summary>
        public string nSerie { get; set; }

        /// <summary>
        /// Utilizar Tabela RENAVAM.
        /// </summary>
        public string tpComb;

        [XmlIgnore]
        public int TpComb
        {
            get { return tpComb != null ? int.Parse(tpComb) : 0; }
            set { tpComb = (value < 10) ? $"0{value}" : value.ToString(); }
        }

        /// <summary>
        /// Número de Motor.
        /// </summary>
        public string nMotor { get; set; }

        /// <summary>
        /// CMT-Capacidade Máxima de Tração - em Toneladas 4 casas decimais.
        /// </summary>
        public string CMT { get; set; }

        /// <summary>
        /// Distância entre eixos.
        /// </summary>
        public string dist { get; set; }

        /// <summary>
        /// Ano Modelo de Fabricação.
        /// </summary>
        public uint anoMod { get; set; }

        /// <summary>
        /// Ano de Fabricação.
        /// </summary>
        public uint anoFab { get; set; }

        /// <summary>
        /// Tipo de Pintura.
        /// </summary>
        public string tpPint { get; set; }

        /// <summary>
        /// Tipo de Veículo.
        /// Utilizar Tabela RENAVAM.
        /// </summary>
        public string tpVeic { get; set; }

        /// <summary>
        /// Espécie de Veículo. 
        /// Utilizar Tabela RENAVAM.
        /// </summary>
        public ushort espVeic { get; set; }

        /// <summary>
        /// Informa-se o veículo tem VIN (chassi) remarcado.
        /// R=Remarcado; N=Normal
        /// </summary>
        public char VIN;

        [XmlIgnore]
        public bool VINBool
        {
            get { return VIN == 'R'; }
            set { VIN = value ? 'R' : 'N'; }
        }

        /// <summary>
        /// Condição do Veículo.
        /// 1=Acabado; 2=Inacabado; 3=Semiacabado
        /// </summary>
        public ushort condVeic;

        [XmlIgnore]
        public int CondVeic
        {
            get { return condVeic - 1; }
            set { condVeic = (ushort)(value + 1); }
        }

        /// <summary>
        /// Código Marca Modelo.
        /// Utilizar Tabela RENAVAM.
        /// </summary>
        public string cMod { get; set; }

        /// <summary>
        /// Segundo as regras de pré-cadastro do DENATRAN (v2.0)
        /// 01=AMARELO, 02=AZUL, 03=BEGE, 04=BRANCA, 05=CINZA,06=-DOURADA,
        /// 07=GRENÁ, 08=LARANJA, 09=MARROM, 10=PRATA, 11=PRETA, 12=ROSA,
        /// 13=ROXA, 14=VERDE, 15=VERMELHA, 16=FANTASIA 151b
        /// </summary>
        public string cCorDENATRAN;

        [XmlIgnore]
        public int CCorDENATRAN
        {
            get { return cCorDENATRAN != null ? int.Parse(cCorDENATRAN) : 0; }
            set { cCorDENATRAN = value < 10 ? $"0{value}" : value.ToString(); }
        }

        /// <summary>
        /// Capacidade máxima de lotação, inclusive o motorista.
        /// </summary>
        public string lota { get; set; }

        /// <summary>
        /// Restrição:
        /// 0=Não há; 1=Alienação Fiduciária; 2=Arrendamento Mercantil;
        /// 3=Reserva de Domínio; 4=Penhor de Veículos; 9=Outras.
        /// </summary>
        public int tpRest;

        [XmlIgnore]
        public int TpRest
        {
            get { return tpRest != 9 ? tpRest : 5; }
            set { tpRest = value != 5 ? value : 9; }
        }
    }
}
