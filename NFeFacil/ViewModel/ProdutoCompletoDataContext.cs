using NFeFacil;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesProdutoOuServico;
using NFeFacil.ViewModel.ImpostosProduto;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

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

        public bool UsarCIDE
        {
            get => Comb.CIDE != null;
            set
            {
                Comb.CIDE = value ? new CIDE() : null;
                OnPropertyChanged(nameof(UsarCIDE), nameof(Comb));
            }
        }

        public ObservableCollection<TiposEspeciaisProduto> ListaTiposEspeciaisProduto => Extensoes.ObterItens<TiposEspeciaisProduto>();
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

        ICMSDataContext contextoICMS;
        public ICMSDataContext ContextoICMS
        {
            get
            {
                if (contextoICMS == null)
                {
                    contextoICMS = new ICMSDataContext();
                }
                return contextoICMS;
            }
        }

        PISDataContext contextoPIS;
        public PISDataContext ContextoPIS
        {
            get
            {
                if (contextoPIS == null)
                {
                    contextoPIS = new PISDataContext();
                }
                contextoPIS.ProdutoReferente = ProdutoCompleto.Produto;
                return contextoPIS;
            }
        }

        COFINSDataContext contextoCOFINS;
        public COFINSDataContext ContextoCOFINS
        {
            get
            {
                if (contextoCOFINS == null)
                {
                    contextoCOFINS = new COFINSDataContext();
                }
                contextoCOFINS.ProdutoReferente = ProdutoCompleto.Produto;
                return contextoCOFINS;
            }
        }

        IPIDataContext contextoIPI;
        public IPIDataContext ContextoIPI
        {
            get
            {
                if (contextoIPI == null)
                {
                    contextoIPI = new IPIDataContext();
                }
                contextoIPI.ProdutoReferente = ProdutoCompleto.Produto;
                return contextoIPI;
            }
        }

        ISSQNDataContext contextoISSQN;
        public ISSQNDataContext ContextoISSQN
        {
            get
            {
                if (contextoISSQN == null)
                {
                    contextoISSQN = new ISSQNDataContext();
                }
                return contextoISSQN;
            }
        }

        II contextoII;
        public II ContextoII
        {
            get
            {
                if (contextoII == null)
                {
                    contextoII = new II();
                }
                return contextoII;
            }
        }

        ImpostoDevol contextoImpostoDevol;
        public ImpostoDevol ContextoImpostoDevol
        {
            get
            {
                if (contextoImpostoDevol == null)
                {
                    contextoImpostoDevol = new ImpostoDevol();
                }
                return contextoImpostoDevol;
            }
        }

        ICMSUFDest contextoIcmsUFDest;
        public ICMSUFDest ContextoIcmsUFDest
        {
            get
            {
                if (contextoIcmsUFDest == null)
                {
                    contextoIcmsUFDest = new ICMSUFDest();
                }
                return contextoIcmsUFDest;
            }
        }

        public ProdutoCompletoDataContext(DetalhesProdutos produtoCompleto)
        {
            ProdutoCompleto = produtoCompleto;
            ProdutoCompleto.Produto.DadoImpostoChanged += (x, y) =>
            {
                OnPropertyChanged(nameof(ContextoPIS), nameof(ContextoCOFINS), nameof(ContextoIPI));
            };
            NovoMedicamento = new Medicamento();
            NovoArmamento = new Arma();
            AdicionarDeclaracaoImportacaoCommand = new Comando(AdicionarDeclaracaoImportacao, true);
            AdicionarDeclaracaoExportacaoCommand = new Comando(AdicionarDeclaracaoExportacao, true);
            RemoverDeclaracaoImportacaoCommand = new Comando<DeclaracaoImportacao>(RemoverDeclaracaoImportacao);
            RemoverDeclaracaoExportacaoCommand = new Comando<GrupoExportacao>(RemoverDeclaracaoExportacao);
            AdicionarMedicamentoCommand = new Comando(AdicionarMedicamento, true);
            RemoverMedicamentoCommand = new Comando(RemoverMedicamento, true);
            AdicionarArmamentoCommand = new Comando(AdicionarArmamento, true);
            RemoverArmamentoCommand = new Comando(RemoverArmamento, true);
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
            var caixa = new View.CaixasDialogo.DeclaracaoImportacao()
            {
                DataContext = new DeclaracaoImportacao()
            };
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                ProdutoCompleto.Produto.DI.Add(caixa.DataContext as DeclaracaoImportacao);
                OnPropertyChanged(nameof(ListaDI));
            }
        }

        private async void AdicionarDeclaracaoExportacao()
        {
            var caixa = new View.CaixasDialogo.DeclaracaoExportacao()
            {
                DataContext = new GrupoExportacao()
            };
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                ProdutoCompleto.Produto.GrupoExportação.Add(caixa.DataContext as GrupoExportacao);
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
