﻿using NFeFacil.ItensBD;
using NFeFacil.Log;
using NFeFacil.ModeloXML;
using NFeFacil.ModeloXML.PartesProcesso;
using NFeFacil.Validacao;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class VisualizacaoNFe : Page
    {
        Popup Log = Popup.Current;
        NFeDI ItemBanco { get; set; }
        object ObjetoItemBanco { get; set; }

        public VisualizacaoNFe()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MainPage.Current.SeAtualizar(Symbol.View, "Visualizar NFe");
            ItemBanco = (NFeDI)e.Parameter;
            var xml = XElement.Parse(ItemBanco.XML);
            List<PropriedadeHierárquica> propriedades;
            if (ItemBanco.Status < (int)StatusNFe.Emitida)
            {
                var nfe = xml.FromXElement<NFe>();
                ObjetoItemBanco = nfe;
                propriedades = ObterPropriedades(nfe.Informacoes);
            }
            else
            {
                var processo = xml.FromXElement<Processo>();
                ObjetoItemBanco = processo;
                propriedades = ObterPropriedades(processo.NFe.Informacoes);
            }
            var linear = PropriedadeHierarquicaToLinear(propriedades, 0);
            linear.ForEach(x => AdicionarCampo(x.Texto, (EstilosTexto)x.Profundidade, x.Complementar));
            AtualizarBotoesComando();
        }

        List<PropriedadeHierárquica> ObterPropriedades(object obj)
        {
            var retorno = new List<PropriedadeHierárquica>();
            var tipo = obj.GetType();
            foreach (var prop in tipo.GetProperties().Where(x => x.CanWrite
                && x.GetCustomAttribute<System.Xml.Serialization.XmlIgnoreAttribute>() == null))
            {
                var valor = prop.GetValue(obj);
                if (valor != null)
                {
                    var tipoFilho = valor.GetType();
                    if (tipoFilho.Namespace.Contains("NFeFacil") && !(valor is IEnumerable))
                    {
                        retorno.Add(new PropriedadeHierárquica
                        {
                            Nome = prop.Name,
                            Valor = ObterPropriedades(valor)
                        });
                    }
                    else if (valor is IEnumerable teste && !(valor is string))
                    {
                        List<PropriedadeHierárquica> propriedadesFilhas = new List<PropriedadeHierárquica>();
                        foreach (var item in teste)
                        {
                            var tipoItem = item.GetType();
                            object valorItem;
                            if (tipoItem.Namespace.Contains("NFeFacil"))
                            {
                                valorItem = ObterPropriedades(item);
                            }
                            else
                            {
                                valorItem = item;
                            }
                            propriedadesFilhas.Add(new PropriedadeHierárquica
                            {
                                Nome = tipoItem.Name,
                                Valor = valorItem
                            });
                        }
                        retorno.AddRange(propriedadesFilhas);
                    }
                    else
                    {
                        var desc = prop.GetCustomAttribute<DescricaoPropriedade>();
                        var ext = prop.GetCustomAttribute<PropriedadeExtensivel>();
                        if (desc != null)
                        {
                            retorno.Add(new PropriedadeHierárquica
                            {
                                Nome = desc.Descricao,
                                Valor = valor
                            });
                        }
                        else if (ext != null)
                        {
                            retorno.Add(new PropriedadeHierárquica
                            {
                                Nome = ext.NomeExtensão,
                                Valor = ext.ObterValor(valor)
                            });
                        }
                        else
                        {
                            retorno.Add(new PropriedadeHierárquica
                            {
                                Nome = prop.Name,
                                Valor = valor
                            });
                        }
                    }
                }
            }
            return retorno;
        }

        List<PropriedadeLinear> PropriedadeHierarquicaToLinear(List<PropriedadeHierárquica> hierarquia, int profundidade)
        {
            var retorno = new List<PropriedadeLinear>();
            for (int i = 0; i < hierarquia.Count; i++)
            {
                var atual = hierarquia[i];
                if (atual.Valor is List<PropriedadeHierárquica> subhierarquia)
                {
                    retorno.Add(new PropriedadeLinear
                    {
                        Profundidade = profundidade,
                        Texto = atual.Nome
                    });
                    retorno.AddRange(PropriedadeHierarquicaToLinear(subhierarquia, profundidade + 1));
                }
                else
                {
                    retorno.Add(new PropriedadeLinear
                    {
                        Profundidade = (int)EstilosTexto.BodyTextBlockStyle,
                        Texto = atual.Nome,
                        Complementar = atual.Valor.ToString()
                    });
                }
            }
            return retorno;
        }

        void AdicionarCampo(string texto, EstilosTexto estilo, string textoComplementar)
        {
            texto = ProcessarTitulo(texto);
            var linha = new Run()
            {
                Text = textoComplementar == null ? texto : $"{texto}: "
            };
            switch (estilo)
            {
                case EstilosTexto.HeaderTextBlockStyle:
                    linha.FontWeight = FontWeights.Light;
                    linha.FontSize = 46;
                    break;
                case EstilosTexto.SubheaderTextBlockStyle:
                    linha.FontWeight = FontWeights.Light;
                    linha.FontSize = 34;
                    break;
                case EstilosTexto.TitleTextBlockStyle:
                    linha.FontWeight = FontWeights.SemiLight;
                    linha.FontSize = 24;
                    break;
                case EstilosTexto.SubtitleTextBlockStyle:
                    linha.FontWeight = FontWeights.Normal;
                    linha.FontSize = 20;
                    break;
                case EstilosTexto.BodyTextBlockStyle:
                    break;
                default:
                    break;
            }

            if (textoComplementar != null)
            {
                linha.FontWeight = FontWeights.Bold;
            }

            visualizacao.Inlines.Add(linha);

            if (textoComplementar != null)
            {
                linha = new Run()
                {
                    Text = textoComplementar
                };
                switch (estilo)
                {
                    case EstilosTexto.HeaderTextBlockStyle:
                        linha.FontWeight = FontWeights.Light;
                        linha.FontSize = 46;
                        break;
                    case EstilosTexto.SubheaderTextBlockStyle:
                        linha.FontWeight = FontWeights.Light;
                        linha.FontSize = 34;
                        break;
                    case EstilosTexto.TitleTextBlockStyle:
                        linha.FontWeight = FontWeights.SemiLight;
                        linha.FontSize = 24;
                        break;
                    case EstilosTexto.SubtitleTextBlockStyle:
                        linha.FontWeight = FontWeights.Normal;
                        linha.FontSize = 20;
                        break;
                    case EstilosTexto.BodyTextBlockStyle:
                        break;
                    default:
                        break;
                }
                visualizacao.Inlines.Add(linha);
            }

            visualizacao.Inlines.Add(new LineBreak());
        }

        string ProcessarTitulo(string titulo)
        {
            StringBuilder construtor = new StringBuilder();
            construtor.Append(char.ToUpper(titulo[0]));
            for (int i = 1; i < titulo.Length; i++)
            {
                if (char.IsUpper(titulo[i]) && char.IsLower(titulo[i - 1]))
                {
                    construtor.Append($" {titulo[i]}");
                }
                else
                {
                    construtor.Append(titulo[i]);
                }
            }
            return construtor.ToString();
        }

        struct PropriedadeHierárquica
        {
            public string Nome { get; set; }
            public object Valor { get; set; }
        }

        struct PropriedadeLinear
        {
            public string Texto { get; set; }
            public string Complementar { get; set; }
            public int Profundidade { get; set; }
        }

        enum EstilosTexto
        {
            HeaderTextBlockStyle,
            SubheaderTextBlockStyle,
            TitleTextBlockStyle,
            SubtitleTextBlockStyle,
            BodyTextBlockStyle
        }

        private void Editar(object sender, RoutedEventArgs e)
        {
            var nfe = (NFe)ObjetoItemBanco;
            var analisador = new AnalisadorNFe(ref nfe);
            nfe.Signature = null;
            ItemBanco.Status = (int)StatusNFe.Edição;
            analisador.Desnormalizar();
            MainPage.Current.Navegar<ManipulacaoNotaFiscal>(nfe);
        }

        private void Salvar(object sender, RoutedEventArgs e)
        {
            ItemBanco.Status = (int)StatusNFe.Salva;
            AtualizarDI();
            AtualizarBotoesComando();
            Log.Escrever(TitulosComuns.Sucesso, "Nota fiscal salva com sucesso.");
        }

        private async void Assinar(object sender, RoutedEventArgs e)
        {
            var nfe = (NFe)ObjetoItemBanco;
            var OperacoesNota = new OperacoesNotaSalva(Log);
            if (await OperacoesNota.Assinar(nfe))
            {
                ItemBanco.Status = (int)StatusNFe.Assinada;
                AtualizarDI();
                AtualizarBotoesComando();
                Log.Escrever(TitulosComuns.Sucesso, "Nota fiscal assinada com sucesso.");
            }
        }

        private async void Transmitir(object sender, RoutedEventArgs e)
        {
            var nfe = (NFe)ObjetoItemBanco;
            var OperacoesNota = new OperacoesNotaSalva(Log);
            var resposta = await OperacoesNota.Transmitir(nfe, nfe.AmbienteTestes);
            if (resposta.sucesso)
            {
                ObjetoItemBanco = new Processo()
                {
                    NFe = nfe,
                    ProtNFe = resposta.protocolo
                };
                ItemBanco.Status = (int)StatusNFe.Emitida;
                AtualizarDI();
                AtualizarBotoesComando();
            }
        }

        private void Imprimir(object sender, RoutedEventArgs e)
        {
            var processo = (Processo)ObjetoItemBanco;
            MainPage.Current.Navegar<DANFE.ViewDANFE>(processo);
            ItemBanco.Impressa = true;
            AtualizarDI();
            AtualizarBotoesComando();
        }

        private async void Exportar(object sender, RoutedEventArgs e)
        {
            XElement xml;
            string id;
            if (ItemBanco.Status < (int)StatusNFe.Emitida)
            {
                var nfe = (NFe)ObjetoItemBanco;
                id = nfe.Informacoes.Id;
                xml = ObjetoItemBanco.ToXElement<NFe>();
            }
            else
            {
                var processo = (Processo)ObjetoItemBanco;
                id = processo.NFe.Informacoes.Id;
                xml = ObjetoItemBanco.ToXElement<Processo>();
            }

            var OperacoesNota = new OperacoesNotaSalva(Log);
            if (await OperacoesNota.Exportar(xml, id))
            {
                ItemBanco.Exportada = true;
                AtualizarDI();
                Log.Escrever(TitulosComuns.Sucesso, $"Nota fiscal exportada com sucesso.");
            }
        }

        private void AtualizarDI()
        {
            try
            {
                using (var db = new AplicativoContext())
                {
                    ItemBanco.UltimaData = DateTime.Now;
                    if (ItemBanco.Status < (int)StatusNFe.Emitida)
                    {
                        ItemBanco.XML = ObjetoItemBanco.ToXElement<NFe>().ToString();
                    }
                    else
                    {
                        ItemBanco.XML = ObjetoItemBanco.ToXElement<Processo>().ToString();
                    }

                    if (db.NotasFiscais.Count(x => x.Id == ItemBanco.Id) == 0)
                    {
                        db.Add(ItemBanco);
                    }
                    else
                    {
                        db.Update(ItemBanco);
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                e.ManipularErro();
            }
        }

        void AtualizarBotoesComando()
        {
            switch ((StatusNFe)ItemBanco.Status)
            {
                case StatusNFe.Validada:
                    btnEditar.IsEnabled = true;
                    btnSalvar.IsEnabled = false;
                    btnAssinar.IsEnabled = false;
                    btnTransmitir.IsEnabled = false;
                    btnImprimir.IsEnabled = false;
                    break;
                case StatusNFe.Salva:
                    btnEditar.IsEnabled = true;
                    btnSalvar.IsEnabled = false;
                    btnAssinar.IsEnabled = true;
                    btnTransmitir.IsEnabled = false;
                    btnImprimir.IsEnabled = false;
                    break;
                case StatusNFe.Assinada:
                    btnEditar.IsEnabled = true;
                    btnSalvar.IsEnabled = false;
                    btnAssinar.IsEnabled = false;
                    btnTransmitir.IsEnabled = true;
                    btnImprimir.IsEnabled = false;
                    break;
                case StatusNFe.Emitida:
                    btnEditar.IsEnabled = false;
                    btnSalvar.IsEnabled = false;
                    btnAssinar.IsEnabled = false;
                    btnTransmitir.IsEnabled = false;
                    btnImprimir.IsEnabled = true;
                    break;
                case StatusNFe.Cancelada:
                    btnEditar.IsEnabled = false;
                    btnSalvar.IsEnabled = false;
                    btnAssinar.IsEnabled = false;
                    btnTransmitir.IsEnabled = false;
                    btnImprimir.IsEnabled = false;
                    break;
            }
        }
    }
}
