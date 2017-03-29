using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFeFacil.ItensBD
{
    interface IConverterDI<in TipoBase> where TipoBase : class
    {
        IId Converter(TipoBase item);
    }
}
