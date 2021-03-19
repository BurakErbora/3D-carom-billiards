using CaromBilliards3D.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaromBilliards3D.Services
{
    public interface IEventManager : IBaseService
    {
        public void StartListening(string eventName, Action listener);
        public void StartListening(string eventName, Action<object> listener);
        public void StopListening(string eventName, Action listener);
        public void StopListening(string eventName, Action<object> listener);
        public void TriggerEvent(string eventName);
        public void TriggerEvent(string eventName, object partameter);
    }
}