using UnityEngine;
using TMPro;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class UIstat : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI enemiesCountText;
        [SerializeField] private TextMeshProUGUI crystalCountText;
        [SerializeField] private TextMeshProUGUI playerLifesText;
        [SerializeField] private TextMeshProUGUI playerScoresText;
        [SerializeField] private TextMeshProUGUI distanceToEnemy;
        [SerializeField] private TextMeshProUGUI distanceToCrystal;
        [SerializeField] private TextMeshProUGUI recordText;
        private async void Start()
        {
            await Task.Delay(50);
            Enemy.EnemyHitCrystal += RefreshCrystalCount;
            Player.PlayerGotCrystal += RefreshScoresCount;
            Player.PlayerGotCrystal += RefreshCrystalCount;
            Player.PlayerTouchEnemy += RefreshLifesCount;
            CrystalFather.CrystalSpawn += RefreshCrystalCount;
            EnemyFather.EnemyCountChange += RefreshEnemyCount;
            int bestResult = 0;
            if (PlayerPrefs.HasKey("Best"))
                bestResult = PlayerPrefs.GetInt("Best");
            recordText.text = $"Рекорд: {bestResult}";
        }

        private async void OnDestroy()
        {
            Player.PlayerGotCrystal -= RefreshScoresCount;
            Player.PlayerGotCrystal -= RefreshCrystalCount;
            Player.PlayerTouchEnemy -= RefreshLifesCount;
            CrystalFather.CrystalSpawn -= RefreshCrystalCount;
            Enemy.EnemyHitCrystal -= RefreshCrystalCount;
            EnemyFather.EnemyCountChange -= RefreshEnemyCount;
        }

        private async void RefreshEnemyCount(GameObject obj)
        {
            await Task.Delay(5);
            enemiesCountText.text = EnemyFather.EnemyCount.ToString();
        }

        private async void RefreshCrystalCount(GameObject obj)
        {
            await Task.Delay(5);
            crystalCountText.text = CrystalFather.CrystalCount.ToString();
        }

        private async void RefreshLifesCount(GameObject obj)
        {
            await Task.Delay(5);
            playerLifesText.text = Player.Lifes.ToString();
        }

        private async void RefreshScoresCount(GameObject obj)
        {
            await Task.Delay(5);
            playerScoresText.text = Player.Scores.ToString();
        }

        private void FixedUpdate()
        {
            distanceToCrystal.text = CrystalFather.GetClosestDistanceToPlayer().ToString("#0.0");
            distanceToEnemy.text = EnemyFather.GetClosestDistanceToPlayer().ToString("#0.0");
        }
    }
}
