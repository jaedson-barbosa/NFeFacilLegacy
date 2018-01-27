using NFeFacil.ItensBD;
using NFeFacil.ModeloXML;
using System;
using System.Threading.Tasks;

namespace NFeFacil.Fiscal
{
    abstract class AcoesVisualizacao
    {
        public NFeDI ItemBanco { get; }
        public event EventHandler StatusChanged;

        protected AcoesVisualizacao(NFeDI nota)
        {
            ItemBanco = nota;
        }

        public StatusNFe Status { get; }
        public abstract void Editar();
        public abstract void Salvar();
        public abstract Task Assinar();
        public abstract Task Transmitir();
        public abstract void Imprimir();
        public abstract Task Exportar();
        public abstract InformacoesBase ObterVisualizacao();

        protected void OnStatusChanged(StatusNFe novoStatus)
        {
            StatusChanged?.Invoke(this, new StatusChangedEventArgs(novoStatus));
        }

        protected void AtualizarDI(object itemCompleto)
        {
            try
            {
                using (var repo = new Repositorio.Escrita())
                {
                    ItemBanco.XML = ItemBanco.Status < (int)StatusNFe.Emitida
                        ? itemCompleto.ToXElement().ToString()
                        : itemCompleto.ToXElement().ToString();
                    repo.SalvarItemSimples(ItemBanco, DefinicoesTemporarias.DateTimeNow);
                }
            }
            catch (Exception e)
            {
                e.ManipularErro();
            }
        }
    }
}
