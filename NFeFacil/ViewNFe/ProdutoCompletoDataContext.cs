using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesProdutoOuServico;
using NFeFacil.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace NFeFacil.ViewNFe
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

        public VeiculoNovo Veiculo
        {
            get { return ProdutoCompleto.Produto.veicProd ?? (ProdutoCompleto.Produto.veicProd = new VeiculoNovo()); }
        }

        public Combustivel Comb
        {
            get { return ProdutoCompleto.Produto.comb ?? (ProdutoCompleto.Produto.comb = new Combustivel()); }
        }

        public bool UsarCIDE
        {
            get => Comb.CIDE != null;
            set
            {
                Comb.CIDE = value ? new CIDE() : null;
                OnPropertyChanged(nameof(UsarCIDE), nameof(Comb));
            }
        }

        public ObservableCollection<TiposEspeciaisProduto> ListaTiposEspeciaisProduto => ExtensoesPrincipal.ObterItens<TiposEspeciaisProduto>();
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

        public ObservableCollection<Arma> ListaArmamento
        {
            get { return ProdutoCompleto.Produto.armas.GerarObs(); }
        }

        public ImpostoDevol ContextoImpostoDevol
        {
            get
            {
                if (ProdutoCompleto.ImpostoDevol == null)
                {
                    ProdutoCompleto.ImpostoDevol = new ImpostoDevol();
                }
                return ProdutoCompleto.ImpostoDevol;
            }
        }

        public ProdutoCompletoDataContext(DetalhesProdutos produtoCompleto)
        {
            ProdutoCompleto = produtoCompleto;

            AdicionarDeclaracaoImportacaoCommand = new Comando(AdicionarDeclaracaoImportacao);
            AdicionarDeclaracaoExportacaoCommand = new Comando(AdicionarDeclaracaoExportacao);
            RemoverDeclaracaoImportacaoCommand = new Comando<DeclaracaoImportacao>(RemoverDeclaracaoImportacao);
            RemoverDeclaracaoExportacaoCommand = new Comando<GrupoExportacao>(RemoverDeclaracaoExportacao);
            AdicionarMedicamentoCommand = new Comando(AdicionarMedicamento);
            RemoverMedicamentoCommand = new Comando<Medicamento>(RemoverMedicamento);
            AdicionarArmamentoCommand = new Comando(AdicionarArmamento);
            RemoverArmamentoCommand = new Comando<Arma>(RemoverArmamento);
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
            var caixa = new CaixasDialogoProduto.AdicionarDeclaracaoImportacao();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                ProdutoCompleto.Produto.DI.Add(caixa.Declaracao);
                OnPropertyChanged(nameof(ListaDI));
            }
        }

        private async void AdicionarDeclaracaoExportacao()
        {
            var caixa = new CaixasDialogoProduto.EscolherTipoDeclaracaoExportacao();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var tipo = caixa.TipoEscolhido;
                if (tipo == CaixasDialogoProduto.TiposDeclaracaoExportacao.Direta)
                {
                    var caixa2 = new CaixasDialogoProduto.AddDeclaracaoExportacaoDireta();
                    if (await caixa2.ShowAsync() == ContentDialogResult.Primary)
                    {
                        ProdutoCompleto.Produto.GrupoExportação.Add(caixa2.Declaracao);
                    }
                }
                else
                {
                    var caixa2 = new CaixasDialogoProduto.AddDeclaracaoExportacaoIndireta();
                    if (await caixa2.ShowAsync() == ContentDialogResult.Primary)
                    {
                        ProdutoCompleto.Produto.GrupoExportação.Add(caixa2.Declaracao);
                    }
                }
                OnPropertyChanged(nameof(ListaGE));
            }
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

        async void AdicionarMedicamento()
        {
            var caixa = new CaixasDialogoProduto.AdicionarMedicamento();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var novoMedicamento = (Medicamento)caixa.DataContext;
                ProdutoCompleto.Produto.medicamentos.Add(novoMedicamento);
                OnPropertyChanged(nameof(ListaMedicamentos));
            }
        }

        private void RemoverMedicamento(Medicamento med)
        {
            ProdutoCompleto.Produto.medicamentos.Remove(med);
            OnPropertyChanged(nameof(ListaMedicamentos));
        }

        async void AdicionarArmamento()
        {
            var caixa = new CaixasDialogoProduto.AdicionarArmamento();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var novoArmamento = (Arma)caixa.DataContext;
                ProdutoCompleto.Produto.armas.Add(novoArmamento);
                OnPropertyChanged(nameof(ListaArmamento));
            }
        }

        public void RemoverArmamento(Arma arma)
        {
            ProdutoCompleto.Produto.armas.Remove(arma);
            OnPropertyChanged(nameof(ListaArmamento));
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
