using CaromBilliards3D.Utility;
using UnityEngine;

namespace CaromBilliards3D.Controller
{
    // Statemachine implementation for Monobehaviours (for use with Controllers and Managers).
    // Delegates the Update() method to "state" delegates assigned in the stateHolder.

    public abstract class MonoBehaviourWithStates : MonoBehaviour
    {
        protected StateHolder stateHolder = new StateHolder();

        protected virtual void Update()
        {
            if (stateHolder.IsUpdateInterval())
                stateHolder.UpdateStates();
        }
    
    }
}