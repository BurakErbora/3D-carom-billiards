using CaromBilliards3D.DataModel;
using CaromBilliards3D.Utility;

namespace CaromBilliards3D.Services
{
    public interface IGameManager : IBaseService
    {
        public GameSettings gameSettings { get; set; }
        public void LoadGameSettings(string directoryPath, string fileName, string fileExtension = ".dat");
        public void SaveGameSettings(string directoryPath, string fileName, string fileExtension = ".dat");
        public void InitializeGameSettings();
    }
}
