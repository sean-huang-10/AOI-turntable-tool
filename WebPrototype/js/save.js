// Same role as Assets/Scripts/Save/SaveManager.cs, backed by localStorage
// instead of a JSON file - this is what makes progress survive a page
// reload in the browser.
const SAVE_KEY = "csharpEngineerQuest.playerProgress";

function loadProgress() {
  const raw = localStorage.getItem(SAVE_KEY);

  if (!raw) {
    return { level: 1, xp: 0, coins: 0, unlockedLevels: [1], completedLevels: [], stars: [] };
  }

  return JSON.parse(raw);
}

function saveProgress(progress) {
  localStorage.setItem(SAVE_KEY, JSON.stringify(progress));
}
