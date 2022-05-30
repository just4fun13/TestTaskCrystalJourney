using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class CrystalFather : MonoBehaviour
    {
        public delegate void CrystalSpawnEvent(GameObject obj);
        public static event CrystalSpawnEvent CrystalSpawn;
        [SerializeField] private float spawnIntervalInSeconds = 2f;
        [SerializeField] private int maxCrystalCount = 5;
        [SerializeField] private GameObject crystalPrefab;
        private float mapLen = 10f;
        private float crystalWidth = 0.8f;
        private float Len => mapLen - crystalWidth;
        private Queue<GameObject> crystalsPool = new Queue<GameObject>();
        private static List<Transform> activeCrystals = new List<Transform>();
        public static int CrystalCount = 0;
        private static Transform playerTransform;
        private void Start()
        {
            activeCrystals.Clear();
            GameObject[] crystalsOnTheScene = GameObject.FindGameObjectsWithTag("Crystal");
            foreach (GameObject cr in crystalsOnTheScene)
                activeCrystals.Add(cr.transform);
            CrystalCount = activeCrystals.Count;
            for (int i = CrystalCount; i < maxCrystalCount; i++)
            {
                GameObject newCrystal = Instantiate(crystalPrefab, transform);
                crystalsPool.Enqueue(newCrystal);
                newCrystal.SetActive(false);
            }
            StartCoroutine(SpawnContinuous());
            Player.PlayerGotCrystal += CrystalDestroyed;
            Enemy.EnemyHitCrystal += CrystalDestroyed;
            FindPlayer();
        }

        private static void FindPlayer() =>
            playerTransform = GameObject.FindObjectOfType<Player>().transform;


        private void OnDestroy()
        {
            Player.PlayerGotCrystal -= CrystalDestroyed;
            Enemy.EnemyHitCrystal -= CrystalDestroyed;
        }

        private void SpawnCrystal()
        {
            int tryiesCount = 100;
            Vector3 pos = RandomPointOnMap;
            while (!PointIsEmpty(pos) && tryiesCount > 0)
            {
                tryiesCount--;
                pos = RandomPointOnMap;
            }
            if (tryiesCount == 0)
            {
                Debug.LogError("Can not find space for new Crystal!");
                return;
            }
            GameObject newCrystal = crystalsPool.Dequeue();
            newCrystal.transform.position = RandomPointOnMap;
            CrystalCount++;
            if (CrystalSpawn != null)
                CrystalSpawn(newCrystal);
            newCrystal.SetActive(true);
            activeCrystals.Add(newCrystal.transform);
        }

        private IEnumerator SpawnContinuous()
        {
            while (true)
            {
                yield return new WaitForSeconds(spawnIntervalInSeconds);
                if (crystalsPool.Count > 0)
                    SpawnCrystal();
            }
        }

        private void CrystalDestroyed(GameObject crystalObj)
        {
            crystalsPool.Enqueue(crystalObj);
            activeCrystals.Remove(crystalObj.transform);
            CrystalCount--;
        }

        private bool PointIsEmpty(Vector3 pos)
        {
            Physics.SyncTransforms();
            Collider[] cols = Physics.OverlapSphere(pos, crystalWidth);
            if (cols.Length == 0)
                return true;
            else
                return false;
        }

        private Vector3 RandomPointOnMap =>
             new Vector3(Random.Range(-Len, Len), 0f, Random.Range(-Len, Len));

        public static float GetClosestDistanceToPlayer()
        {
            if (playerTransform == null)
            {
                FindPlayer();
                return -1;
            }
            float res = 1000f;
            foreach (Transform t in activeCrystals)
            {
                float distSqr = (playerTransform.position - t.position).sqrMagnitude;
                if (distSqr < res)
                    res = distSqr;
            }
            return Mathf.Sqrt(res);
        }

    }
}
