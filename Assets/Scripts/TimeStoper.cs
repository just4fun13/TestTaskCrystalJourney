using UnityEngine;

namespace Assets.Scripts
{
    public class TimeStoper : MonoBehaviour
    {
        private void Awake()
        {
            TimeStoper.PauseTime();
        }

        public static void PauseTime()
        {
            Time.timeScale = 0f;
        }

        public static void ResumeTime()
        {
            Time.timeScale = 1f;
        }
    }
}
