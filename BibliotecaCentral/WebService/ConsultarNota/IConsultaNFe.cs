using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace BibliotecaCentral.WebService.ConsultarNota
{
    [ServiceContract(Namespace = EnderecosConexao.ConsultarServico)]
    public interface IConsultaNFe
    {
        [OperationContract(Action = EnderecosConexao.ConsultarMetodo, ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true, Style = OperationFormatStyle.Document)]
        Message Consultar(Message request);

        [OperationContract(Action = EnderecosConexao.ConsultarMetodo, ReplyAction = "*")]
        Task<Message> ConsultarAsync(Message request);
    }
}
