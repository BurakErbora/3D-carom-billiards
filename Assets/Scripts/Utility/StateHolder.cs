using UnityEngine;

namespace CaromBilliards3D.Utility
{
    public class StateHolder
    {
        

        public delegate void UpdateDelegate();
        public UpdateDelegate[] updateDelegates;

        private int _state = 0;
        private float _updateInterval = 0f;
        private float _lastUpdate = 0f;

        public void UpdateStates()
        {
            _lastUpdate = Time.time;

            if (updateDelegates != null && updateDelegates.Length > _state && updateDelegates[_state] != null)
                updateDelegates[_state]();
        }

        public bool IsUpdateInterval()
        {
            if (_updateInterval == 0 || Time.timeScale > 0 && Time.time - _lastUpdate > _updateInterval)
                return true;
            return false;
        }

        public void SetUpdateInterval(float updateIntervalInSeconds)
        {
            _updateInterval = updateIntervalInSeconds;
        }

        public void SetState(int state)
        {
            _state = state;
        }
    }
}