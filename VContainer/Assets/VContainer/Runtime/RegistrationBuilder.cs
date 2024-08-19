using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer.Extensions;
using VContainer.Internal;

namespace VContainer
{
    public class RegistrationBuilder
    {
        protected internal readonly Type ImplementationType;
        protected internal readonly Lifetime Lifetime;

        protected internal List<Type> InterfaceTypes;
        protected internal List<IInjectParameter> Parameters;

        public RegistrationBuilder(Type implementationType, Lifetime lifetime)
        {
            ImplementationType = implementationType;
            Lifetime = lifetime;
        }

        public virtual Registration Build()
        {
            var implementationType = ImplementationType;
            var interfaces = InterfaceTypes;
            
#if UNITY_INCLUDE_TESTS            
            implementationType = Substitute(implementationType, ref interfaces);
#endif
            
            var injector = InjectorCache.GetOrBuild(implementationType);
            var spawner = new InstanceProvider(injector, Parameters);
            return new Registration(
                implementationType,
                Lifetime,
                interfaces,
                spawner);
        }
        
        private Type Substitute(Type implementationType, ref List<Type> interfaces)
        {
            if (InterfaceTypes is not null)
            {
                foreach (var interfaceType in InterfaceTypes)
                {
                    if (InstallerSubstitution.TryGetSubstitution(interfaceType, out var substitution))
                    {
                        Debug.Log($"Implementation {implementationType.FullName} substitute for {substitution.FullName}");
                        implementationType = substitution;
                        interfaces = implementationType.GetInterfaces().ToList();
                    }
                }
            }
            return implementationType;
        }

        public RegistrationBuilder As<TInterface>()
            => As(typeof(TInterface));

        public RegistrationBuilder As<TInterface1, TInterface2>()
            => As(typeof(TInterface1), typeof(TInterface2));

        public RegistrationBuilder As<TInterface1, TInterface2, TInterface3>()
            => As(typeof(TInterface1), typeof(TInterface2), typeof(TInterface3));

        public RegistrationBuilder As<TInterface1, TInterface2, TInterface3, TInterface4>()
            => As(typeof(TInterface1), typeof(TInterface2), typeof(TInterface3), typeof(TInterface4));

        public RegistrationBuilder AsSelf()
        {
            AddInterfaceType(ImplementationType);
            return this;
        }

        public virtual RegistrationBuilder AsImplementedInterfaces()
        {
            InterfaceTypes = InterfaceTypes ?? new List<Type>();
            InterfaceTypes.AddRange(ImplementationType.GetInterfaces());
            return this;
        }

        public RegistrationBuilder As(Type interfaceType)
        {
            AddInterfaceType(interfaceType);
            return this;
        }

        public RegistrationBuilder As(Type interfaceType1, Type interfaceType2)
        {
            AddInterfaceType(interfaceType1);
            AddInterfaceType(interfaceType2);
            return this;
        }

        public RegistrationBuilder As(Type interfaceType1, Type interfaceType2, Type interfaceType3)
        {
            AddInterfaceType(interfaceType1);
            AddInterfaceType(interfaceType2);
            AddInterfaceType(interfaceType3);
            return this;
        }

        public RegistrationBuilder As(params Type[] interfaceTypes)
        {
            foreach (var interfaceType in interfaceTypes)
            {
                AddInterfaceType(interfaceType);
            }
            return this;
        }

        public RegistrationBuilder WithParameter(string name, object value)
        {
            Parameters = Parameters ?? new List<IInjectParameter>();
            Parameters.Add(new NamedParameter(name, value));
            return this;
        }
        
        public RegistrationBuilder WithParameter(string name, Func<IObjectResolver, object> value)
        {
            Parameters = Parameters ?? new List<IInjectParameter>();
            Parameters.Add(new FuncNamedParameter(name, value));
            return this;
        }

        public RegistrationBuilder WithParameter(Type type, object value)
        {
            Parameters = Parameters ?? new List<IInjectParameter>();
            Parameters.Add(new TypedParameter(type, value));
            return this;
        }
        
        public RegistrationBuilder WithParameter(Type type, Func<IObjectResolver, object> value)
        {
            Parameters = Parameters ?? new List<IInjectParameter>();
            Parameters.Add(new FuncTypedParameter(type, value));
            return this;
        }

        public RegistrationBuilder WithParameter<TParam>(TParam value)
        {
            return WithParameter(typeof(TParam), value);
        }
        
        public RegistrationBuilder WithParameter<TParam>(Func<IObjectResolver, TParam> value)
        {
            return WithParameter(typeof(TParam), resolver => value(resolver));
        }
        
        public RegistrationBuilder WithParameter<TParam>(Func<TParam> value)
        {
            return WithParameter(typeof(TParam), _ => value());
        }

        protected virtual void AddInterfaceType(Type interfaceType)
        {
            if (!interfaceType.IsAssignableFrom(ImplementationType))
            {
                throw new VContainerException(interfaceType, $"{ImplementationType} is not assignable from {interfaceType}");
            }
            InterfaceTypes = InterfaceTypes ?? new List<Type>();
            if (!InterfaceTypes.Contains(interfaceType))
                InterfaceTypes.Add(interfaceType);
        }
   }
}
