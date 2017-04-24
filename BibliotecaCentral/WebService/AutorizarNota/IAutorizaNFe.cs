using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace BibliotecaCentral.WebService.AutorizarNota
{
    [ServiceContract(Namespace = ConjuntoServicos.AutorizarServico)]
    public interface IAutorizaNFe
    {
        [OperationContract(Action = ConjuntoServicos.AutorizarMetodo, ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Message nfeAutorizacaoLote(Message request);

        [OperationContract(Action = ConjuntoServicos.AutorizarMetodo, ReplyAction = "*")]
        Task<Message> nfeAutorizacaoLoteAsync(Message request);
    }
}
