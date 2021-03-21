using CaromBilliards3D.DataModel;

namespace CaromBilliards3D.Services
{
    public interface IGameSessionService : IBaseService
    {
        public GameSessionData gameSessionData { get; set; }
        public int GetTimePlayed();
        public int GetScore();
        public int GetShotsTaken();
        public void SetTimePlayed(int timePlayed);
        public void SetScore(int score);
        public void SetShotsTaken(int shotsTaken);
        public void LoadGameSessionData(string directoryPath, string fileName, string fileExtension = ".dat");
        public void SaveGameSessionData(string directoryPath, string fileName, string fileExtension = ".dat");
        public void ResetSession();
    }
}
