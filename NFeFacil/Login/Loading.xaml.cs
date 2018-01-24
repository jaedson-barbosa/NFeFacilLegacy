using NFeFacil.View;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Login
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class Loading : Page
    {
        EtapaProcesso[] Etapas { get; }

        public Loading()
        {
            InitializeComponent();
            Etapas = new EtapaProcesso[]
            {
                new EtapaProcesso("Processar visual"),
                new EtapaProcesso("Processar dados do app"),
                new EtapaProcesso("Processar dados do usuário")
            };
            Start();
        }

        void Update(int etapasConcluidas)
        {
            barGeral.Value = etapasConcluidas;
            if (etapasConcluidas > 0)
            {
                Etapas[etapasConcluidas - 1].Atual = EtapaProcesso.Status.Concluido;
                Etapas[etapasConcluidas - 1].Update();
            }
            if (etapasConcluidas < Etapas.Length)
            {
                Etapas[etapasConcluidas].Atual = EtapaProcesso.Status.EmAndamento;
                Etapas[etapasConcluidas].Update();
            }
        }

        void Start()
        {
            Update(0);
            ProcessarVisual();
            Update(1);
            ProcessarDadosApp();
            Update(2);
            ProcessarDadosUsuario();
            Update(3);
        }

        void ProcessarVisual()
        {

        }

        void ProcessarDadosApp()
        {

        }

        void ProcessarDadosUsuario()
        {

        }
    }
}
