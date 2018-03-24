using System;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace BaseGeral.Controles
{
    public sealed class NumeroChangedEventArgs : EventArgs
    {
        public double NovoNumero { get; }

        public NumeroChangedEventArgs(double novoNumero)
        {
            NovoNumero = novoNumero;
        }
    }
}
