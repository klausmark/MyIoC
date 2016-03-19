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
        public ImplementationParameter[] Parameters { get; set; }
    }

    public class MyIoC
    {
        private readonly Dictionary<Type, Implementation> _registrations = new Dictionary<Type, Implementation>();

        public void Register<TInterface, TImplementation>(bool useSingleInstance, params ImplementationParameter[] parameters)
        {
            var typeOfInterface = typeof (TInterface);
            var typeOfImplementation = typeof (TImplementation);

            CheckIfImplementationImplementsInterface(typeOfInterface, typeOfImplementation);

            var implementation = new Implementation
            {
                UseSingleInstance = useSingleInstance,
                TypeOfImplementation = typeOfImplementation,
                Parameters = parameters ?? new ImplementationParameter[0]
            };
            _registrations.Add(typeOfInterface, implementation);
        }

        public void Register<TInterface, TImplementation>()
        {
            Register<TInterface, TImplementation>(true);
        }

        private static void CheckIfImplementationImplementsInterface(Type typeOfInterface,Type typeOfImplementation)
        {
            if (!typeOfInterface.IsAssignableFrom(typeOfImplementation))
                throw new ImplementationDoesNotImplementInterfaceException("Implementation does not implement abstraction");
        }

        public void Register<TImplementation>(bool useSingleInstance)
        {
            Register<TImplementation, TImplementation>(useSingleInstance);
        }

        public void Register<TImplementation>()
        {
            Register<TImplementation, TImplementation>(true);
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

            var constructor = GetConstructorICanConstruct(type);
            var parameters = ConstructParametersForConstructor(type, constructor).ToArray();
            var instanceOfType = constructor.Invoke(parameters);

            if (implementation.UseSingleInstance) implementation.InstanceOfImplementation = instanceOfType;
            return instanceOfType;
        }

        private IEnumerable<object> ConstructParametersForConstructor(Type type, ConstructorInfo constructor)
        {
            var parameters = constructor.GetParameters();
            foreach (var parameter in parameters)
            {
                if (ImplementationHasTheParameter(type, parameter)) yield return GetImplementationParameter(type, parameter);
                else yield return Resolv(parameter.ParameterType);
            }
        }

        private object GetImplementationParameter(Type type, ParameterInfo parameter)
        {
            return _registrations[type].Parameters.First(p => p.ParameterName == parameter.Name).Parameter;
        }

        private bool ImplementationHasTheParameter(Type type, ParameterInfo parameter)
        {
            var typesOwnParameters = _registrations[type].Parameters;
            foreach (var implementationParameter in typesOwnParameters)
            {
                if (implementationParameter.ParameterName == parameter.Name)
                    return true;
            }
            return false;
        }

        private ConstructorInfo GetConstructorICanConstruct(Type type)
        {
            foreach (var constructor in _registrations[type].TypeOfImplementation.GetConstructors())
            {
                var doIHaveAllParamaters = true;
                var parameters = constructor.GetParameters().ToList();
                foreach (var parameter in parameters)
                    if (ImplementationDoesntHaveParameter(type, parameter) && WeCannotResolve(parameter))
                        doIHaveAllParamaters = false;
                if (doIHaveAllParamaters) return constructor;
            }
            throw new NoValidConstructor($"{type.Name} has no constructor that i know types of all parameters for");
        }

        private bool ImplementationDoesntHaveParameter(Type type, ParameterInfo parameter)
        {
            return !ImplementationHasTheParameter(type, parameter);
        }

        private bool WeCannotResolve(ParameterInfo parameter)
        {
            return !_registrations.ContainsKey(parameter.ParameterType);
        }

        public static MyIoC Default { get; } = new MyIoC();
    }
}