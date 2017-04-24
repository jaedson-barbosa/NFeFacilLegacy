using BibliotecaCentral.Log;

namespace BibliotecaCentral.Validacao
{
    public interface IValidavel
    {
        bool Validar(ILog log);
    }
}
