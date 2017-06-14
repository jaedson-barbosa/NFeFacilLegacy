using System;
using System.Collections.Generic;
using System.Linq;

namespace CodigoBarras.Symbologies
{
    internal class Code128 : BarcodeCommon, IBarcode
    {
        private List<string[]> C128Code = new List<string[]>();
        private List<string> _FormattedData = new List<string>();
        private Code128Types type;

        public string EncodedValue
        {
            get
            {
                Init_Code128();
                return GetEncoding();
            }
        }

        /// <summary>
        /// Encodes data in Code128 format.
        /// </summary>
        /// <param name="input">Data to encode.</param>
        /// <param name="type">Type of encoding to lock to. (Code 128A, Code 128B, Code 128C)</param>
        public Code128(string input, Code128Types type) : base(input)
        {
            this.type = type;
        }

        private void Init_Code128()
        {
            //As colunas são Value, A, B, C e Encoding
            C128Code.Add(new string[] { "0", " ", " ", "00", "11011001100" });
            C128Code.Add(new string[] { "1", "!", "!", "01", "11001101100" });
            C128Code.Add(new string[] { "2", "\"", "\"", "02", "11001100110" });
            C128Code.Add(new string[] { "3", "#", "#", "03", "10010011000" });
            C128Code.Add(new string[] { "4", "$", "$", "04", "10010001100" });
            C128Code.Add(new string[] { "5", "%", "%", "05", "10001001100" });
            C128Code.Add(new string[] { "6", "&", "&", "06", "10011001000" });
            C128Code.Add(new string[] { "7", "'", "'", "07", "10011000100" });
            C128Code.Add(new string[] { "8", "(", "(", "08", "10001100100" });
            C128Code.Add(new string[] { "9", ")", ")", "09", "11001001000" });
            C128Code.Add(new string[] { "10", "*", "*", "10", "11001000100" });
            C128Code.Add(new string[] { "11", "+", "+", "11", "11000100100" });
            C128Code.Add(new string[] { "12", ",", ",", "12", "10110011100" });
            C128Code.Add(new string[] { "13", "-", "-", "13", "10011011100" });
            C128Code.Add(new string[] { "14", ".", ".", "14", "10011001110" });
            C128Code.Add(new string[] { "15", "/", "/", "15", "10111001100" });
            C128Code.Add(new string[] { "16", "0", "0", "16", "10011101100" });
            C128Code.Add(new string[] { "17", "1", "1", "17", "10011100110" });
            C128Code.Add(new string[] { "18", "2", "2", "18", "11001110010" });
            C128Code.Add(new string[] { "19", "3", "3", "19", "11001011100" });
            C128Code.Add(new string[] { "20", "4", "4", "20", "11001001110" });
            C128Code.Add(new string[] { "21", "5", "5", "21", "11011100100" });
            C128Code.Add(new string[] { "22", "6", "6", "22", "11001110100" });
            C128Code.Add(new string[] { "23", "7", "7", "23", "11101101110" });
            C128Code.Add(new string[] { "24", "8", "8", "24", "11101001100" });
            C128Code.Add(new string[] { "25", "9", "9", "25", "11100101100" });
            C128Code.Add(new string[] { "26", ":", ":", "26", "11100100110" });
            C128Code.Add(new string[] { "27", ";", ";", "27", "11101100100" });
            C128Code.Add(new string[] { "28", "<", "<", "28", "11100110100" });
            C128Code.Add(new string[] { "29", "=", "=", "29", "11100110010" });
            C128Code.Add(new string[] { "30", ">", ">", "30", "11011011000" });
            C128Code.Add(new string[] { "31", "?", "?", "31", "11011000110" });
            C128Code.Add(new string[] { "32", "@", "@", "32", "11000110110" });
            C128Code.Add(new string[] { "33", "A", "A", "33", "10100011000" });
            C128Code.Add(new string[] { "34", "B", "B", "34", "10001011000" });
            C128Code.Add(new string[] { "35", "C", "C", "35", "10001000110" });
            C128Code.Add(new string[] { "36", "D", "D", "36", "10110001000" });
            C128Code.Add(new string[] { "37", "E", "E", "37", "10001101000" });
            C128Code.Add(new string[] { "38", "F", "F", "38", "10001100010" });
            C128Code.Add(new string[] { "39", "G", "G", "39", "11010001000" });
            C128Code.Add(new string[] { "40", "H", "H", "40", "11000101000" });
            C128Code.Add(new string[] { "41", "I", "I", "41", "11000100010" });
            C128Code.Add(new string[] { "42", "J", "J", "42", "10110111000" });
            C128Code.Add(new string[] { "43", "K", "K", "43", "10110001110" });
            C128Code.Add(new string[] { "44", "L", "L", "44", "10001101110" });
            C128Code.Add(new string[] { "45", "M", "M", "45", "10111011000" });
            C128Code.Add(new string[] { "46", "N", "N", "46", "10111000110" });
            C128Code.Add(new string[] { "47", "O", "O", "47", "10001110110" });
            C128Code.Add(new string[] { "48", "P", "P", "48", "11101110110" });
            C128Code.Add(new string[] { "49", "Q", "Q", "49", "11010001110" });
            C128Code.Add(new string[] { "50", "R", "R", "50", "11000101110" });
            C128Code.Add(new string[] { "51", "S", "S", "51", "11011101000" });
            C128Code.Add(new string[] { "52", "T", "T", "52", "11011100010" });
            C128Code.Add(new string[] { "53", "U", "U", "53", "11011101110" });
            C128Code.Add(new string[] { "54", "V", "V", "54", "11101011000" });
            C128Code.Add(new string[] { "55", "W", "W", "55", "11101000110" });
            C128Code.Add(new string[] { "56", "X", "X", "56", "11100010110" });
            C128Code.Add(new string[] { "57", "Y", "Y", "57", "11101101000" });
            C128Code.Add(new string[] { "58", "Z", "Z", "58", "11101100010" });
            C128Code.Add(new string[] { "59", "[", "[", "59", "11100011010" });
            C128Code.Add(new string[] { "60", @"\", @"\", "60", "11101111010" });
            C128Code.Add(new string[] { "61", "]", "]", "61", "11001000010" });
            C128Code.Add(new string[] { "62", "^", "^", "62", "11110001010" });
            C128Code.Add(new string[] { "63", "_", "_", "63", "10100110000" });
            C128Code.Add(new string[] { "64", "\0", "`", "64", "10100001100" });
            C128Code.Add(new string[] { "65", Convert.ToChar(1).ToString(), "a", "65", "10010110000" });
            C128Code.Add(new string[] { "66", Convert.ToChar(2).ToString(), "b", "66", "10010000110" });
            C128Code.Add(new string[] { "67", Convert.ToChar(3).ToString(), "c", "67", "10000101100" });
            C128Code.Add(new string[] { "68", Convert.ToChar(4).ToString(), "d", "68", "10000100110" });
            C128Code.Add(new string[] { "69", Convert.ToChar(5).ToString(), "e", "69", "10110010000" });
            C128Code.Add(new string[] { "70", Convert.ToChar(6).ToString(), "f", "70", "10110000100" });
            C128Code.Add(new string[] { "71", Convert.ToChar(7).ToString(), "g", "71", "10011010000" });
            C128Code.Add(new string[] { "72", Convert.ToChar(8).ToString(), "h", "72", "10011000010" });
            C128Code.Add(new string[] { "73", Convert.ToChar(9).ToString(), "i", "73", "10000110100" });
            C128Code.Add(new string[] { "74", Convert.ToChar(10).ToString(), "j", "74", "10000110010" });
            C128Code.Add(new string[] { "75", Convert.ToChar(11).ToString(), "k", "75", "11000010010" });
            C128Code.Add(new string[] { "76", Convert.ToChar(12).ToString(), "l", "76", "11001010000" });
            C128Code.Add(new string[] { "77", Convert.ToChar(13).ToString(), "m", "77", "11110111010" });
            C128Code.Add(new string[] { "78", Convert.ToChar(14).ToString(), "n", "78", "11000010100" });
            C128Code.Add(new string[] { "79", Convert.ToChar(15).ToString(), "o", "79", "10001111010" });
            C128Code.Add(new string[] { "80", Convert.ToChar(16).ToString(), "p", "80", "10100111100" });
            C128Code.Add(new string[] { "81", Convert.ToChar(17).ToString(), "q", "81", "10010111100" });
            C128Code.Add(new string[] { "82", Convert.ToChar(18).ToString(), "r", "82", "10010011110" });
            C128Code.Add(new string[] { "83", Convert.ToChar(19).ToString(), "s", "83", "10111100100" });
            C128Code.Add(new string[] { "84", Convert.ToChar(20).ToString(), "t", "84", "10011110100" });
            C128Code.Add(new string[] { "85", Convert.ToChar(21).ToString(), "u", "85", "10011110010" });
            C128Code.Add(new string[] { "86", Convert.ToChar(22).ToString(), "v", "86", "11110100100" });
            C128Code.Add(new string[] { "87", Convert.ToChar(23).ToString(), "w", "87", "11110010100" });
            C128Code.Add(new string[] { "88", Convert.ToChar(24).ToString(), "x", "88", "11110010010" });
            C128Code.Add(new string[] { "89", Convert.ToChar(25).ToString(), "y", "89", "11011011110" });
            C128Code.Add(new string[] { "90", Convert.ToChar(26).ToString(), "z", "90", "11011110110" });
            C128Code.Add(new string[] { "91", Convert.ToChar(27).ToString(), "{", "91", "11110110110" });
            C128Code.Add(new string[] { "92", Convert.ToChar(28).ToString(), "|", "92", "10101111000" });
            C128Code.Add(new string[] { "93", Convert.ToChar(29).ToString(), "}", "93", "10100011110" });
            C128Code.Add(new string[] { "94", Convert.ToChar(30).ToString(), "~", "94", "10001011110" });
            C128Code.Add(new string[] { "95", Convert.ToChar(31).ToString(), Convert.ToChar(127).ToString(), "95", "10111101000" });
            C128Code.Add(new string[] { "96", Convert.ToChar(202).ToString()/*FNC3*/, Convert.ToChar(202).ToString()/*FNC3*/, "96", "10111100010" });
            C128Code.Add(new string[] { "97", Convert.ToChar(201).ToString()/*FNC2*/, Convert.ToChar(201).ToString()/*FNC2*/, "97", "11110101000" });
            C128Code.Add(new string[] { "98", "SHIFT", "SHIFT", "98", "11110100010" });
            C128Code.Add(new string[] { "99", "CODE_C", "CODE_C", "99", "10111011110" });
            C128Code.Add(new string[] { "100", "CODE_B", Convert.ToChar(203).ToString()/*FNC4*/, "CODE_B", "10111101110" });
            C128Code.Add(new string[] { "101", Convert.ToChar(203).ToString()/*FNC4*/, "CODE_A", "CODE_A", "11101011110" });
            C128Code.Add(new string[] { "102", Convert.ToChar(200).ToString()/*FNC1*/, Convert.ToChar(200).ToString()/*FNC1*/, Convert.ToChar(200).ToString()/*FNC1*/, "11110101110" });
            C128Code.Add(new string[] { "103", "START_A", "START_A", "START_A", "11010000100" });
            C128Code.Add(new string[] { "104", "START_B", "START_B", "START_B", "11010010000" });
            C128Code.Add(new string[] { "105", "START_C", "START_C", "START_C", "11010011100" });
            C128Code.Add(new string[] { "", "STOP", "STOP", "STOP", "1100011101011" });
        }

        private string CalculateCheckDigit()
        {
            string currentStartChar = _FormattedData[0];
            uint CheckSum = 0;

            for (uint i = 0; i < _FormattedData.Count; i++)
            {
                //replace apostrophes with double apostrophes for escape chars
                string s = _FormattedData[(int)i].Replace("'", "''");

                //try to find value in the A column
                IEnumerable<string[]> rows = C128Code.Where(x => x[1] == s);

                //try to find value in the B column
                if (rows.Count() <= 0)
                    rows = C128Code.Where(x => x[2] == s);

                //try to find value in the C column
                if (rows.Count() <= 0)
                    rows = C128Code.Where(x => x[3] == s);

                uint value = UInt32.Parse(rows.First()[0]);
                uint addition = value * ((i == 0) ? 1 : i);
                CheckSum += addition;
            }

            uint Remainder = (CheckSum % 103);
            string[] RetRows = C128Code.First(x => x[0] == Remainder.ToString());
            return RetRows[4].ToString();
        }

        private void BreakUpDataForEncoding()
        {
            string temp = "";
            string tempRawData = RawData;

            //breaking the raw data up for code A and code B will mess up the encoding
            if (type == Code128Types.A || type == Code128Types.B)
            {
                foreach (char c in RawData)
                    _FormattedData.Add(c.ToString());
                return;
            }
            if (type == Code128Types.C)
            {
                if (!CheckNumericOnly())
                    throw new Exception("EC128-6: Only numeric values can be encoded with C128-C.");

                //CODE C: adds a 0 to the front of the Raw_Data if the length is not divisible by 2
                if (RawData.Length % 2 > 0)
                    tempRawData = "0" + RawData;
            }

            foreach (char c in tempRawData)
            {
                if (Char.IsNumber(c))
                {
                    if (temp == "")
                    {
                        temp += c;
                    }
                    else
                    {
                        temp += c;
                        _FormattedData.Add(temp);
                        temp = "";
                    }
                }
                else
                {
                    if (temp != "")
                    {
                        _FormattedData.Add(temp);
                        temp = "";
                    }
                    _FormattedData.Add(c.ToString());
                }
            }

            if (temp != "")
            {
                _FormattedData.Add(temp);
            }
        }

        private void InsertStartandCodeCharacters()
        {
            switch (type)
            {
                case Code128Types.A:
                    _FormattedData.Insert(0, "START_A");
                    break;
                case Code128Types.B:
                    _FormattedData.Insert(0, "START_B");
                    break;
                case Code128Types.C:
                    _FormattedData.Insert(0, "START_C");
                    break;
                default:
                    throw new Exception("EC128-4: Unknown start type in fixed type encoding.");
            }
        }

        private string GetEncoding()
        {
            //break up data for encoding
            BreakUpDataForEncoding();

            //insert the start characters
            InsertStartandCodeCharacters();

            string CheckDigit = CalculateCheckDigit();

            string Encoded_Data = "";
            foreach (string s in _FormattedData)
            {
                string[] E_Row;

                E_Row = C128Code.Find(x => x[(int)type + 1] == s);

                if (E_Row == null || E_Row.Length <= 0)
                    throw new Exception("EC128-5: Could not find encoding of a value( " + s + " ) in C128 type " + type.ToString());

                Encoded_Data += E_Row[4].ToString();
            }

            //add the check digit
            Encoded_Data += CalculateCheckDigit();

            //add the stop character
            Encoded_Data += C128Code.Find(x => x[1] == "STOP")[4];

            return Encoded_Data;
        }
    }

    public enum Code128Types : ushort
    {
        A,
        B,
        C
    }
}
