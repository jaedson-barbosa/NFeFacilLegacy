using NFeFacil.IBGE;
using NFeFacil.ItensBD;
using NFeFacil.ModeloXML;
using NFeFacil.ModeloXML.PartesProcesso;
using NFeFacil.WebService;
using NFeFacil.WebService.Pacotes;
using System;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class Consulta : Page
    {
        string Chave { get; set; }
        bool Homologacao { get; set; }
        Estado UF { get; set; }
        ObservableCollection<string> Resultados { get; } = new ObservableCollection<string>();

        public Consulta()
        {
            InitializeComponent();
        }

        private async void Analisar(object sender, RoutedEventArgs e)
        {
            if (UF == null || string.IsNullOrEmpty(Chave))
            {
                Resultados.Insert(0, "Estado e Chave são informações obrigatórias.");
            }
            else
            {
                btnAnalisar.IsEnabled = false;
                carregamento.IsActive = true;
                try
                {
                    var resp = await new GerenciadorGeral<ConsSitNFe, RetConsSitNFe>(UF, Operacoes.Consultar, Homologacao)
                        .EnviarAsync(new ConsSitNFe(Chave, Homologacao));
                    Resultados.Insert(0, resp.xMotivo);
                    if (resp.cStat == 100)
                    {
                        using (var db = new AplicativoContext())
                        {
                            var nota = db.NotasFiscais.Find(resp.chNFe);
                            if (nota != null)
                            {
                                if (nota.Status == (int)StatusNFe.Emitida || nota.Status == (int)StatusNFe.Cancelada) { }
                                else
                                {
                                    nota.Status = (int)StatusNFe.Emitida;
                                    var original = XElement.Parse(nota.XML).FromXElement<NFe>();
                                    var novo = new Processo()
                                    {
                                        NFe = original,
                                        ProtNFe = resp.protNFe
                                    };
                                    nota.XML = novo.ToXElement<Processo>().ToString();
                                    nota.UltimaData = Propriedades.DateTimeNow;
                                    db.NotasFiscais.Update(nota);
                                    db.SaveChanges();
                                }
                                Resultados.Insert(0, "Detectada e atualizada nota fiscal.");
                            }
                        }
                    }
                }
                catch (Exception erro)
                {
                    erro.ManipularErro();
                }
                btnAnalisar.IsEnabled = true;
                carregamento.IsActive = false;
            }
        }

        private void CopiarXML(object sender, RoutedEventArgs e)
        {

        }
    }
}
