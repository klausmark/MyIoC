using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MyIoCLib.Exceptions;

namespace MyIoCLib
{
    internal class Implementation
    {
        public Type TypeOfImplementation { get; set; }
        public object InstanceOfImplementation { get; set; }
        public bool UseSingleInstance { get; set; }
    }
    public class MyIoC
    {
        private readonly Dictionary<Type, Implementation> _registrations = new Dictionary<Type, Implementation>();

        public void Register<TInterface, TImplementation>(bool useSingleInstance = true)
        {
            var typeOfInterface = typeof (TInterface);
            var typeOfImplementation = typeof (TImplementation);

            if (ImplementationDoesNotImplementInterface(typeOfInterface, typeOfImplementation))
                throw new ImplementationDoesNotImplementInterfaceException("Implementation does not implement abstraction");

            var implementation = new Implementation
            {
                UseSingleInstance = useSingleInstance,
                TypeOfImplementation = typeOfImplementation
            };
            _registrations.Add(typeOfInterface, implementation);
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
            var implementation = _registrations[type];
            if (implementation.UseSingleInstance && implementation.InstanceOfImplementation != null)
                return implementation.InstanceOfImplementation;

            var constructor = GetConstructorICanConstruct(implementation.TypeOfImplementation);
            var parameters = ConstructParametersForConstructor(constructor);
            var instanceOfType = constructor.Invoke(parameters);

            if (implementation.UseSingleInstance) implementation.InstanceOfImplementation = instanceOfType;
            return instanceOfType;
        }

        private object[] ConstructParametersForConstructor(ConstructorInfo constructor)
        {
            var parameters = constructor.GetParameters();
            return parameters.Select(parameter => Resolv(parameter.ParameterType)).ToArray();
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
            throw new NoValidConstructor("There is no constructor that i know types of all parameters for");
        }

        public static MyIoC Default { get; } = new MyIoC();
    }
}