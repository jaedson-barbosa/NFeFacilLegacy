﻿using NFeFacil.Log;
using NFeFacil.Validacao;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System;
using NFeFacil.ItensBD;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class AdicionarMotorista : Page
    {
        private MotoristaDI motorista;
        private ILog Log = Popup.Current;

        public AdicionarMotorista()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter == null)
            {
                motorista = new MotoristaDI();
                MainPage.Current.SeAtualizar(Symbol.Add, "Motorista");
            }
            else
            {
                motorista = (MotoristaDI)e.Parameter;
                MainPage.Current.SeAtualizar(Symbol.Edit, "Motorista");
            }
            DataContext = new MotoristaDataContext(ref motorista);
        }

        private void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (new ValidadorMotorista(motorista).Validar(Log))
                {
                    using (var db = new AplicativoContext())
                    {
                        motorista.UltimaData = DateTime.Now;
                        if (motorista.Id == Guid.Empty)
                        {
                            db.Add(motorista);
                            Log.Escrever(TitulosComuns.Sucesso, "Motorista salvo com sucesso.");
                        }
                        else
                        {
                            db.Update(motorista);
                            Log.Escrever(TitulosComuns.Sucesso, "Motorista alterado com sucesso.");
                        }
                        db.SaveChanges();
                    }
                    MainPage.Current.Retornar();
                }
            }
            catch (Exception erro)
            {
                erro.ManipularErro();
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Current.Retornar();
        }
    }
}