using System.Collections.Generic;
using System.Linq;
using CSharpEngineerQuest.Data;

namespace CSharpEngineerQuest.Level
{
    /// <summary>
    /// Looks up level content and decides unlock rules. Does not own or persist
    /// PlayerProgress - that belongs to SaveManager/PlayerProgressManager (later
    /// phases), which call in here and pass their PlayerProgress instance.
    /// </summary>
    public class LevelManager
    {
        private readonly List<LevelData> _levels;

        public LevelData CurrentLevel { get; private set; }

        public LevelManager(LevelLoader levelLoader)
        {
            _levels = levelLoader.LoadAllLevels();
        }

        public LevelData GetLevel(int levelId)
        {
            return _levels.FirstOrDefault(level => level.levelId == levelId);
        }

        /// <summary>
        /// Levels are sequential by levelId for the MVP's linear world map.
        /// </summary>
        public LevelData GetNextLevel(int currentLevelId)
        {
            return GetLevel(currentLevelId + 1);
        }

        public void SetCurrentLevel(int levelId)
        {
            CurrentLevel = GetLevel(levelId);
        }

        public bool IsLevelUnlocked(PlayerProgress progress, int levelId)
        {
            return progress.unlockedLevels.Contains(levelId);
        }

        public void UnlockLevel(PlayerProgress progress, int levelId)
        {
            if (!progress.unlockedLevels.Contains(levelId))
            {
                progress.unlockedLevels.Add(levelId);
            }
        }

        /// <summary>
        /// Call after a level is completed to unlock whatever comes after it.
        /// Does nothing if there is no next level yet (e.g. last authored level).
        /// </summary>
        public void UnlockNextLevel(PlayerProgress progress, int completedLevelId)
        {
            LevelData nextLevel = GetNextLevel(completedLevelId);
            if (nextLevel != null)
            {
                UnlockLevel(progress, nextLevel.levelId);
            }
        }
    }
}
