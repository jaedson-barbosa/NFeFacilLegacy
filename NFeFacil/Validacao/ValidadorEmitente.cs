﻿using NFeFacil.Log;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;

namespace NFeFacil.Validacao
{
    public sealed class ValidadorEmitente : IValidavel
    {
        private Emitente Emit;

        public ValidadorEmitente(Emitente emit)
        {
            Emit = emit;
        }

        public bool Validar(ILog log)
        {
            return new ValidarDados(new ValidadorEndereco(Emit.endereço)).ValidarTudo(log,
                new ConjuntoAnalise(Emit.regimeTributario < 1, "Não foi escolhido um regime tributário"),
                new ConjuntoAnalise(string.IsNullOrEmpty(Emit.nome), "Não foi informado o nome do emitente"),
                new ConjuntoAnalise(string.IsNullOrEmpty(Emit.CNPJ), "Não foi informado o CNPJ do emitente"),
                new ConjuntoAnalise(string.IsNullOrEmpty(Emit.inscricaoEstadual), "Não foi informada a inscrição estadual do emitente"));
        }
    }
}
