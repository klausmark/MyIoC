using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MyIoC
{
    public class MyIoC
    {
        private readonly Dictionary<Type, Type> _registrations = new Dictionary<Type, Type>();

        public void Register<TInterface, TImplementation>()
        {
            var interfaceType = typeof (TInterface);
            var implementationType = typeof (TImplementation);
            if (ImplementationDoesNotImplementInterface(interfaceType, implementationType)) throw new Exception("Implementation does not implement abstraction");
            _registrations.Add(typeof(TInterface), typeof(TImplementation));
        }

        private static bool ImplementationDoesNotImplementInterface(Type interfaceType, Type implementationType)
        {
            return !interfaceType.IsAssignableFrom(implementationType);
        }

        public void Register<TImplementation>()
        {
            Register<TImplementation, TImplementation>();
        }

        public T Resolv<T>()
        {
            var type = typeof (T);
            return (T) Resolv(type);
        }

        private object Resolv(Type type)
        {
            var constructor = GetConstructorICanConstruct(type);
            var parameters = ConstructParametersForConstructor(constructor);
            return constructor.Invoke(parameters);
        }

        private object[] ConstructParametersForConstructor(ConstructorInfo constructor)
        {
            var parameters = constructor.GetParameters();
            return parameters.Select(parameter => Resolv(_registrations[parameter.ParameterType])).ToArray();
        }

        private ConstructorInfo GetConstructorICanConstruct(Type type)
        {
            foreach (var constructor in type.GetConstructors())
            {
                var doIHaveAllParamaters = true;
                var parameters = constructor.GetParameters();
                foreach (var parameter in parameters)
                    if (!_registrations.ContainsKey(parameter.ParameterType))
                        doIHaveAllParamaters = false;
                if (doIHaveAllParamaters) return constructor;
            }
            throw new Exception("There is no constructor that i know types of all parameters for");
        }

        public static MyIoC Default { get; } = new MyIoC();
    }
}