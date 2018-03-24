using NFeFacil.Produto.Impostos.DetalhamentoICMS.DadosRN;
using NFeFacil.Produto.Impostos.DetalhamentoICMS.DadosSN;
using System.Xml.Serialization;

namespace NFeFacil.Produto
{
    public sealed class ICMSArmazenado : ImpostoArmazenado
    {
        [XmlElement(nameof(Tipo101), typeof(Tipo101)),
            XmlElement(nameof(Tipo201), typeof(Tipo201)),
            XmlElement(nameof(Tipo202), typeof(Tipo202)),
            XmlElement(nameof(Tipo500), typeof(Tipo500)),
            XmlElement(nameof(Tipo900), typeof(Tipo900)),
            XmlElement(nameof(TipoNT), typeof(TipoNT))]
        public BaseSN SimplesNacional { get; set; }

        [XmlElement(nameof(Tipo0), typeof(Tipo0)),
            XmlElement(nameof(Tipo10), typeof(Tipo10)),
            XmlElement(nameof(Tipo20), typeof(Tipo20)),
            XmlElement(nameof(Tipo30), typeof(Tipo30)),
            XmlElement(nameof(Tipo40_41_50), typeof(Tipo40_41_50)),
            XmlElement(nameof(Tipo51), typeof(Tipo51)),
            XmlElement(nameof(Tipo60), typeof(Tipo60)),
            XmlElement(nameof(Tipo70), typeof(Tipo70)),
            XmlElement(nameof(Tipo90), typeof(Tipo90)),
            XmlElement(nameof(TipoICMSST), typeof(TipoICMSST)),
            XmlElement(nameof(TipoPart), typeof(TipoPart))]
        public BaseRN RegimeNormal { get; set; }

        public bool IsRegimeNormal { get; set; }
    }
}
