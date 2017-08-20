using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesProdutoOuServico;
using NFeFacil.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml.Data;

namespace NFeFacil.View.CaixasDialogo
{
    public sealed class ConverterContextoDeclaracaoImportacao : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesProdutoOuServico.DeclaracaoImportacao dec)
            {
                return new DeclaracaoImportacaoDataContext(ref dec);
            }
            throw new ArgumentException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is DeclaracaoImportacaoDataContext contexto)
            {
                return contexto.Declaracao;
            }
            throw new ArgumentException();
        }

        private sealed class DeclaracaoImportacaoDataContext : INotifyPropertyChanged
        {
            public NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesProdutoOuServico.DeclaracaoImportacao Declaracao { get; }

            public DateTimeOffset dataRegistro
            {
                get
                {
                    if (Declaracao.DDI == null) Declaracao.DDI = DateTimeOffset.Now.ToString("yyyy-MM-dd");
                    return DateTimeOffset.Parse(Declaracao.DDI);
                }
                set { Declaracao.DDI = value.ToString("yyyy-MM-dd"); }
            }

            public DateTimeOffset dataDesembaraco
            {
                get
                {
                    if (Declaracao.DDesemb == null) Declaracao.DDesemb = DateTimeOffset.Now.ToString("yyyy-MM-dd");
                    return DateTimeOffset.Parse(Declaracao.DDesemb);
                }
                set { Declaracao.DDesemb = value.ToString("yyyy-MM-dd"); }
            }

            public int transpInternacional
            {
                get { return Declaracao.TpViaTransp - 1; }
                set { Declaracao.TpViaTransp = (ushort)(value + 1); }
            }

            public int tipoImportacao
            {
                get { return Declaracao.TpIntermedio - 1; }
                set { Declaracao.TpIntermedio = (ushort)(value + 1); }
            }

            public ObservableCollection<DIAdicao> Adicoes
            {
                get { return Declaracao.Adi.GerarObs(); }
            }
            public DIAdicao NovaAdicao { get; private set; }
            public int IndexAdicaoSelecionada { get; set; }

            public ICommand AdicionarAdicaoCommand { get; }
            public ICommand RemoverAdicaoCommand { get; }

            public event PropertyChangedEventHandler PropertyChanged;

            private void AdicionarAdicao()
            {
                NovaAdicao.NSeqAdic = Adicoes.Count(x => x.NAdicao == NovaAdicao.NAdicao) + 1;
                Declaracao.Adi.Add(NovaAdicao);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Adicoes)));
                NovaAdicao = new DIAdicao();
            }

            private void RemoverAdicao()
            {
                if (IndexAdicaoSelecionada != -1 && Declaracao.Adi.Count > 0)
                {
                    Declaracao.Adi.RemoveAt(IndexAdicaoSelecionada);
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Adicoes)));
                }
            }

            public DeclaracaoImportacaoDataContext(ref NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesProdutoOuServico.DeclaracaoImportacao dec) : base()
            {
                Declaracao = dec;
                NovaAdicao = new DIAdicao();
                AdicionarAdicaoCommand = new Comando(AdicionarAdicao, true);
                RemoverAdicaoCommand = new Comando(RemoverAdicao, true);
            }
        }
    }
}
