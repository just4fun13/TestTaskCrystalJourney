using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class SceneSwitcher : MonoBehaviour
    {
        public static void RestartTheGame()
        {
            SceneManager.LoadScene(0);
        }

    }
}
