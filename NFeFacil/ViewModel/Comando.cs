using System;
using System.Windows.Input;

namespace NFeFacil.ViewModel
{
    public sealed class Comando : ICommand
    {
        private Action _action;
        private bool _canExecute;

        public Comando(Action action, bool canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter) => _canExecute;
        public void Execute(object parameter) => _action?.Invoke();
    }

    public sealed class Comando<Parametro, ProcessoPropriedades> : ICommand where ProcessoPropriedades : IObterPropriedade<Parametro>, new()
    {
        private Action<Parametro> _action;
        private IObterPropriedade<Parametro> _processarEntrada = new ProcessoPropriedades();

        public Comando(Action<Parametro> action)
        {
            _action = action;
        }

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter) => _processarEntrada.TipoEsperado(parameter);
        public void Execute(object parameter) => _action?.Invoke(_processarEntrada.ObterPropriedade(parameter));
    }
}
