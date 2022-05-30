using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Player : Unit
    {
        public delegate void PlayerGotCrystalEvent(GameObject crystalObj);
        public static event PlayerGotCrystalEvent PlayerGotCrystal;
        public delegate void PlayerTouchEnemyEvent(GameObject enemyObj);
        public static event PlayerTouchEnemyEvent PlayerTouchEnemy;
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
            animator.SetBool("IsRun", !IsArived);
        }

        private void OnCollisionEnter(Collision collision)
        {
            ResetPath();
            if (inImmortalMode)
                return;
            if (collision.gameObject.CompareTag("Enemy"))
            {
                if (PlayerTouchEnemy != null)
                    PlayerTouchEnemy(collision.gameObject);
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
                collision.gameObject.SetActive(false);
            }
            if (collision.gameObject.CompareTag("Crystal"))
            {
                if (PlayerGotCrystal != null)
                    PlayerGotCrystal(collision.gameObject);
                Player.stats.GetCrystal();
                collision.gameObject.SetActive(false);
            }    
        }
    }
}