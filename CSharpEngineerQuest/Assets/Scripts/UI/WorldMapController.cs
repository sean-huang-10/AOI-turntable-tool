using CSharpEngineerQuest.Data;
using CSharpEngineerQuest.Game;
using UnityEngine;

namespace CSharpEngineerQuest.UI
{
    /// <summary>
    /// Attach to WorldMapScene's canvas. Drives level buttons already placed
    /// in the scene - title text and locked state come from GameManager/level
    /// data, never from a per-level branch in this script.
    /// </summary>
    public class WorldMapController : MonoBehaviour
    {
        [SerializeField] private LevelButtonView[] _levelButtons;

        private void Start()
        {
            foreach (LevelButtonView levelButton in _levelButtons)
            {
                int levelId = levelButton.LevelId;
                LevelData level = GameManager.Instance.GetLevel(levelId);

                levelButton.SetLabel(level != null ? level.title : $"Level {levelId}");
                levelButton.SetLocked(!GameManager.Instance.IsLevelUnlocked(levelId));
                levelButton.Button.onClick.AddListener(() => GameManager.Instance.StartLevel(levelId));
            }
        }
    }
}
