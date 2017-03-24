using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace NFeFacil.ViewModel
{
    public sealed class ProdutoEspecificoDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(params string[] parametros)
        {
            for (int i = 0; i < parametros.Length; i++)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(parametros[i]));
            }
        }

        public ProdutoOuServiço Produto { get; set; }

        public VeículosNovos Veiculo
        {
            get { return Produto.veicProd ?? (Produto.veicProd = new VeículosNovos()); }
        }

        public Combustível Comb
        {
            get { return Produto.comb ?? (Produto.comb = new Combustível()); }
        }

        public ObservableCollection<TiposEspeciaisProduto> ListaTiposEspeciaisProduto
        {
            get { return Enum.GetValues(typeof(TiposEspeciaisProduto)).Cast<TiposEspeciaisProduto>().GerarObs(); }
        }

        private TiposEspeciaisProduto tipoEspecialProdutoSelecionado;
        public TiposEspeciaisProduto TipoEspecialProdutoSelecionado
        {
            get { return tipoEspecialProdutoSelecionado; }
            set
            {
                tipoEspecialProdutoSelecionado = value;
                VisibilidadeVeiculoNovo = TiposEspeciaisProduto.Veículo == value;
                VisibilidadeMedicamento = TiposEspeciaisProduto.Medicamento == value;
                VisibilidadeArmamento = TiposEspeciaisProduto.Armamento == value;
                VisibilidadeCombustivel = TiposEspeciaisProduto.Combustível == value;
                VisibilidadePapel = TiposEspeciaisProduto.Papel == value;
                OnPropertyChanged(nameof(VisibilidadeVeiculoNovo),
                    nameof(VisibilidadeMedicamento),
                    nameof(VisibilidadeArmamento),
                    nameof(VisibilidadeCombustivel),
                    nameof(VisibilidadePapel));
                if (value == TiposEspeciaisProduto.Veículo)
                {
                    Produto.veicProd = new VeículosNovos();
                    Produto.comb = null;
                }
                else if (value == TiposEspeciaisProduto.Combustível)
                {
                    Produto.veicProd = null;
                    Produto.comb = new Combustível();
                }
                Produto.medicamentos = new List<Medicamentos>();
                Produto.armas = new List<Armas>();
                Produto.nRECOPI = null;
                OnPropertyChanged("Produto.veicProd", "Produto.comb", "Produto.nRECOPI", "ListaMedicamentos", "ListaArmamento" );
            }
        }

        public bool VisibilidadeVeiculoNovo { get; set; }
        public bool VisibilidadeMedicamento { get; set; }
        public bool VisibilidadeArmamento { get; set; }
        public bool VisibilidadeCombustivel { get; set; }
        public bool VisibilidadePapel { get; set; }

        public ObservableCollection<Medicamentos> ListaMedicamentos
        {
            get { return Produto.medicamentos.GerarObs(); }
        }
        public Medicamentos NovoMedicamento { get; private set; }
        public int IndexMedicamentoSelecionado { get; set; }


        public ObservableCollection<Armas> ListaArmamento
        {
            get { return Produto.armas.GerarObs(); }
        }
        public Armas NovoArmamento { get; private set; }
        public int IndexArmamentoSelecionado { get; set; }

        public ProdutoEspecificoDataContext(ProdutoOuServiço prod)
        {
            Produto = prod;
            NovoMedicamento = new Medicamentos();
            NovoArmamento = new Armas();
            AdicionarMedicamentoCommand = new ComandoSemParametros(AdicionarMedicamento, true);
            RemoverMedicamentoCommand = new ComandoSemParametros(RemoverMedicamento, true);
            AdicionarArmamentoCommand = new ComandoSemParametros(AdicionarArmamento, true);
            RemoverArmamentoCommand = new ComandoSemParametros(RemoverArmamento, true);
        }

        public ICommand AdicionarMedicamentoCommand { get; }
        public ICommand RemoverMedicamentoCommand { get; }
        public ICommand AdicionarArmamentoCommand { get; }
        public ICommand RemoverArmamentoCommand { get; }

        private void AdicionarMedicamento()
        {
            Produto.medicamentos.Add(NovoMedicamento);
            OnPropertyChanged(nameof(ListaMedicamentos));
            NovoMedicamento = new Medicamentos();
            OnPropertyChanged(nameof(NovoMedicamento));
        }

        private void RemoverMedicamento()
        {
            if (IndexMedicamentoSelecionado != -1 && Produto.medicamentos.Count > 0)
            {
                Produto.medicamentos.RemoveAt(IndexMedicamentoSelecionado);
                OnPropertyChanged(nameof(ListaMedicamentos));
            }
        }

        public void AdicionarArmamento()
        {
            Produto.armas.Add(NovoArmamento);
            OnPropertyChanged(nameof(ListaArmamento));
            NovoArmamento = new Armas();
            OnPropertyChanged(nameof(NovoArmamento));
        }

        public void RemoverArmamento()
        {
            if (IndexArmamentoSelecionado != -1 && Produto.armas.Count > 0)
            {
                Produto.armas.RemoveAt(IndexArmamentoSelecionado);
                OnPropertyChanged(nameof(ListaArmamento));
            }
        }

        public enum TiposEspeciaisProduto
        {
            Simples,
            Veículo,
            Medicamento,
            Armamento,
            Combustível,
            Papel
        }
    }
}
