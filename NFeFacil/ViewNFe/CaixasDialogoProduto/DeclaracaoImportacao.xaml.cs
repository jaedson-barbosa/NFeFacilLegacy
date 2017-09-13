using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesProdutoOuServico;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item da Caixa de Diálogo de Conteúdo está documentado em http://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.CaixasDialogoProduto
{
    public sealed partial class AdicionarDeclaracaoImportacao : ContentDialog
    {
        public AdicionarDeclaracaoImportacao()
        {
            InitializeComponent();
            Declaracao = new DeclaracaoImportacao();
            NovaAdicao = new DIAdicao();
            Adicoes = Declaracao.Adi.GerarObs();
        }

        public DeclaracaoImportacao Declaracao { get; }

        DateTimeOffset dataRegistro
        {
            get
            {
                if (Declaracao.DDI == null) Declaracao.DDI = DateTimeOffset.Now.ToString("yyyy-MM-dd");
                return DateTimeOffset.Parse(Declaracao.DDI);
            }
            set { Declaracao.DDI = value.ToString("yyyy-MM-dd"); }
        }

        DateTimeOffset dataDesembaraco
        {
            get
            {
                if (Declaracao.DDesemb == null) Declaracao.DDesemb = DateTimeOffset.Now.ToString("yyyy-MM-dd");
                return DateTimeOffset.Parse(Declaracao.DDesemb);
            }
            set { Declaracao.DDesemb = value.ToString("yyyy-MM-dd"); }
        }

        int transpInternacional
        {
            get { return Declaracao.TpViaTransp - 1; }
            set { Declaracao.TpViaTransp = (ushort)(value + 1); }
        }

        int tipoImportacao
        {
            get { return Declaracao.TpIntermedio - 1; }
            set { Declaracao.TpIntermedio = (ushort)(value + 1); }
        }

        ObservableCollection<DIAdicao> Adicoes { get; }
        DIAdicao NovaAdicao { get; set; }

        void Adicionar(object sender, RoutedEventArgs e)
        {
            NovaAdicao.NSeqAdic = Adicoes.Count(x => x.NAdicao == NovaAdicao.NAdicao) + 1;
            Declaracao.Adi.Add(NovaAdicao);
            Adicoes.Add(NovaAdicao);
            NovaAdicao = new DIAdicao();
        }

        void Remover(object sender, RoutedEventArgs e)
        {
            var adicao = (DIAdicao)((FrameworkElement)sender).DataContext;
            Declaracao.Adi.Remove(adicao);
            Adicoes.Remove(adicao);
        }
    }
}
