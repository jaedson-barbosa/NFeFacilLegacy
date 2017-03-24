using System;
using System.Windows.Input;

namespace NFeFacil.ViewModel
{
    public class ComandoSemParametros : ICommand
    {
        private Action _action;
        private bool _canExecute;

        public ComandoSemParametros(Action action, bool canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter) => _canExecute;
        public void Execute(object parameter) => _action?.Invoke();
    }
}
