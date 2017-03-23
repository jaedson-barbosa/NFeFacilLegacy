namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    public class impostoDevol : Imposto
    {
        public string pDevol { get; set; }

        private IPIDevolvido ipi;
        public IPIDevolvido IPI
        {
            get
            {
                if (ipi == null) ipi = new IPIDevolvido();
                return ipi;
            }
            set
            {
                ipi = value;
            }
        }

        public override bool IsValido
        {
            get { return NaoNulos(pDevol, ipi); }
        }

        public class IPIDevolvido
        {
            public string vIPIDevol { get; set; }
        }
    }
}
