using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public abstract class ObjectPoolHandler : MonoBehaviour
    {
        [SerializeField] private GameObject objectSample;
        [SerializeField] private int maxObjectsOnSceneCount = 5;
        [SerializeField] private float spawnIntervalInSeconds = 3f;
        private Queue<GameObject> objectsPool = new Queue<GameObject>();
        private List<Transform> activeObjectsList = new List<Transform>();
        [HideInInspector]public int ObjectsOnSceneCount = 0;
        private static Transform playerTransform;
        public string MyObjectTag => objectSample.tag;
        protected virtual void Awake()
        {
            activeObjectsList.Clear();
            GameObject[] objectsOnScene = GameObject.FindGameObjectsWithTag(MyObjectTag);
            foreach (GameObject obj in objectsOnScene)
                activeObjectsList.Add(obj.transform);
            ObjectsOnSceneCount = objectsOnScene.Length;
            for (int i = ObjectsOnSceneCount; i < maxObjectsOnSceneCount; i++)
            {
                GameObject obj = Instantiate(objectSample, transform);
                obj.SetActive(false);
                objectsPool.Enqueue(obj);
            }
            StartCoroutine(SpawnContinuous());
            FindPlayer();
        }
        private static void FindPlayer()
        {
            Player player = GameObject.FindObjectOfType<Player>();
            if (player == null) return;
            playerTransform = player.transform;
        }
        protected virtual Vector3 GetPositionForSpawn => Vector3.zero;
        protected virtual void SpawnObject()
        {
            GameObject obj = objectsPool.Dequeue();
            obj.transform.position = GetPositionForSpawn;
            obj.SetActive(true);
            ObjectsOnSceneCount++;
            activeObjectsList.Add(obj.transform);
        }
        protected virtual void ObjectDestroy(GameObject objToDestroyOnTheScene)
        {        
            if (!objToDestroyOnTheScene.CompareTag(MyObjectTag)) return;
            objToDestroyOnTheScene.SetActive(false);
            ObjectsOnSceneCount--;
            objectsPool.Enqueue(objToDestroyOnTheScene);
            activeObjectsList.Remove(objToDestroyOnTheScene.transform);
        }
        private IEnumerator SpawnContinuous()
        {
            while (true)
            {
                yield return new WaitForSeconds(spawnIntervalInSeconds);
                if (objectsPool.Count > 0)
                    SpawnObject();
            }
        }
        public float GetClosestDistanceToPlayer()
        {
            if (playerTransform == null)
            {
                FindPlayer();
                return -1;
            }
            if (activeObjectsList.Count == 0)
                return -1;
            float res = MapClass.MaxDistanceSqr;
            foreach (Transform t in activeObjectsList)
            {
                float distSqr = (playerTransform.position - t.position).sqrMagnitude;
                if (distSqr < res)
                    res = distSqr;
            }
            return Mathf.Sqrt(res);
        }
    }
}
