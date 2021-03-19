using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


namespace CaromBilliards3D.Utility
{
    public static class SaveLoadUtility
    {

        public static void SaveJsonData<T>(T serializableObject, string directoryPath, string fileName, string fileExtension = ".dat")
        {
            directoryPath = $"{directoryPath}//";

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            string filePath = directoryPath + fileName + fileExtension;

            if (File.Exists(filePath))
                File.Delete(filePath);

            string jsonString = JsonUtility.ToJson(serializableObject, true);
            File.WriteAllText(filePath, jsonString);
        }

        public static bool LoadJsonData<T>(out T serializableObject, string directoryPath, string fileName, string fileExtension = ".dat")
        {
            directoryPath = $"{directoryPath}//";
            string filePath = directoryPath + fileName + fileExtension;

            if (File.Exists(filePath))
            {
                string jsonString = File.ReadAllText(filePath);
                serializableObject = JsonUtility.FromJson<T>(jsonString);
                return true;
            }

            Debug.LogError($"Data not found at path: {filePath}");
            serializableObject = default(T);
            return false;
        }

        public static bool IsFileExists(string directoryPath, string fileName, string fileExtension = ".dat")
        {
            directoryPath = $"{directoryPath}//";
            string filePath = directoryPath + fileName + fileExtension;

            return File.Exists(filePath);
        }
    }
}