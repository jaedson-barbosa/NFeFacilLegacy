using System;

namespace NFeFacil.ItensBD
{
    public interface IUltimaData
    {
        Guid Id { get; set; }
        DateTime UltimaData { get; set; }
    }

    public interface IStatusAtivacao : IUltimaData
    {
        bool Ativo { get; set; }
    }
}
