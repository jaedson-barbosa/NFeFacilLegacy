using System;

namespace BibliotecaCentral.Sincronizacao.Pacotes
{
    public abstract class PacoteBase
    {
        public DateTime HoraRequisição { get; set; } = DateTime.Now;
    }
}
