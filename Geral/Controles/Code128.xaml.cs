using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace BaseGeral.Controles
{
    public sealed partial class Code128 : UserControl
    {
        public Rectangle[] Barras
        {
            set
            {
                stkBarras.Children.Clear();
                for (int i = 0; i < value.Length; i++)
                {
                    stkBarras.Children.Add(value[i]);
                }
            }
        }

        public string Data
        {
            get => (string)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register(nameof(Data), typeof(string), typeof(Code128),
                new PropertyMetadata(string.Empty, new PropertyChangedCallback(BarcodeChanged)));

        public Code128()
        {
            InitializeComponent();
        }

        static void BarcodeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var code = (Code128)sender;
            if (!string.IsNullOrEmpty(code.Data))
            {
                var height = double.IsNaN(code.Height) ? 30 : code.Height;
                code.Barras = Encode(code.Data, height);
            }
        }

        static Rectangle[] Encode(string RawData, double height)
        {
            var formattedData = new List<string>() { "START_C" };
            string rawData = RawData.Length % 2 == 0 ? RawData : "0" + RawData;
            var i = 0;
            for (i = 0; i < rawData.Length; i += 2)
            {
                formattedData.Add(rawData.Substring(i, 2));
            }

            i = 0;
            int sum = formattedData.Sum(x => int.Parse(Find(x, 0)) * (i == 0 ? i = 1 : i++));
            formattedData.Add((sum % 103).ToString("00"));
            formattedData.Add("STOP");

            var EncodedValue = string.Concat(formattedData.Select(s => Find(s, 2)));
            var bars = new Rectangle[EncodedValue.Length];
            for (int pos = 0; pos < EncodedValue.Length; pos++)
            {
                bars[pos] = new Rectangle
                {
                    Fill = new SolidColorBrush(EncodedValue[pos] == '1' ? Colors.Black : Colors.White),
                    Width = 1,
                    Height = height,
                    UseLayoutRounding = false
                };
            }
            return bars;

            string Find(string str, int colunaRetornada)
            {
                for (int k = 0; k < 107; k++)
                {
                    if (C128Code[k, 1] == str)
                        return C128Code[k, colunaRetornada];
                }
                for (int k = 0; k < 107; k++)
                {
                    if (C128Code[k, 0] == str)
                        return C128Code[k, colunaRetornada];
                }
                throw new Exception($"EC128-5: Could not find encoding of a value in C128 type C");
            }
        }

        //As colunas são Value, C e Encoding
        static string[,] C128Code = new string[107, 3]
        {
            { "0", "00", "11011001100" },
            { "1", "01", "11001101100" },
            { "2", "02", "11001100110" },
            { "3", "03", "10010011000" },
            { "4", "04", "10010001100" },
            { "5", "05", "10001001100" },
            { "6", "06", "10011001000" },
            { "7", "07", "10011000100" },
            { "8", "08", "10001100100" },
            { "9", "09", "11001001000" },
            { "10", "10", "11001000100" },
            { "11", "11", "11000100100" },
            { "12", "12", "10110011100" },
            { "13", "13", "10011011100" },
            { "14", "14", "10011001110" },
            { "15", "15", "10111001100" },
            { "16", "16", "10011101100" },
            { "17", "17", "10011100110" },
            { "18", "18", "11001110010" },
            { "19", "19", "11001011100" },
            { "20", "20", "11001001110" },
            { "21", "21", "11011100100" },
            { "22", "22", "11001110100" },
            { "23", "23", "11101101110" },
            { "24", "24", "11101001100" },
            { "25", "25", "11100101100" },
            { "26", "26", "11100100110" },
            { "27", "27", "11101100100" },
            { "28", "28", "11100110100" },
            { "29", "29", "11100110010" },
            { "30", "30", "11011011000" },
            { "31", "31", "11011000110" },
            { "32", "32", "11000110110" },
            { "33", "33", "10100011000" },
            { "34", "34", "10001011000" },
            { "35", "35", "10001000110" },
            { "36", "36", "10110001000" },
            { "37", "37", "10001101000" },
            { "38", "38", "10001100010" },
            { "39", "39", "11010001000" },
            { "40", "40", "11000101000" },
            { "41", "41", "11000100010" },
            { "42", "42", "10110111000" },
            { "43", "43", "10110001110" },
            { "44", "44", "10001101110" },
            { "45", "45", "10111011000" },
            { "46", "46", "10111000110" },
            { "47", "47", "10001110110" },
            { "48", "48", "11101110110" },
            { "49", "49", "11010001110" },
            { "50", "50", "11000101110" },
            { "51", "51", "11011101000" },
            { "52", "52", "11011100010" },
            { "53", "53", "11011101110" },
            { "54", "54", "11101011000" },
            { "55", "55", "11101000110" },
            { "56", "56", "11100010110" },
            { "57", "57", "11101101000" },
            { "58", "58", "11101100010" },
            { "59", "59", "11100011010" },
            { "60", "60", "11101111010" },
            { "61", "61", "11001000010" },
            { "62", "62", "11110001010" },
            { "63", "63", "10100110000" },
            { "64", "64", "10100001100" },
            { "65", "65", "10010110000" },
            { "66", "66", "10010000110" },
            { "67", "67", "10000101100" },
            { "68", "68", "10000100110" },
            { "69", "69", "10110010000" },
            { "70", "70", "10110000100" },
            { "71", "71", "10011010000" },
            { "72", "72", "10011000010" },
            { "73", "73", "10000110100" },
            { "74", "74", "10000110010" },
            { "75", "75", "11000010010" },
            { "76", "76", "11001010000" },
            { "77", "77", "11110111010" },
            { "78", "78", "11000010100" },
            { "79", "79", "10001111010" },
            { "80", "80", "10100111100" },
            { "81", "81", "10010111100" },
            { "82", "82", "10010011110" },
            { "83", "83", "10111100100" },
            { "84", "84", "10011110100" },
            { "85", "85", "10011110010" },
            { "86", "86", "11110100100" },
            { "87", "87", "11110010100" },
            { "88", "88", "11110010010" },
            { "89", "89", "11011011110" },
            { "90", "90", "11011110110" },
            { "91", "91", "11110110110" },
            { "92", "92", "10101111000" },
            { "93", "93", "10100011110" },
            { "94", "94", "10001011110" },
            { "95", "95", "10111101000" },
            { "96", "96", "10111100010" },
            { "97", "97", "11110101000" },
            { "98", "98", "11110100010" },
            { "99", "99", "10111011110" },
            { "100", "CODE_B", "10111101110" },
            { "101", "CODE_A", "11101011110" },
            { "102", Convert.ToChar(200).ToString()/*FNC1*/, "11110101110" },
            { "103", "START_A", "11010000100" },
            { "104", "START_B", "11010010000" },
            { "105", "START_C", "11010011100" },
            { "", "STOP", "1100011101011" }
        };
    }
}
