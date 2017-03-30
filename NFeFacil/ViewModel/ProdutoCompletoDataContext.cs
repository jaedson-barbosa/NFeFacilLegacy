using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesProdutoOuServico;
using NFeFacil.ViewModel.PartesProdutoCompleto;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NFeFacil.ViewModel
{
    public sealed class ProdutoCompletoDataContext : INotifyPropertyChanged
    {
        public DetalhesProdutos ProdutoCompleto { get; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(params string[] parametros)
        {
            for (int i = 0; i < parametros.Length; i++)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(parametros[i]));
            }
        }

        public ObservableCollection<DeclaracaoImportacao> ListaDI => ProdutoCompleto.Produto.DI.GerarObs();
        public ObservableCollection<GrupoExportacao> ListaGE => ProdutoCompleto.Produto.GrupoExportação.GerarObs();

        public bool AtivadopvtICMS { get; private set; } = true;
        public bool AtivadopvtISSQN { get; private set; } = false;
        public bool AtivadopvtICMSInterestadual { get; private set; } = true;

        private bool tipoICMSSelecionado = true;
        public bool TipoICMSSelecionado
        {
            get => tipoICMSSelecionado;
            set
            {
                tipoICMSSelecionado = value;
                AtivadopvtICMS = AtivadopvtICMSInterestadual = value;
                AtivadopvtISSQN = !value;
                OnPropertyChanged(nameof(AtivadopvtICMS), nameof(AtivadopvtICMSInterestadual), nameof(AtivadopvtISSQN));
            }
        }

        public VeiculoNovo Veiculo
        {
            get { return ProdutoCompleto.Produto.veicProd ?? (ProdutoCompleto.Produto.veicProd = new VeiculoNovo()); }
        }

        public Combustivel Comb
        {
            get { return ProdutoCompleto.Produto.comb ?? (ProdutoCompleto.Produto.comb = new Combustivel()); }
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
                    ProdutoCompleto.Produto.veicProd = new VeiculoNovo();
                    ProdutoCompleto.Produto.comb = null;
                }
                else if (value == TiposEspeciaisProduto.Combustível)
                {
                    ProdutoCompleto.Produto.veicProd = null;
                    ProdutoCompleto.Produto.comb = new Combustivel();
                }
                ProdutoCompleto.Produto.medicamentos = new List<Medicamento>();
                ProdutoCompleto.Produto.armas = new List<Arma>();
                ProdutoCompleto.Produto.NRECOPI = null;
                OnPropertyChanged("Produto.veicProd", "Produto.comb", "Produto.nRECOPI", "ListaMedicamentos", "ListaArmamento");
            }
        }

        public bool VisibilidadeVeiculoNovo { get; private set; }
        public bool VisibilidadeMedicamento { get; private set; }
        public bool VisibilidadeArmamento { get; private set; }
        public bool VisibilidadeCombustivel { get; private set; }
        public bool VisibilidadePapel { get; private set; }

        public ObservableCollection<Medicamento> ListaMedicamentos
        {
            get { return ProdutoCompleto.Produto.medicamentos.GerarObs(); }
        }
        public Medicamento NovoMedicamento { get; private set; }
        public int IndexMedicamentoSelecionado { get; set; }


        public ObservableCollection<Arma> ListaArmamento
        {
            get { return ProdutoCompleto.Produto.armas.GerarObs(); }
        }
        public Arma NovoArmamento { get; private set; }
        public int IndexArmamentoSelecionado { get; set; }

        public ProdutoCompletoDataContext(DetalhesProdutos produtoCompleto)
        {
            ProdutoCompleto = produtoCompleto;
            NovoMedicamento = new Medicamento();
            NovoArmamento = new Arma();
            AdicionarDeclaracaoImportacaoCommand = new ComandoSemParametros(AdicionarDeclaracaoImportacao, true);
            AdicionarDeclaracaoExportacaoCommand = new ComandoSemParametros(AdicionarDeclaracaoExportacao, true);
            RemoverDeclaracaoImportacaoCommand = new ComandoComParametros<DeclaracaoImportacao, ObterDataContext<DeclaracaoImportacao>>(RemoverDeclaracaoImportacao);
            RemoverDeclaracaoExportacaoCommand = new ComandoComParametros<GrupoExportacao, ObterDataContext<GrupoExportacao>>(RemoverDeclaracaoExportacao);
            AdicionarMedicamentoCommand = new ComandoSemParametros(AdicionarMedicamento, true);
            RemoverMedicamentoCommand = new ComandoSemParametros(RemoverMedicamento, true);
            AdicionarArmamentoCommand = new ComandoSemParametros(AdicionarArmamento, true);
            RemoverArmamentoCommand = new ComandoSemParametros(RemoverArmamento, true);
        }

        public ICommand AdicionarDeclaracaoImportacaoCommand { get; }
        public ICommand AdicionarDeclaracaoExportacaoCommand { get; }
        public ICommand RemoverDeclaracaoImportacaoCommand { get; }
        public ICommand RemoverDeclaracaoExportacaoCommand { get; }
        public ICommand AdicionarMedicamentoCommand { get; }
        public ICommand RemoverMedicamentoCommand { get; }
        public ICommand AdicionarArmamentoCommand { get; }
        public ICommand RemoverArmamentoCommand { get; }

        private async void AdicionarDeclaracaoImportacao()
        {
            var caixa = new View.CaixasDialogo.DeclaracaoImportacao();
            caixa.PrimaryButtonClick += (x, y) =>
            {
                var data = x.DataContext as DeclaracaoImportacaoDataContext;
                ProdutoCompleto.Produto.DI.Add(data.Declaracao);
                OnPropertyChanged(nameof(ListaDI));
            };
            await caixa.ShowAsync();
        }

        private async void AdicionarDeclaracaoExportacao()
        {
            var caixa = new View.CaixasDialogo.DeclaracaoExportacao();
            caixa.PrimaryButtonClick += (x, y) =>
            {
                var data = x.DataContext as DeclaracaoExportacaoDataContext;
                ProdutoCompleto.Produto.GrupoExportação.Add(data.Declaracao);
                OnPropertyChanged(nameof(ListaGE));
            };
            await caixa.ShowAsync();
        }

        private void RemoverDeclaracaoImportacao(DeclaracaoImportacao declaracao)
        {
            ProdutoCompleto.Produto.DI.Remove(declaracao);
            OnPropertyChanged(nameof(ListaDI));
        }

        private void RemoverDeclaracaoExportacao(GrupoExportacao declaracao)
        {
            ProdutoCompleto.Produto.GrupoExportação.Remove(declaracao);
            OnPropertyChanged(nameof(ListaGE));
        }

        private void AdicionarMedicamento()
        {
            ProdutoCompleto.Produto.medicamentos.Add(NovoMedicamento);
            OnPropertyChanged(nameof(ListaMedicamentos));
            NovoMedicamento = new Medicamento();
            OnPropertyChanged(nameof(NovoMedicamento));
        }

        private void RemoverMedicamento()
        {
            if (IndexMedicamentoSelecionado != -1 && ProdutoCompleto.Produto.medicamentos.Count > 0)
            {
                ProdutoCompleto.Produto.medicamentos.RemoveAt(IndexMedicamentoSelecionado);
                OnPropertyChanged(nameof(ListaMedicamentos));
            }
        }

        public void AdicionarArmamento()
        {
            ProdutoCompleto.Produto.armas.Add(NovoArmamento);
            OnPropertyChanged(nameof(ListaArmamento));
            NovoArmamento = new Arma();
            OnPropertyChanged(nameof(NovoArmamento));
        }

        public void RemoverArmamento()
        {
            if (IndexArmamentoSelecionado != -1 && ProdutoCompleto.Produto.armas.Count > 0)
            {
                ProdutoCompleto.Produto.armas.RemoveAt(IndexArmamentoSelecionado);
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
