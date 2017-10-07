﻿using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static NFeFacil.ExtensoesPrincipal;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.DANFE.PartesDANFE
{
    public sealed partial class Canhoto : UserControl
    {
        GridLength ColunaMeio => CentimeterToLength(10);

        public Canhoto()
        {
            InitializeComponent();
        }
    }
}