using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VContainer.Internal
{
    public sealed class InstanceProvider : IInstanceProvider
    {
        readonly IInjector injector;
        readonly IReadOnlyList<IInjectParameter> customParameters;

        public InstanceProvider(
            IInjector injector,
            IReadOnlyList<IInjectParameter> customParameters = null)
        {
            this.injector = injector;
            this.customParameters = customParameters;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object SpawnInstance(IObjectResolver resolver)
            => injector.CreateInstance(resolver, customParameters);
        
        
    }

    public static class InstanceSpawner
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Instantiate<T>(this IObjectResolver resolver, IReadOnlyList<IInjectParameter> injectParameters = null)
        {
            var injector = ReflectionInjector.Build(typeof(T));
            return (T)injector.CreateInstance(resolver, injectParameters);
        }
    } 
}