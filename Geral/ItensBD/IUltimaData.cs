using System;

namespace BaseGeral.ItensBD
{
    public interface IUltimaData
    {
        DateTime UltimaData { get; set; }
    }

    public interface IGuidId
    {
        Guid Id { get; set; }
    }

    public interface IStatusAtual
    {
        int Status { get; set; }
        int StatusAdd { get; }
    }

    public interface IStatusAtivacao : IUltimaData
    {
        bool Ativo { get; set; }
    }
}
