using CSharpEngineerQuest.Game;
using CSharpEngineerQuest.Judge;
using UnityEngine;
using UnityEngine.UI;

namespace CSharpEngineerQuest.UI
{
    /// <summary>
    /// Attach to ResultScene's canvas. Reads GameManager.LastResult to show
    /// success ("LEVEL COMPLETE" + XP + Next Level) or failure ("CODE ERROR"
    /// + Try Again). Stars are fixed at full for the MVP since star
    /// conditions aren't part of LevelData yet.
    /// </summary>
    public class ResultController : MonoBehaviour
    {
        [SerializeField] private GameObject _successPanel;
        [SerializeField] private GameObject _failurePanel;
        [SerializeField] private Text _xpRewardText;
        [SerializeField] private Button _nextLevelButton;
        [SerializeField] private Button _tryAgainButton;

        private void Start()
        {
            bool isCorrect = GameManager.Instance.LastResult == JudgeResult.Correct;

            _successPanel.SetActive(isCorrect);
            _failurePanel.SetActive(!isCorrect);

            if (isCorrect)
            {
                _xpRewardText.text = $"XP +{GameManager.Instance.CurrentLevel.xpReward}";
                _nextLevelButton.onClick.AddListener(OnNextLevelClicked);
            }
            else
            {
                _tryAgainButton.onClick.AddListener(OnTryAgainClicked);
            }
        }

        private void OnNextLevelClicked()
        {
            GameManager.Instance.GoToNextLevel();
        }

        private void OnTryAgainClicked()
        {
            GameManager.Instance.RetryLevel();
        }
    }
}
