﻿using BibliotecaCentral.ItensBD;
using BibliotecaCentral.Log;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;

namespace BibliotecaCentral.Validacao
{
    public sealed class ValidadorEmitente : IValidavel
    {
        private Emitente Emit;

        public ValidadorEmitente(Emitente emit)
        {
            Emit = emit;
        }

        public ValidadorEmitente(EmitenteDI emit)
        {
            Emit = emit.ToEmitente();
        }

        public bool Validar(ILog log)
        {
            return new ValidarDados(new Validadorendereco(Emit.Endereco)).ValidarTudo(log,
                new ConjuntoAnalise(string.IsNullOrEmpty(Emit.Nome), "Não foi informado o nome do emitente"),
                new ConjuntoAnalise(Emit.CNPJ == 0, "Não foi informado o CNPJ do emitente"),
                new ConjuntoAnalise(Emit.InscricaoEstadual == 0, "Não foi informada a inscrição estadual do emitente"));
        }
    }
}
