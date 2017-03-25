using System.Threading.Tasks;

namespace NFeFacil.View
{
    interface IValida
    {
        Task<bool> Verificar();
    }
}
