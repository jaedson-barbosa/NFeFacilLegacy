using BaseGeral.ItensBD;
using System.Threading.Tasks;

namespace Fiscal
{
    public interface IControleView
    {
        bool IsNFCe { get; }
        void Exibir(NFeDI nota);
        Task<bool> Cancelar(NFeDI nota);
        Task CriarCopia(NFeDI nota);
    }
}
