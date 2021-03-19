using CaromBilliards3D.Utility;
using UnityEngine;

namespace CaromBilliards3D.Controller
{
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