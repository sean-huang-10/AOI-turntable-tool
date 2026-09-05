using System;
using System.Collections.Generic;

namespace CSharpEngineerQuest.Data
{
    /// <summary>
    /// Star rating earned for one completed level. Kept as a list of records
    /// instead of a Dictionary because Unity's JsonUtility cannot serialize
    /// dictionaries.
    /// </summary>
    [Serializable]
    public class LevelStarRecord
    {
        public int levelId;
        public int stars;
    }

    /// <summary>
    /// Everything SaveManager persists for one player. New players start on
    /// Level 1 already unlocked so the World Map has something to show.
    /// </summary>
    [Serializable]
    public class PlayerProgress
    {
        public int level = 1;
        public int xp = 0;
        public int coins = 0;
        public List<int> unlockedLevels = new List<int> { 1 };
        public List<int> completedLevels = new List<int>();
        public List<LevelStarRecord> stars = new List<LevelStarRecord>();
    }
}
