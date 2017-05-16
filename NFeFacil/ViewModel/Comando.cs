using System;
using System.Windows.Input;

namespace NFeFacil.ViewModel
{
    public sealed class Comando : ICommand
    {
        private Action _action;
        private bool _canExecute;

        public Comando(Action action, bool canExecute = true)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter) => _canExecute;
        public void Execute(object parameter) => _action?.Invoke();
    }

    public sealed class Comando<Parametro> : ICommand
    {
        private Action<Parametro> _action;
        private IObterPropriedade<Parametro> _processarEntrada;

        public Comando(Action<Parametro> action)
        {
            _action = action;
            _processarEntrada = new ObterDataContext<Parametro>();
        }

        public Comando(Action<Parametro> action, IObterPropriedade<Parametro> processa)
        {
            _action = action;
            _processarEntrada = processa;
        }

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter) => _processarEntrada.TipoEsperado(parameter);
        public void Execute(object parameter) => _action?.Invoke(_processarEntrada.ObterPropriedade(parameter));
    }
}
