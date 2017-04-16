using System;
using System.Windows.Input;

namespace NFeFacil.ViewModel
{
    public sealed class ComandoParametrizado<Parametro, ProcessoPropriedades> : ICommand where ProcessoPropriedades : IObterPropriedade<Parametro>, new()
    {
        private Action<Parametro> _action;
        private IObterPropriedade<Parametro> _processarEntrada = new ProcessoPropriedades();

        public ComandoParametrizado(Action<Parametro> action)
        {
            _action = action;
        }

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter) => _processarEntrada.TipoEsperado(parameter);
        public void Execute(object parameter) => _action?.Invoke(_processarEntrada.ObterPropriedade(parameter));
    }
}
