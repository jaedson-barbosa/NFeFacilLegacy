namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto
{
    public abstract class Imposto
    {
        public abstract bool IsValido { get; }

        protected bool NaoNulos(params object[] obrigatorios)
        {
            for (int i = 0; i < obrigatorios.Length; i++)
            {
                if (obrigatorios[i] is string str)
                {
                    if (string.IsNullOrWhiteSpace(str)) return false;
                }
                else if (obrigatorios[i] is int inteiro)
                {
                    if (inteiro == 0) return false;
                }
                else if (obrigatorios[i] is double quebrado)
                {
                    if (quebrado == 0) return false;
                }
                else if (obrigatorios[i] == null) return false;
            }
            return true;
        }
    }
}
