using BaseGeral.ItensBD;
using System;

namespace NFeFacil.Fiscal
{
    public sealed class StatusChangedEventArgs : EventArgs
    {
        public StatusNota NovoStatus { get; }

        public StatusChangedEventArgs(StatusNota novoStatus)
        {
            NovoStatus = novoStatus;
        }
    }
}
