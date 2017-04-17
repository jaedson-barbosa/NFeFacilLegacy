using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace BibliotecaCentral.WebService.ConsultarNota
{
    [ServiceContract(Namespace = ConjuntoServicos.ConsultarServico)]
    public interface IConsultaNFe
    {
        [OperationContract(Action = ConjuntoServicos.ConsultarMetodo, ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true, Style = OperationFormatStyle.Document)]
        Message Consultar(Message request);

        [OperationContract(Action = ConjuntoServicos.ConsultarMetodo, ReplyAction = "*")]
        Task<Message> ConsultarAsync(Message request);
    }
}
