using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class EnemyFather : MonoBehaviour
    {
        public delegate void EnemySpawnEvent(GameObject obj);
        public static event EnemySpawnEvent EnemyCountChange;
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private int maxEnemiesCount = 5;
        [SerializeField] private float spawnRadius = 3f;
        [SerializeField] private float spawnIntervalInSeconds = 3f;
        private Queue<GameObject> enemyPool = new Queue<GameObject>();
        private static List<Transform> activeEnemies = new List<Transform>();
        public static int EnemyCount = 0;
        private static Transform playerTransform;
        private void Start()
        {
            activeEnemies.Clear();
            Enemy[] enemiesOnTheScene = GameObject.FindObjectsOfType<Enemy>();
            foreach (Enemy en in enemiesOnTheScene)
                activeEnemies.Add(en.transform);

            for (int i = 0; i < maxEnemiesCount; i++)
            {
                GameObject enemy = Instantiate(enemyPrefab, transform);
                enemy.SetActive(false);
                enemyPool.Enqueue(enemy);
            }
            StartCoroutine(SpawnContinuous());
            EnemyCount = GameObject.FindObjectsOfType<Enemy>().Length;
            Player.PlayerTouchEnemy += EnemyDie;
            FindPlayer();
        }

        private static void FindPlayer() =>
            playerTransform = GameObject.FindObjectOfType<Player>().transform;

        private void OnDestroy()
        {
            Player.PlayerTouchEnemy -= EnemyDie;
        }

        private void EnemySpawn()
        {
            GameObject enemy = enemyPool.Dequeue();
            Vector2 p = Random.Range(0, spawnRadius) * Random.insideUnitCircle;
            Vector3 pos = new Vector3(ClampToMap(p.x), 0f, ClampToMap(p.y)) + transform.position;
            enemy.transform.position = pos;
            enemy.SetActive(true);
            enemy.GetComponent<Enemy>().Init();
            EnemyCount++;
            if (EnemyCountChange != null)
                EnemyCountChange(enemy);
            activeEnemies.Add(enemy.transform);
        }

        private void EnemyDie(GameObject enemy)
        {
            enemyPool.Enqueue(enemy);
            EnemyCount--;
            if (EnemyCountChange != null)
                EnemyCountChange(enemy);
            activeEnemies.Remove(enemy.transform);
        }

        private IEnumerator SpawnContinuous()
        {
            while (true)
            {
                yield return new WaitForSeconds(spawnIntervalInSeconds);
                if (enemyPool.Count > 0)
                    EnemySpawn();
            }
        }

        public static float GetClosestDistanceToPlayer()
        {
            if (playerTransform == null)
            {
                FindPlayer();
                return -1;
            }
            if (activeEnemies.Count == 0)
                return -1;
            float res = 1000f;
            foreach (Transform t in activeEnemies)
            {
                float distSqr = (playerTransform.position - t.position).sqrMagnitude;
                if (distSqr < res)
                    res = distSqr;
            }
            return Mathf.Sqrt(res);
        }

        private float ClampToMap(float v) => Mathf.Clamp(v, -10f, 10f);
    }
}
