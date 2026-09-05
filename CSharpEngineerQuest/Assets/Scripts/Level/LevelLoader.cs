using System.Collections.Generic;
using System.Linq;
using CSharpEngineerQuest.Data;
using UnityEngine;

namespace CSharpEngineerQuest.Level
{
    /// <summary>
    /// Reads level JSON files out of Assets/Resources/Levels and turns them
    /// into LevelData. This is the only class that talks to Resources/JsonUtility;
    /// LevelManager (Phase 2) will build on top of it for lookup/unlock logic.
    /// </summary>
    public class LevelLoader
    {
        private const string LevelsResourcePath = "Levels";

        public List<LevelData> LoadAllLevels()
        {
            TextAsset[] levelFiles = Resources.LoadAll<TextAsset>(LevelsResourcePath);
            List<LevelData> levels = new List<LevelData>();

            foreach (TextAsset file in levelFiles)
            {
                LevelData level = JsonUtility.FromJson<LevelData>(file.text);
                levels.Add(level);
            }

            return levels.OrderBy(level => level.levelId).ToList();
        }

        public LevelData LoadLevel(int levelId)
        {
            return LoadAllLevels().FirstOrDefault(level => level.levelId == levelId);
        }
    }
}
