namespace CSharpEngineerQuest.Judge
{
    /// <summary>
    /// Decides whether a player's code answers a level correctly. RuleBasedJudge
    /// is the first-version implementation; a future RemoteCodeJudge can compile
    /// and run the code server-side without JudgeManager or its callers changing.
    /// </summary>
    public interface ICodeJudge
    {
        bool IsCorrect(string playerAnswer, string expectedAnswer);
    }
}
