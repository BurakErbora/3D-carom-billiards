using CaromBilliards3D.DataModel;
using CaromBilliards3D.Utility;

namespace CaromBilliards3D.Services
{
    public interface IGameSessionManager : IBaseService
    {
        public GameSessionData gameSessionData { get; set; }
        public float GetTimePlayed();
        public int GetScore();
        public int GetShotsTaken();
        public void SetTimePlayed(float timePlayed);
        public void SetScore(int score);
        public void SetShotsTaken(int shotsTaken);
        public void LoadGameSessionData(string directoryPath, string fileName, string fileExtension = ".dat");
        public void SaveGameSessionData(string directoryPath, string fileName, string fileExtension = ".dat");
    }
}
