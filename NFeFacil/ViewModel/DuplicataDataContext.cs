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
                if (_Duplicata.dVenc == null) _Duplicata.dVenc = DateTimeOffset.Now.ToString("yyyy-MM-dd");
                return DateTimeOffset.Parse(_Duplicata.dVenc);
            }
            set
            {
                _Duplicata.dVenc = value.ToString("yyyy-MM-dd");
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
