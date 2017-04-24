using NFeFacil.Sincronizacao.Pacotes;
using System.Threading.Tasks;

namespace NFeFacil.Sincronizacao
{
    internal static class ProcessamentoDadosBase
    {
        public static DadosBase Obter()
        {
            using (var db = new AplicativoContext())
            {
                return new DadosBase
                {
                    Emitentes = db.Emitentes,
                    Clientes = db.Clientes,
                    Motoristas = db.Motoristas,
                    Produtos = db.Produtos
                };
            }
        }

        public static async Task SalvarAsync(DadosBase dados)
        {
            using (var db = new AplicativoContext())
            {
                db.Emitentes.AddRange(dados.Emitentes);
                db.Clientes.AddRange(dados.Clientes);
                db.Motoristas.AddRange(dados.Motoristas); ;
                db.Produtos.AddRange(dados.Produtos);
                await db.SaveChangesAsync();
            }
        }
    }
}
