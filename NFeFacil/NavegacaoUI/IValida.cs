using System.Threading.Tasks;

namespace NFeFacil.NavegacaoUI
{
    interface IValida
    {
        Task<bool> Verificar();
    }
}
