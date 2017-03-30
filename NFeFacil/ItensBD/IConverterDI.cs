﻿namespace NFeFacil.ItensBD
{
    interface IConverterDI<in TipoBase> where TipoBase : class
    {
        IId Converter(TipoBase item);
    }
}