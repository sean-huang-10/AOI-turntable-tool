using CSharpEngineerQuest.Core;
using CSharpEngineerQuest.Data;
using CSharpEngineerQuest.Judge;
using CSharpEngineerQuest.Learning;
using CSharpEngineerQuest.Level;
using UnityEngine;

namespace CSharpEngineerQuest.Game
{
    /// <summary>
    /// Owns game flow and current state (current level, last judge result),
    /// and delegates the actual work to LevelManager/QuestionManager/JudgeManager.
    /// Does not touch UI, parse JSON, judge answers itself, or read/write save
    /// files - those stay in their own managers.
    ///
    /// This is the one persistent singleton in the project (it has to survive
    /// scene loads to carry state between them); the managers it owns are
    /// plain classes, not singletons themselves.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public PlayerProgress PlayerProgress { get; private set; }
        public LevelData CurrentLevel => _levelManager.CurrentLevel;
        public JudgeResult LastResult { get; private set; }

        private LevelManager _levelManager;
        private QuestionManager _questionManager;
        private JudgeManager _judgeManager;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            _levelManager = new LevelManager(new LevelLoader());
            _questionManager = new QuestionManager();
            _judgeManager = new JudgeManager(new RuleBasedJudge());
            PlayerProgress = new PlayerProgress();
        }

        public LevelData GetLevel(int levelId)
        {
            return _levelManager.GetLevel(levelId);
        }

        public bool IsLevelUnlocked(int levelId)
        {
            return _levelManager.IsLevelUnlocked(PlayerProgress, levelId);
        }

        public void StartLevel(int levelId)
        {
            _levelManager.SetCurrentLevel(levelId);
            _questionManager.LoadQuestion(_levelManager.CurrentLevel);
            SceneController.LoadLevelScene();
        }

        public string GetStarterCode()
        {
            return _questionManager.StarterCode;
        }

        public string GetNextHint()
        {
            return _questionManager.GetNextHint();
        }

        public void SetPlayerAnswer(string answer)
        {
            _questionManager.PlayerAnswer = answer;
        }

        public void SubmitAnswer()
        {
            LastResult = _judgeManager.Judge(_questionManager.PlayerAnswer, CurrentLevel.expectedAnswer);

            if (LastResult == JudgeResult.Correct)
            {
                PlayerProgress.xp += CurrentLevel.xpReward;

                if (!PlayerProgress.completedLevels.Contains(CurrentLevel.levelId))
                {
                    PlayerProgress.completedLevels.Add(CurrentLevel.levelId);
                }

                _levelManager.UnlockNextLevel(PlayerProgress, CurrentLevel.levelId);
            }

            SceneController.LoadResultScene();
        }

        public void GoToNextLevel()
        {
            LevelData nextLevel = _levelManager.GetNextLevel(CurrentLevel.levelId);

            if (nextLevel != null)
            {
                StartLevel(nextLevel.levelId);
            }
            else
            {
                SceneController.LoadWorldMapScene();
            }
        }

        public void RetryLevel()
        {
            StartLevel(CurrentLevel.levelId);
        }

        public void GoToWorldMap()
        {
            SceneController.LoadWorldMapScene();
        }
    }
}
