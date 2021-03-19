using System;
using UnityEngine;

namespace CaromBilliards3D.DataModel
{
    [Serializable]
    public class GameSettings //: IGameSettings
    {
        //[SerializeField]
        public float audioVolume = 1;

        //float IGameSettings.audioVolume { get => _audioVolume; set => _audioVolume = value; }
    }
}