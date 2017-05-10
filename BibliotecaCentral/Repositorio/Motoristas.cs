using System.Collections.Generic;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte;
using System;

namespace BibliotecaCentral.Repositorio
{
    public sealed class Motoristas : ConexaoBanco
    {
        public IEnumerable<Motorista> Registro => Contexto.Motoristas;

        public void Adicionar(Motorista dado)
        {
            dado.UltimaData = DateTime.Now;
            Contexto.Add(dado);
        }

        public void Atualizar(Motorista dado)
        {
            dado.UltimaData = DateTime.Now;
            Contexto.Update(dado);
        }

        public void Remover(Motorista dado)
        {
            Contexto.Remove(dado);
        }
    }
}
