using CSharpEngineerQuest.Data;
using CSharpEngineerQuest.Game;
using UnityEngine;
using UnityEngine.UI;

namespace CSharpEngineerQuest.UI
{
    /// <summary>
    /// Attach to HomeScene's canvas. Shows the player's level/XP and starts
    /// the flow toward the World Map.
    /// </summary>
    public class HomeController : MonoBehaviour
    {
        [SerializeField] private Text _playerLevelText;
        [SerializeField] private Text _xpText;
        [SerializeField] private Button _startButton;

        private void Start()
        {
            PlayerProgress progress = GameManager.Instance.PlayerProgress;
            _playerLevelText.text = $"Level {progress.level}";
            _xpText.text = $"XP {progress.xp}";

            _startButton.onClick.AddListener(OnStartClicked);
        }

        private void OnStartClicked()
        {
            GameManager.Instance.GoToWorldMap();
        }
    }
}
