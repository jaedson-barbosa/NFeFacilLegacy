﻿using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.View.CaixasDialogo;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;

namespace NFeFacil.ViewModel.NotaFiscal
{
    public sealed class CanaDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public RegistroAquisicaoCana Cana { get; }

        public DateTimeOffset MesAnoReferencia
        {
            get
            {
                if (Cana.referencia == null) return DateTimeOffset.Now;
                else return DateTimeOffset.ParseExact(Cana.referencia, "MM/yyyy", CultureInfo.InvariantCulture);
            }
            set { Cana.referencia = value.ToString("MM/yyyy"); }
        }

        public ICommand AdicionarFornecimentoCommand { get; }
        public ICommand RemoverFornecimentoCommand { get; }
        public ICommand AdicionarDeducaoCommand { get; }
        public ICommand RemoverDeducaoCommand { get; }

        public async void AdicionarFornecimento()
        {
            var caixa = new View.CaixasDialogo.FornecimentoDiario();
            caixa.PrimaryButtonClick += (x, y) =>
            {
                Cana.forDia.Add(x.DataContext as ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.FornecimentoDiario);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Cana)));
            };
            await caixa.ShowAsync();
        }

        public void RemoverFornecimento(ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.FornecimentoDiario fornecimento)
        {
            Cana.forDia.Remove(fornecimento);
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Cana)));
        }

        public async void AdicionarDeducao()
        {
            var caixa = new Deducao();
            caixa.PrimaryButtonClick += (x, y) =>
            {
                Cana.deduc.Add(x.DataContext as Deducoes);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Cana)));
            };
            await caixa.ShowAsync();
        }

        public void RemoverDeducao(Deducoes deducao)
        {
            Cana.deduc.Remove(deducao);
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Cana)));
        }

        public CanaDataContext()
        {
            AdicionarFornecimentoCommand = new ComandoSemParametros(AdicionarFornecimento, true);
            RemoverFornecimentoCommand = new ComandoComParametros<ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.FornecimentoDiario, ObterDataContext<ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.FornecimentoDiario>>(RemoverFornecimento);
            AdicionarDeducaoCommand = new ComandoSemParametros(AdicionarDeducao, true);
            RemoverDeducaoCommand = new ComandoComParametros<Deducoes, ObterDataContext<Deducoes>>(RemoverDeducao);
        }
        public CanaDataContext(ref RegistroAquisicaoCana registro) : this()
        {
            Cana = registro;
        }
    }
}
