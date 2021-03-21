using CaromBilliards3D.DataModel;

namespace CaromBilliards3D.Services
{
    public interface IGameSettingsService : IBaseService
    {
        public GameSettingsData gameSettings { get; set; }
        public void LoadGameSettings(string directoryPath, string fileName, string fileExtension = ".dat");
        public void SaveGameSettings(string directoryPath, string fileName, string fileExtension = ".dat");
    }
}
