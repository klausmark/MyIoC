using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MyIoC
{
    internal class RegisteredImplementation
    {
        public Type ImplementationType { get; set; }
        public bool ShouldBeSingleInstance { get; set; }
        public object Instance { get; set; }
    }

    public class MyIoC
    {
        private readonly Dictionary<Type, RegisteredImplementation> _registrations = new Dictionary<Type, RegisteredImplementation>();

        public void Register<TInterface, TImplementation>(bool shouldThereOnlyBeOneInstanceOfImplementation = true)
        {
            var interfaceType = typeof (TInterface);
            var implementationType = typeof (TImplementation);
            if (ImplementationDoesNotImplementInterface(interfaceType, implementationType)) throw new Exception("Implementation does not implement abstraction");
            var implementation = new RegisteredImplementation
            {
                ImplementationType = implementationType,
                ShouldBeSingleInstance = shouldThereOnlyBeOneInstanceOfImplementation
            };
            _registrations.Add(interfaceType, implementation);
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
            var parameters = ConstructParametersForConstructor(constructor).ToArray();
            return constructor.Invoke(parameters);
        }

        private IEnumerable<object> ConstructParametersForConstructor(ConstructorInfo constructor)
        {
            var parameters = constructor.GetParameters();
            foreach (var parameter in parameters)
            {
                if (_registrations[parameter.ParameterType].ShouldBeSingleInstance && _registrations[parameter.ParameterType].Instance != null)
                    yield return _registrations[parameter.ParameterType].Instance;
                yield return Resolv(_registrations[parameter.ParameterType].ImplementationType);
            }
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