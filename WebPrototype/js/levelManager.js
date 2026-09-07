// Same responsibilities as Assets/Scripts/Level/LevelManager.cs: level
// lookup and unlock rules. Takes progress as a parameter rather than owning
// it, same reasoning as the Unity version - persistence is save.js's job.
function getLevel(levelId) {
  return LEVELS.find(level => level.levelId === levelId) || null;
}

function getNextLevel(currentLevelId) {
  return getLevel(currentLevelId + 1);
}

function isLevelUnlocked(progress, levelId) {
  return progress.unlockedLevels.includes(levelId);
}

function unlockLevel(progress, levelId) {
  if (!progress.unlockedLevels.includes(levelId)) {
    progress.unlockedLevels.push(levelId);
  }
}

function unlockNextLevel(progress, completedLevelId) {
  const nextLevel = getNextLevel(completedLevelId);

  if (nextLevel) {
    unlockLevel(progress, nextLevel.levelId);
  }
}
