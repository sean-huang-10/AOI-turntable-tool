using UnityEngine.SceneManagement;

namespace CSharpEngineerQuest.Core
{
    /// <summary>
    /// Centralizes scene names so no other script hardcodes a scene path
    /// string. Purely a loading mechanism - deciding WHEN to go to which
    /// scene is GameManager's job, not this class's.
    /// </summary>
    public static class SceneController
    {
        private const string BootScene = "BootScene";
        private const string HomeScene = "HomeScene";
        private const string WorldMapScene = "WorldMapScene";
        private const string LevelScene = "LevelScene";
        private const string ResultScene = "ResultScene";

        public static void LoadBootScene()
        {
            SceneManager.LoadScene(BootScene);
        }

        public static void LoadHomeScene()
        {
            SceneManager.LoadScene(HomeScene);
        }

        public static void LoadWorldMapScene()
        {
            SceneManager.LoadScene(WorldMapScene);
        }

        public static void LoadLevelScene()
        {
            SceneManager.LoadScene(LevelScene);
        }

        public static void LoadResultScene()
        {
            SceneManager.LoadScene(ResultScene);
        }
    }
}
