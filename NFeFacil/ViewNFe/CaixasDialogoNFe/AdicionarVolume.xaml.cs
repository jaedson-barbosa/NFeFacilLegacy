﻿using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item da Caixa de Diálogo de Conteúdo está documentado em http://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.CaixasDialogoNFe
{
    public sealed partial class AdicionarVolume : ContentDialog
    {
        public Volume vol => DataContext as Volume;
        public AdicionarVolume()
        {
            InitializeComponent();
            DataContext = new Volume();
        }

        private void btnAddLacre_Click(object sender, RoutedEventArgs e)
        {
            vol.Lacres.Add(new Lacre { NLacre = intLacre.Text });
            lstLacres.ItemsSource = new ObservableCollection<Lacre>(vol.Lacres);
            intLacre.Text = "";
        }

        private void btnDelLacre_Click(object sender, RoutedEventArgs e)
        {
            if (lstLacres.SelectedIndex != -1)
            {
                vol.Lacres.RemoveAt(lstLacres.SelectedIndex);
                lstLacres.ItemsSource = new ObservableCollection<Lacre>(vol.Lacres);
            }
        }
    }
}