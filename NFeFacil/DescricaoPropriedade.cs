using System;

namespace NFeFacil
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public sealed class DescricaoPropriedade : Attribute
    {
        public DescricaoPropriedade(string descricao)
        {
            Descricao = descricao;
        }

        public string Descricao { get; }
    }
}
