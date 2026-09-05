namespace CSharpEngineerQuest.Data
{
    /// <summary>
    /// Supported question formats. Phase 1 content only uses FillCode and Debug;
    /// the rest are reserved so QuestionManager/JudgeManager can grow into them later.
    /// </summary>
    public enum QuestionType
    {
        FillCode,
        CodeOrder,
        Debug,
        OutputPrediction,
        Coding
    }
}
