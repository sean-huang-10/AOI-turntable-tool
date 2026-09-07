using System.Text.RegularExpressions;

namespace CSharpEngineerQuest.Judge
{
    /// <summary>
    /// Compares code by token, not by raw string, so formatting differences
    /// like spacing around "=" or "(" don't count as wrong. It still tells
    /// "machineReady" apart from "machine Ready", since those tokenize
    /// differently.
    /// </summary>
    public class RuleBasedJudge : ICodeJudge
    {
        private static readonly Regex TokenPattern = new Regex(@"\w+|[^\s\w]");

        public bool IsCorrect(string playerAnswer, string expectedAnswer)
        {
            if (playerAnswer == null || expectedAnswer == null)
            {
                return false;
            }

            string[] playerTokens = Tokenize(playerAnswer);
            string[] expectedTokens = Tokenize(expectedAnswer);

            if (playerTokens.Length != expectedTokens.Length)
            {
                return false;
            }

            for (int i = 0; i < playerTokens.Length; i++)
            {
                if (playerTokens[i] != expectedTokens[i])
                {
                    return false;
                }
            }

            return true;
        }

        private static string[] Tokenize(string code)
        {
            MatchCollection matches = TokenPattern.Matches(code);
            string[] tokens = new string[matches.Count];
            for (int i = 0; i < matches.Count; i++)
            {
                tokens[i] = matches[i].Value;
            }

            return tokens;
        }
    }
}
