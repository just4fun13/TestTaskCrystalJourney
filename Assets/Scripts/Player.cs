using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Player : Unit
    {
        public delegate void BackToPoolEvent(GameObject crystalObj);
        public static event BackToPoolEvent BackToPool;

        private const string enemyTag = "Enemy";
        private const string crystalTag = "Crystal";
        private const string runAnimBoolParameterName = "IsRun";

        // we have only one player
        private static PlayerStat stats = new PlayerStat();
        public static int Scores => stats.crystalCount;
        public static int Lifes => stats.lifesCount;

        private bool inImmortalMode = false;
        private float immortalTimeInSeconds = 3f;
        [SerializeField] private Animator animator;

        private void Start()
        {
            stats = new PlayerStat();
        }
        private IEnumerator ImmortalityForAWhile()
        {
            inImmortalMode = true;
            yield return new WaitForSeconds(immortalTimeInSeconds);
            inImmortalMode = false;
        }

        private void TryRecord()
        {
            int bestResult = 0;
            
            if (PlayerPrefs.HasKey("Best"))
                bestResult = PlayerPrefs.GetInt("Best");
            if (Player.stats.crystalCount > bestResult)
                PlayerPrefs.SetInt("Best", Player.stats.crystalCount);
        }

        private void Update()
        {
            base.Update();
            animator.SetBool(runAnimBoolParameterName, !IsArived);
        }

        private void OnCollisionEnter(Collision collision)
        {
            ResetPath();
            if (inImmortalMode)
                return;
            if (collision.gameObject.CompareTag(enemyTag) || collision.gameObject.CompareTag(crystalTag))
            {
                if (collision.gameObject.CompareTag(enemyTag))
                {
                    Player.stats.GotDamaged();
                    if (Player.stats.IsAlive)
                    {
                        StartCoroutine(ImmortalityForAWhile());
                    }
                    else
                    {
                        TryRecord();
                        SceneSwitcher.RestartTheGame();
                    }
                }
                else
                {
                    Player.stats.GetCrystal();
                }
                if (BackToPool != null)
                    BackToPool(collision.gameObject);
            }
        }
    }
}