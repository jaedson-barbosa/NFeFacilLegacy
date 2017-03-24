using System;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace NFeFacil.ViewModel
{
    public sealed class ComandoComParametros<Parametro> : ICommand
    {
        private Action<Parametro> _action;
        private IObterPropriedade<Parametro> _processarEntrada;

        public ComandoComParametros(Action<Parametro> action,
            IObterPropriedade<Parametro> processarEntrada)
        {
            _action = action;
            _processarEntrada = processarEntrada;
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

    public struct ObterDataContext<Parametro> : IObterPropriedade<Parametro>
    {
        public Parametro ObterPropriedade(object elemento)
        {
            if (!(elemento is FrameworkElement)) throw new ArgumentException();
            var contexto = (elemento as FrameworkElement).DataContext;
            return (Parametro)contexto;
        }

        public bool TipoEsperado(object elemento)
        {
            return elemento is FrameworkElement;
        }
    }
}
