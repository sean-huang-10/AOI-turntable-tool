using CSharpEngineerQuest.Data;

namespace CSharpEngineerQuest.Learning
{
    /// <summary>
    /// Exposes the current level's question content (question text, starter
    /// code, question type, sequential hints) and holds the player's in-progress
    /// answer. Does not judge the answer - that is JudgeManager's job.
    /// </summary>
    public class QuestionManager
    {
        private LevelData _currentLevelData;
        private int _hintsRevealed;

        public string PlayerAnswer { get; set; }

        public string Question => _currentLevelData.question;
        public string StarterCode => _currentLevelData.starterCode;
        public QuestionType QuestionType => _currentLevelData.GetQuestionType();
        public int HintsRevealedCount => _hintsRevealed;

        public void LoadQuestion(LevelData levelData)
        {
            _currentLevelData = levelData;
            _hintsRevealed = 0;
            PlayerAnswer = levelData.starterCode;
        }

        /// <summary>
        /// Reveals the next hint in order. Returns null once every hint for
        /// this level has already been shown - callers should disable the
        /// Hint button in that case.
        /// </summary>
        public string GetNextHint()
        {
            if (_currentLevelData.hints == null || _hintsRevealed >= _currentLevelData.hints.Length)
            {
                return null;
            }

            string hint = _currentLevelData.hints[_hintsRevealed];
            _hintsRevealed++;
            return hint;
        }
    }
}
