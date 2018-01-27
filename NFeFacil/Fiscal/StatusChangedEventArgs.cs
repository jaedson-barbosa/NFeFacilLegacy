using NFeFacil.ItensBD;
using System;

namespace NFeFacil.Fiscal
{
    public sealed class StatusChangedEventArgs : EventArgs
    {
        public StatusNFe NovoStatus { get; }

        public StatusChangedEventArgs(StatusNFe novoStatus)
        {
            NovoStatus = novoStatus;
        }
    }
}
