using System;
using System.Runtime.CompilerServices;

namespace VContainer.Internal
{
    sealed class FuncInstanceProvider : IInstanceProvider
    {
        readonly Func<IObjectResolver, object> implementationProvider;

        public FuncInstanceProvider(Func<IObjectResolver, object> implementationProvider)
        {
            this.implementationProvider = implementationProvider;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object SpawnInstance(IObjectResolver resolver) => implementationProvider(resolver);
    }
    
    sealed class FuncInstanceProviderContexted : IInstanceProvider
    {
        readonly Func<object, IObjectResolver, object> implementationProvider;
        private readonly object context;

        public FuncInstanceProviderContexted(Func<object, IObjectResolver, object> implementationProvider, object context)
        {
            this.implementationProvider = implementationProvider;
            this.context = context;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object SpawnInstance(IObjectResolver resolver) => implementationProvider(context, resolver);
    }
}
