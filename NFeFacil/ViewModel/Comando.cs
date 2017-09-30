using System;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace NFeFacil.ViewModel
{
    public sealed class Comando : ICommand
    {
        private Action _action;

        public Comando(Action action)
        {
            _action = action;
        }

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => _action?.Invoke();
    }

    public sealed class Comando<Parametro> : ICommand
    {
        private Action<Parametro> _action;

        public Comando(Action<Parametro> action)
        {
            _action = action;
        }

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter) => TipoEsperado(parameter);
        public void Execute(object parameter) => _action?.Invoke(ObterPropriedade(parameter));

        public Parametro ObterPropriedade(object elemento)
        {
            if (elemento is FrameworkElement ok) return (Parametro)ok.DataContext;
            else throw new ArgumentException();
        }

        public bool TipoEsperado(object elemento) => elemento is FrameworkElement;
    }
}
