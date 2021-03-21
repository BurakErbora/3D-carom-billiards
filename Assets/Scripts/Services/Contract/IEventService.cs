using System;

namespace CaromBilliards3D.Services
{
    public interface IEventService : IBaseService
    {
        public void StartListening(string eventName, Action listener);
        public void StartListening(string eventName, Action<object> listener);
        public void StopListening(string eventName, Action listener);
        public void StopListening(string eventName, Action<object> listener);
        public void TriggerEvent(string eventName);
        public void TriggerEvent(string eventName, object partameter);
    }
}