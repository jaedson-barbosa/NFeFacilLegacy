using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace NFeFacil.WebService.RespostaAutorizarNota
{
    [ServiceContract(Namespace = ConjuntoServicos.RespostaAutorizarServico)]
    internal interface IRespostaAutorizaNFe
    {
        [OperationContract(Action = ConjuntoServicos.RespostaAutorizarMetodo, ReplyAction = "*")]
        [XmlSerializerFormat(SupportFaults = true)]
        Message nfeRetAutorizacaoLote(Message request);

        [OperationContract(Action = ConjuntoServicos.RespostaAutorizarMetodo, ReplyAction = "*")]
        Task<Message> nfeRetAutorizacaoLoteAsync(Message request);
    }
}
