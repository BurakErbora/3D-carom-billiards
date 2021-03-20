using System;

namespace CaromBilliards3D.DataModel
{
    [Serializable]
    public class GameSessionData
    {
        public int timePlayed;
        public int shotsTaken;
        [NonSerialized] public int score; // no need to; will always be the same with Constants.GAMEPLAY_TOTAL_SCORES_TO_WIN
    }
}