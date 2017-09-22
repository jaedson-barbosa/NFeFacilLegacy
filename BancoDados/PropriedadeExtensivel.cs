using NFeFacil.IBGE;
using System;
using System.Linq;

namespace NFeFacil
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class PropriedadeExtensivel : Attribute
    {
        internal PropriedadeExtensivel(string nomeExtensao, MetodosObtencao metodo)
        {
            NomeExtensão = nomeExtensao;
            Metodo = metodo;
        }

        public string NomeExtensão { get; }
        MetodosObtencao Metodo { get; }

        public string ObterValor(object valor)
        {
            switch (Metodo)
            {
                case MetodosObtencao.Estado:
                    return ObterEstado(valor);
                default:
                    throw new Exception("Método não cadastrado");
            }
        }

        static string ObterEstado(object valor)
        {
            if (valor is ushort numerico)
            {
                return Estados.EstadosCache.First(x => x.Codigo == numerico).Nome;
            }
            else if (valor is string sigla)
            {
                return Estados.EstadosCache.First(x => x.Sigla == sigla).Nome;
            }
            throw new Exception("Formato inválido.");
        }
    }

    enum MetodosObtencao
    {
        Estado
    }
}
