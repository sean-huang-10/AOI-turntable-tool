using UnityEngine;
using UnityEngine.UI;

namespace CSharpEngineerQuest.UI
{
    /// <summary>
    /// One level entry in the World Map, pre-placed in the scene. Only holds
    /// its own levelId and view state - WorldMapController reads/writes it
    /// but never branches logic on which level a given button represents.
    /// </summary>
    public class LevelButtonView : MonoBehaviour
    {
        [SerializeField] private int _levelId;
        [SerializeField] private Button _button;
        [SerializeField] private Text _label;
        [SerializeField] private GameObject _lockedOverlay;

        public int LevelId => _levelId;
        public Button Button => _button;

        public void SetLabel(string text)
        {
            _label.text = text;
        }

        public void SetLocked(bool isLocked)
        {
            _button.interactable = !isLocked;
            _lockedOverlay.SetActive(isLocked);
        }
    }
}
