using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace NFeFacil.ViewModel
{
    public class DeclaracaoImportaçãoDataContext : INotifyPropertyChanged
    {
        public DeclaraçãoImportação Declaracao { get; }

        public DateTimeOffset dataRegistro
        {
            get
            {
                if (Declaracao.dDI == null) Declaracao.dDI = DateTimeOffset.Now.ToString("yyyy-MM-dd");
                return DateTimeOffset.Parse(Declaracao.dDI);
            }
            set { Declaracao.dDI = value.ToString("yyyy-MM-dd"); }
        }

        public DateTimeOffset dataDesembaraco
        {
            get
            {
                if (Declaracao.dDesemb == null) Declaracao.dDesemb = DateTimeOffset.Now.ToString("yyyy-MM-dd");
                return DateTimeOffset.Parse(Declaracao.dDesemb);
            }
            set { Declaracao.dDesemb = value.ToString("yyyy-MM-dd"); }
        }

        public int transpInternacional
        {
            get { return Declaracao.tpViaTransp - 1; }
            set { Declaracao.tpViaTransp = (ushort)(value + 1); }
        }

        public int tipoImportacao
        {
            get { return Declaracao.tpIntermedio - 1; }
            set { Declaracao.tpIntermedio = (ushort)(value + 1); }
        }

        public ObservableCollection<DIAdição> Adicoes
        {
            get { return Declaracao.adi.GerarObs(); }
        }
        public DIAdição NovaAdicao { get; private set; }
        public int IndexAdicaoSelecionada { get; set; }

        public ICommand AdicionarAdicaoCommand { get; }
        public ICommand RemoverAdicaoCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void AdicionarAdicao()
        {
            NovaAdicao.nSeqAdic = Adicoes.Count(x => x.nAdicao == NovaAdicao.nAdicao) + 1;
            Declaracao.adi.Add(NovaAdicao);
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Adicoes)));
            NovaAdicao = new DIAdição();
        }

        private void RemoverAdicao()
        {
            if (IndexAdicaoSelecionada != -1 && Declaracao.adi.Count > 0)
            {
                Declaracao.adi.RemoveAt(IndexAdicaoSelecionada);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Adicoes)));
            }
        }

        public DeclaracaoImportaçãoDataContext()
        {
            Declaracao = new DeclaraçãoImportação();
            NovaAdicao = new DIAdição();
            AdicionarAdicaoCommand = new ComandoSemParametros(AdicionarAdicao, true);
            RemoverAdicaoCommand = new ComandoSemParametros(RemoverAdicao, true);
        }
        public DeclaracaoImportaçãoDataContext(ref DeclaraçãoImportação dec) : base()
        {
            Declaracao = dec;
            NovaAdicao = new DIAdição();
            AdicionarAdicaoCommand = new ComandoSemParametros(AdicionarAdicao, true);
            RemoverAdicaoCommand = new ComandoSemParametros(RemoverAdicao, true);
        }
    }
}
