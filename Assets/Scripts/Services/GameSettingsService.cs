using CaromBilliards3D.DataModel;
using CaromBilliards3D.Utility;

namespace CaromBilliards3D.Services
{
    public class GameSettingsService : IGameSettingsService
    {
        private GameSettingsData _gameSettings = new GameSettingsData();

        GameSettingsData IGameSettingsService.gameSettings { get => _gameSettings; set => _gameSettings = value; }

        public void LoadGameSettings(string directoryPath, string fileName, string fileExtension = ".dat")
        {
            if (!SaveLoadUtility.IsFileExists(directoryPath, fileName, fileExtension = ".dat"))
                return;

            SaveLoadUtility.LoadJsonData(out _gameSettings, directoryPath, fileName, fileExtension = ".dat");
        }

        public void SaveGameSettings(string directoryPath, string fileName, string fileExtension = ".dat")
        {
            SaveLoadUtility.SaveJsonData(_gameSettings, directoryPath, fileName, fileExtension);
        }
    }
}

