using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace BibliotecaCentral.WebService.AutorizarNota
{
    [ServiceContract(Namespace = EnderecosConexao.AutorizarServico)]
    public interface IAutorizaNFe
    {
        [OperationContract(Action = EnderecosConexao.AutorizarMetodo, ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Message nfeAutorizacaoLote(Message request);

        [OperationContract(Action = EnderecosConexao.AutorizarMetodo, ReplyAction = "*")]
        Task<Message> nfeAutorizacaoLoteAsync(Message request);
    }
}
