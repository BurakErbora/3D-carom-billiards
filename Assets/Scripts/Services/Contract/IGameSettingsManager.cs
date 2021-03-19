using CaromBilliards3D.DataModel;
using CaromBilliards3D.Utility;

namespace CaromBilliards3D.Services
{
    public interface IGameSettingsManager : IBaseService
    {
        public GameSettingsData gameSettings { get; set; }
        public void LoadGameSettings(string directoryPath, string fileName, string fileExtension = ".dat");
        public void SaveGameSettings(string directoryPath, string fileName, string fileExtension = ".dat");
    }
}
