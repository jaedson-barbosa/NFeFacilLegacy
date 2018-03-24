using BaseGeral.IBGE;
using System;
using System.Linq;

namespace BaseGeral.View
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class PropriedadeExtensivel : Attribute
    {
        public PropriedadeExtensivel(string nomeExtensao, MetodosObtencao metodo)
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
                case MetodosObtencao.Municipio:
                    return ObterMunicipio(valor);
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
                if (sigla == "EX") return "Exterior";
                else return Estados.EstadosCache.First(x => x.Sigla == sigla).Nome;
            }
            throw new Exception("Formato inválido.");
        }

        static string ObterMunicipio(object valor)
        {
            return Municipios.MunicipiosCache.First(x => x.Codigo == (int)valor).Nome;
        }
    }
}
