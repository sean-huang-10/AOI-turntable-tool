namespace CSharpEngineerQuest.Judge
{
    /// <summary>
    /// Judges a player's answer against a level's expected answer. Takes its
    /// ICodeJudge by constructor injection so the underlying judging strategy
    /// (RuleBasedJudge today, a RemoteCodeJudge later) can be swapped without
    /// touching this class or its callers.
    /// </summary>
    public class JudgeManager
    {
        private readonly ICodeJudge _codeJudge;

        public JudgeManager(ICodeJudge codeJudge)
        {
            _codeJudge = codeJudge;
        }

        public JudgeResult Judge(string playerAnswer, string expectedAnswer)
        {
            return _codeJudge.IsCorrect(playerAnswer, expectedAnswer)
                ? JudgeResult.Correct
                : JudgeResult.Wrong;
        }
    }
}
