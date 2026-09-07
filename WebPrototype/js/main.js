// Same role as Assets/Scripts/Game/GameManager.cs: owns flow/state and
// switches "scenes" (screens). Each render* function is the only place that
// touches its screen's DOM; the flow functions below just decide what
// happens next.
let progress = loadProgress();
let currentLevel = null;
let hintsRevealed = 0;
let lastResult = null; // "correct" | "wrong"

function showScreen(screenId) {
  document.querySelectorAll(".screen").forEach(el => el.classList.remove("active"));
  document.getElementById(screenId).classList.add("active");
}

function goHome() {
  document.getElementById("home-level").textContent = progress.level;
  document.getElementById("home-xp").textContent = progress.xp;
  showScreen("screen-home");
}

function goWorldMap() {
  const list = document.getElementById("level-list");
  list.innerHTML = "";

  LEVELS.forEach(level => {
    const unlocked = isLevelUnlocked(progress, level.levelId);

    const card = document.createElement("div");
    card.className = "level-card" + (unlocked ? "" : " locked");
    card.innerHTML =
      '<div>' +
      '<p class="level-card-title">Level ' + level.levelId + '・' + level.title + '</p>' +
      '<p class="level-card-sub">' + level.topic + '</p>' +
      '</div>' +
      '<div class="level-card-badge">' + (unlocked ? "▶" : "🔒") + '</div>';

    if (unlocked) {
      card.addEventListener("click", () => startLevel(level.levelId));
    }

    list.appendChild(card);
  });

  showScreen("screen-worldmap");
}

function startLevel(levelId) {
  currentLevel = getLevel(levelId);
  hintsRevealed = 0;

  document.getElementById("level-title").textContent = "Level " + currentLevel.levelId + "・" + currentLevel.title;
  document.getElementById("level-topic").textContent = currentLevel.topic;
  document.getElementById("level-story").textContent = currentLevel.story;
  document.getElementById("level-question").textContent = currentLevel.question;
  document.getElementById("level-code-input").value = currentLevel.starterCode;
  document.getElementById("level-hint").textContent = "";

  showScreen("screen-level");
}

function showNextHint() {
  if (!currentLevel.hints || hintsRevealed >= currentLevel.hints.length) {
    return;
  }

  document.getElementById("level-hint").textContent = currentLevel.hints[hintsRevealed];
  hintsRevealed++;
}

function submitAnswer() {
  const answer = document.getElementById("level-code-input").value;
  lastResult = isCorrect(answer, currentLevel.expectedAnswer) ? "correct" : "wrong";

  if (lastResult === "correct") {
    progress.xp += currentLevel.xpReward;

    if (!progress.completedLevels.includes(currentLevel.levelId)) {
      progress.completedLevels.push(currentLevel.levelId);
    }

    unlockNextLevel(progress, currentLevel.levelId);
    saveProgress(progress);
  }

  renderResult();
  showScreen("screen-result");
}

function renderResult() {
  const isSuccess = lastResult === "correct";
  document.getElementById("result-success").classList.toggle("active", isSuccess);
  document.getElementById("result-failure").classList.toggle("active", !isSuccess);

  if (isSuccess) {
    document.getElementById("result-xp").textContent = "XP +" + currentLevel.xpReward;
  }
}

function goToNextLevel() {
  const nextLevel = getNextLevel(currentLevel.levelId);

  if (nextLevel) {
    startLevel(nextLevel.levelId);
  } else {
    goWorldMap();
  }
}

function retryLevel() {
  startLevel(currentLevel.levelId);
}

document.getElementById("btn-start").addEventListener("click", goWorldMap);
document.getElementById("btn-worldmap-back").addEventListener("click", goHome);
document.getElementById("btn-level-back").addEventListener("click", goWorldMap);
document.getElementById("btn-hint").addEventListener("click", showNextHint);
document.getElementById("btn-submit").addEventListener("click", submitAnswer);
document.getElementById("btn-next-level").addEventListener("click", goToNextLevel);
document.getElementById("btn-try-again").addEventListener("click", retryLevel);

goHome();
