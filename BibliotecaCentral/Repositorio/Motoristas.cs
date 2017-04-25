using System.Collections.Generic;
using BibliotecaCentral.ItensBD;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte;

namespace BibliotecaCentral.Repositorio
{
    public sealed class Motoristas : ConexaoBanco
    {
        public IEnumerable<Motorista> Registro => Contexto.Motoristas;
        public void Adicionar(Motorista dado) => Contexto.Add(dado);
        public void Atualizar(Motorista dado) => Contexto.Update(dado);
        public void Remover(Motorista dado) => Contexto.Remove(dado);
    }
}
