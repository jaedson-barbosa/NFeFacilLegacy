using System;
using Windows.UI.Xaml;

namespace NFeFacil.ViewModel
{
    public struct ObterDataContext<Retorno> : IObterPropriedade<Retorno>
    {
        public Retorno ObterPropriedade(object elemento)
        {
            if (elemento is FrameworkElement ok) return (Retorno)ok.DataContext;
            else throw new ArgumentException();
        }

        public bool TipoEsperado(object elemento) => elemento is FrameworkElement;
    }
}
