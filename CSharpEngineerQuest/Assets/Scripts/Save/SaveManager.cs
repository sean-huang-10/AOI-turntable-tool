using System.IO;
using CSharpEngineerQuest.Data;
using UnityEngine;

namespace CSharpEngineerQuest.Save
{
    /// <summary>
    /// Reads and writes PlayerProgress as a single local JSON file under
    /// Application.persistentDataPath. First version is local file storage
    /// only - no cloud save.
    /// </summary>
    public class SaveManager
    {
        private const string SaveFileName = "player_progress.json";

        private string SavePath => Path.Combine(Application.persistentDataPath, SaveFileName);

        /// <summary>
        /// Returns a fresh PlayerProgress (Level 1 unlocked, 0 XP) if no save
        /// file exists yet, e.g. on first launch.
        /// </summary>
        public PlayerProgress Load()
        {
            if (!File.Exists(SavePath))
            {
                return new PlayerProgress();
            }

            string json = File.ReadAllText(SavePath);
            return JsonUtility.FromJson<PlayerProgress>(json);
        }

        public void Save(PlayerProgress progress)
        {
            string json = JsonUtility.ToJson(progress, true);
            File.WriteAllText(SavePath, json);
        }
    }
}
