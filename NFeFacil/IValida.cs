using System.Threading.Tasks;

namespace NFeFacil
{
    interface IValida
    {
        Task<bool> Verificar();
    }
}
