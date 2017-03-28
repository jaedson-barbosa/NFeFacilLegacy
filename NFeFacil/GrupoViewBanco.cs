using NFeFacil.ItensBD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFeFacil
{
    internal struct GrupoViewBanco<TipoDado> where TipoDado : IId
    {
        public TipoDado ItemBanco { get; set; }
        public TipoOperacao OperacaoRequirida { get; set; }
    }

    enum TipoOperacao
    {
        Adicao,
        Edicao
    }
}
