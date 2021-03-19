using System;
using UnityEngine;

namespace CaromBilliards3D.DataModel
{
    [Serializable]
    public class GameSettingsData //: IGameSettings
    {
        //[SerializeField]
        public float audioVolume = 1f;

        //float IGameSettings.audioVolume { get => _audioVolume; set => _audioVolume = value; }
    }
}