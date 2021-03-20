using System;
using System.Collections.Generic;
using UnityEngine;

namespace CaromBilliards3D.Utility
{
    public class ServiceLocator
    {
        private ServiceLocator() { }
        private readonly Dictionary<string, IBaseService> _services = new Dictionary<string, IBaseService>();

        public static ServiceLocator Current { get; private set; }

        public static void Initiailze()
        {
            Current = new ServiceLocator();
        }

        public static T Resolve<T>() where T: IBaseService
        {
            return Current.Get<T>();
        }
        
        public T Get<T>() where T : IBaseService
        {
            string key = typeof(T).Name;
            if (!_services.ContainsKey(key))
                Debug.LogError($"{key} not registered with {GetType().Name}");

            return (T)_services[key];
        }

        public Type GetType<T>() where T : IBaseService
        {
            string key = typeof(T).Name;
            if (!_services.ContainsKey(key))
                Debug.LogError($"{key} not registered with {GetType().Name}");

            return typeof(T);
        }

        public void Register<T>(T service) where T : IBaseService
        {
            string key = typeof(T).Name;
            if (_services.ContainsKey(key))
            {
                Debug.LogError($"Attempted to register service of type {key} which is already registered with the {GetType().Name}.");
                return;
            }

            _services.Add(key, service);
        }

        public void Unregister<T>() where T : IBaseService
        {
            string key = typeof(T).Name;
            if (!_services.ContainsKey(key))
            {
                Debug.LogError($"Attempted to unregister service of type {key} which is not registered with the {GetType().Name}.");
                return;
            }

            _services.Remove(key);
        }
    }
}