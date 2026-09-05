using System;

namespace CSharpEngineerQuest.Data
{
    /// <summary>
    /// Plain data model for one level, loaded from Resources/Levels/*.json.
    /// Field names match the JSON schema exactly, since Unity's JsonUtility
    /// maps JSON keys to fields by name.
    /// </summary>
    [Serializable]
    public class LevelData
    {
        public int levelId;
        public int worldId;
        public string title;
        public string topic;
        public int difficulty;

        // Stored as string because Unity's JsonUtility cannot parse a JSON
        // string value (e.g. "FillCode") straight into an enum field.
        public string questionType;

        public string story;
        public string question;
        public string starterCode;
        public string expectedAnswer;
        public string[] hints;
        public int xpReward;

        public QuestionType GetQuestionType()
        {
            return (QuestionType)Enum.Parse(typeof(QuestionType), questionType);
        }
    }
}
