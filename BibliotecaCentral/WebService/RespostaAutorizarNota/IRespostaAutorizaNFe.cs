using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace BibliotecaCentral.WebService.RespostaAutorizarNota
{
    [ServiceContract(Namespace = EnderecosConexao.RespostaAutorizarServico)]
    internal interface IRespostaAutorizaNFe
    {
        [OperationContract(Action = EnderecosConexao.RespostaAutorizarMetodo, ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Message nfeRetAutorizacaoLote(Message request);

        [OperationContract(Action = EnderecosConexao.RespostaAutorizarMetodo, ReplyAction = "*")]
        Task<Message> nfeRetAutorizacaoLoteAsync(Message request);
    }
}
