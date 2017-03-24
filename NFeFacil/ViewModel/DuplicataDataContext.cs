using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using System;

namespace NFeFacil.ViewModel
{
    public sealed class DuplicataDataContext
    {
        public Duplicata _Duplicata { get; }

        public DateTimeOffset DVenc
        {
            get
            {
                if (_Duplicata.DVenc == null) _Duplicata.DVenc = DateTimeOffset.Now.ToString("yyyy-MM-dd");
                return DateTimeOffset.Parse(_Duplicata.DVenc);
            }
            set
            {
                _Duplicata.DVenc = value.ToString("yyyy-MM-dd");
            }
        }

        public DuplicataDataContext()
        {
            _Duplicata = new Duplicata();
        }
        public DuplicataDataContext(ref Duplicata duplicata)
        {
            _Duplicata = duplicata;
        }
    }
}
