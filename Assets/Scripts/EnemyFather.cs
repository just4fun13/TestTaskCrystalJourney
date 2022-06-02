using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class EnemyFather : ObjectPoolHandler
    {
        public delegate void CountChangedEvent();
        public static event CountChangedEvent CountChanged;
        [SerializeField] private float spawnRadius;
        [SerializeField] private Transform spawnPoint;
        private void Awake()
        {
            base.Awake();
            Player.BackToPool += ObjectDestroy;
        }

        private void OnDestroy()
        {
            Player.BackToPool -= ObjectDestroy;
        }
        protected override Vector3 GetPositionForSpawn => MapClass.GetPointOnTheMapInsideCircle(spawnPoint.position, spawnRadius);

        protected override void SpawnObject()
        {
            base.SpawnObject();
            if (CountChanged != null) CountChanged();
        }

        protected override void ObjectDestroy(GameObject objToDestroyOnTheScene)
        {
            base.ObjectDestroy(objToDestroyOnTheScene);
            if (CountChanged != null) CountChanged();
        }

    }
}
