using NFeFacil.View;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesDetalhes.PartesProduto.PartesProdutoOuServico
{
    public sealed class VeiculoNovo
    {
        [XmlElement(Order = 0), DescricaoPropriedade("Tipo da operação")]
        public ushort tpOp { get; set; }

        [XmlElement(Order = 1), DescricaoPropriedade("Chassi do veículo")]
        public string chassi { get; set; }

        [XmlElement(Order = 2), DescricaoPropriedade("Código de cada montadora")]
        public string cCor { get; set; }

        [XmlElement(Order = 3), DescricaoPropriedade("Descrição da cor")]
        public string xCor { get; set; }

        [XmlElement(Order = 4), DescricaoPropriedade("Potência máxima do motor do veículo em cavalo vapor (CV)")]
        public string pot { get; set; }

        [XmlElement(Order = 5), DescricaoPropriedade("Capacidade voluntária do motor expressa em cilindradas")]
        public string cilin { get; set; }

        [XmlElement(Order = 6), DescricaoPropriedade("Peso líquido em toneladas")]
        public string pesoL { get; set; }

        [XmlElement(Order = 7), DescricaoPropriedade("Peso bruto em toneladas")]
        public string pesoB { get; set; }

        [XmlElement(Order = 8), DescricaoPropriedade("Número de série")]
        public string nSerie { get; set; }

        [XmlElement(Order = 9), DescricaoPropriedade("Tipo de combustível")]
        public string tpComb { get; set; }

        [XmlIgnore]
        public int TpComb
        {
            get => tpComb != null ? int.Parse(tpComb) : 0;
            set => tpComb = (value < 10) ? $"0{value}" : value.ToString();
        }

        [XmlElement(Order = 10), DescricaoPropriedade("Número de Motor")]
        public string nMotor { get; set; }

        [XmlElement(Order = 11), DescricaoPropriedade("Capacidade Máxima de Tração em toneladas")]
        public string CMT { get; set; }

        [XmlElement(Order = 12), DescricaoPropriedade("Distância entre eixos")]
        public string dist { get; set; }

        [XmlElement(Order = 13), DescricaoPropriedade("Ano do modelo de fabricação")]
        public short anoMod { get; set; }

        [XmlElement(Order = 14), DescricaoPropriedade("Ano de fabricação")]
        public short anoFab { get; set; }

        [XmlElement(Order = 15), DescricaoPropriedade("Tipo de pintura")]
        public string tpPint { get; set; }

        [XmlElement(Order = 16), DescricaoPropriedade("Tipo de veículo")]
        public ushort tpVeic { get; set; }

        [XmlElement(Order = 17), DescricaoPropriedade("Espécie de veículo")]
        public ushort espVeic { get; set; }

        [XmlElement(Order = 18)]
        public char VIN { get; set; }

        [XmlIgnore]
        public bool VINBool
        {
            get => VIN == 'R';
            set => VIN = value ? 'R' : 'N';
        }

        [XmlElement(Order = 19), DescricaoPropriedade("Condição do veículo")]
        public ushort condVeic { get; set; }

        [XmlElement(Order = 20), DescricaoPropriedade("Código da marca do modelo")]
        public int cMod { get; set; }

        [XmlElement("cCorDENATRAN", Order = 21), DescricaoPropriedade("Cor")]
        public ushort CCorDENATRAN { get; set; }

        /// <summary>
        /// .
        /// </summary>
        [XmlElement(Order = 22), DescricaoPropriedade("Capacidade máxima de lotação, inclusive o motorista")]
        public short lota { get; set; }

        [XmlElement(Order = 23), DescricaoPropriedade("Restrição")]
        public ushort tpRest { get; set; }
    }
}
