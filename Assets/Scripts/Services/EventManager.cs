using System.Collections.Generic;
using System;

namespace CaromBilliards3D.Services
{
    public class EventManager : IEventManager
    {
        private Dictionary<string, Action> _eventDictionary = new Dictionary<string, Action>();
        private Dictionary<string, Action<object>> _eventDictionaryWithSingleParameter = new Dictionary<string, Action<object>>();

        public void StartListening(string eventName, Action listener)
        {
            if (_eventDictionary.ContainsKey(eventName))
                _eventDictionary[eventName] += listener;
            else
                _eventDictionary.Add(eventName, listener);
        }

        // not the best way to send generic parameters but it'll have to do.
        // Subscribing methods will need to cast the parameter to their own known type.
        public void StartListening(string eventName, Action<object> listener)
        {
            if (_eventDictionaryWithSingleParameter.ContainsKey(eventName))
                _eventDictionaryWithSingleParameter[eventName] += listener;
            else
                _eventDictionaryWithSingleParameter.Add(eventName, listener);
        }


        public void StopListening(string eventName, Action listener)
        {
            if (_eventDictionary.ContainsKey(eventName))
                _eventDictionary[eventName] -= listener;
        }

        public void StopListening(string eventName, Action<object> listener)
        {
            if (_eventDictionaryWithSingleParameter.ContainsKey(eventName))
                _eventDictionaryWithSingleParameter[eventName] -= listener;
        }

        public void TriggerEvent(string eventName)
        {
            Action thisEvent = null;
            if (_eventDictionary.TryGetValue(eventName, out thisEvent))
                thisEvent?.Invoke();
        }

        public void TriggerEvent(string eventName, object partameter)
        {
            Action<object> thisEvent = null;
            if (_eventDictionaryWithSingleParameter.TryGetValue(eventName, out thisEvent))
                thisEvent?.Invoke(partameter);
        }


    }
}