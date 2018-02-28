using System;

namespace NFeFacil.Sincronizacao.FastServer
{
    /// <summary>
    /// This class is only used as a marker
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
    public sealed class FromContentAttribute : Attribute { }
}
