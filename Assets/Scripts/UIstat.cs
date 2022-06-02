using UnityEngine;
using TMPro;

namespace Assets.Scripts
{
    public class UIstat : MonoBehaviour
    {
        private const string bestString = "Best";
        private const string recordString = "Рекорд: ";
        private const string crystalTag = "Crystal";
        private const string enemyTag = "Enemy";

        public MapClass mapClass = new MapClass();
        [SerializeField] private TextMeshProUGUI enemiesCountText;
        [SerializeField] private TextMeshProUGUI crystalCountText;
        [SerializeField] private TextMeshProUGUI playerLifesText;
        [SerializeField] private TextMeshProUGUI playerScoresText;
        [SerializeField] private TextMeshProUGUI distanceToEnemy;
        [SerializeField] private TextMeshProUGUI distanceToCrystal;
        [SerializeField] private TextMeshProUGUI recordText;
        private ObjectPoolHandler crystalFather;
        private ObjectPoolHandler enemyFather;


        private void Start()
        {
            CrystalFather.CountChanged += RefreshStats;
            EnemyFather.CountChanged += RefreshStats;
            int bestResult = 0;
            if (PlayerPrefs.HasKey(bestString))
                bestResult = PlayerPrefs.GetInt(bestString);
            recordText.text = $"{recordString} {bestResult}";
            ObjectPoolHandler[] objectPoolHandlers = GameObject.FindObjectsOfType<ObjectPoolHandler>();
            if (objectPoolHandlers.Length != 2)
            {
                Debug.LogError($"Unexpected count of father on the scene {objectPoolHandlers.Length}");
                return;
            }
            if (objectPoolHandlers[0].MyObjectTag == crystalTag)
            {
                crystalFather = objectPoolHandlers[0];
                enemyFather = objectPoolHandlers[1];
            }
            else
            {
                crystalFather = objectPoolHandlers[1];
                enemyFather = objectPoolHandlers[0];
            }
            RefreshStats();
        }

        private void OnDestroy()
        {
            CrystalFather.CountChanged -= RefreshStats;
            EnemyFather.CountChanged -= RefreshStats;
        }

        private void RefreshStats()
        {
            enemiesCountText.text = enemyFather.ObjectsOnSceneCount.ToString();
            crystalCountText.text = crystalFather.ObjectsOnSceneCount.ToString();
            playerLifesText.text = Player.Lifes.ToString();
            playerScoresText.text = Player.Scores.ToString();
        }

        private void FixedUpdate()
        {
            distanceToCrystal.text = crystalFather.GetClosestDistanceToPlayer().ToString("#0.0");
            distanceToEnemy.text = enemyFather.GetClosestDistanceToPlayer().ToString("#0.0");
        }
    }
}
