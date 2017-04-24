using System;

namespace NFeFacil.Sincronizacao.Pacotes
{
    public abstract class PacoteBase
    {
        public DateTime HoraRequisição { get; set; } = DateTime.Now;
    }
}
