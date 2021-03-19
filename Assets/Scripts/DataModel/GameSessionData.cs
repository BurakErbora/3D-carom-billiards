using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaromBilliards3D.DataModel
{
    [Serializable]
    public class GameSessionData
    {
        public int timePlayed;
        public int shotsTaken;
        public int score;
    }
}