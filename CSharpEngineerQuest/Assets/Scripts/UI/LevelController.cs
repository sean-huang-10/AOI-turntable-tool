using CSharpEngineerQuest.Data;
using CSharpEngineerQuest.Game;
using UnityEngine;
using UnityEngine.UI;

namespace CSharpEngineerQuest.UI
{
    /// <summary>
    /// Attach to LevelScene's canvas. Shows the current level's story/mission,
    /// hosts the code input box, and forwards Hint/Submit taps to GameManager.
    /// Set _codeInput's Line Type to Multi Line Newline in the Inspector so
    /// players can write multi-line code.
    /// </summary>
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private Text _titleText;
        [SerializeField] private Text _storyText;
        [SerializeField] private Text _missionText;
        [SerializeField] private InputField _codeInput;
        [SerializeField] private Text _hintText;
        [SerializeField] private Button _hintButton;
        [SerializeField] private Button _submitButton;

        private void Start()
        {
            LevelData level = GameManager.Instance.CurrentLevel;

            _titleText.text = level.title;
            _storyText.text = level.story;
            _missionText.text = level.question;
            _codeInput.text = GameManager.Instance.GetStarterCode();
            _hintText.text = string.Empty;

            _hintButton.onClick.AddListener(OnHintClicked);
            _submitButton.onClick.AddListener(OnSubmitClicked);
        }

        private void OnHintClicked()
        {
            string hint = GameManager.Instance.GetNextHint();

            if (hint != null)
            {
                _hintText.text = hint;
            }
        }

        private void OnSubmitClicked()
        {
            GameManager.Instance.SetPlayerAnswer(_codeInput.text);
            GameManager.Instance.SubmitAnswer();
        }
    }
}
