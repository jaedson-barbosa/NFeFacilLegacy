using System;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace NFeFacil.ViewModel
{
    public sealed class ComandoComParametros<Parametro, ProcessoPropriedades> : ICommand where ProcessoPropriedades : IObterPropriedade<Parametro>, new()
    {
        private Action<Parametro> _action;
        private IObterPropriedade<Parametro> _processarEntrada = new ProcessoPropriedades();

        public ComandoComParametros(Action<Parametro> action)
        {
            _action = action;
        }

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter) => _processarEntrada.TipoEsperado(parameter);
        public void Execute(object parameter) => _action?.Invoke(_processarEntrada.ObterPropriedade(parameter));
    }

    public interface IObterPropriedade<out Retorno>
    {
        bool TipoEsperado(object elemento);
        Retorno ObterPropriedade(object elemento);
    }

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
