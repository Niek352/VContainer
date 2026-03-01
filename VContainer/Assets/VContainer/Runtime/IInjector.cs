using System.Collections.Generic;
using System;

namespace VContainer
{
    public interface IInjector
    {
        void Inject(object instance, IObjectResolver resolver, IReadOnlyList<IInjectParameter> parameters);
        object CreateInstance(IObjectResolver resolver, IReadOnlyList<IInjectParameter> parameters);
        
        void InjectWithSpan(object instance, IObjectResolver resolver, ReadOnlySpan<IInjectParameter> parameters);
        object CreateInstanceWithSpan(IObjectResolver resolver, ReadOnlySpan<IInjectParameter> parameters);
    }
}
