using CSharpEngineerQuest.Core;
using UnityEngine;

namespace CSharpEngineerQuest.Game
{
    /// <summary>
    /// Attach next to GameManager in BootScene. Unity runs every Awake() in a
    /// scene before any Start(), so GameManager has already set up its
    /// singleton and DontDestroyOnLoad by the time this leaves for Home.
    /// </summary>
    public class BootController : MonoBehaviour
    {
        private void Start()
        {
            SceneController.LoadHomeScene();
        }
    }
}
