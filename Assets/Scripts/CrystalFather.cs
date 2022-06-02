using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class CrystalFather : ObjectPoolHandler
    {
        public delegate void CountChangedEvent();
        public static event CountChangedEvent CountChanged;
        private void Awake()
        {
            base.Awake();
            Player.BackToPool += ObjectDestroy;
            Enemy.BackToPool += ObjectDestroy;
        }

        private void OnDestroy()
        {
            Player.BackToPool -= ObjectDestroy;
            Enemy.BackToPool -= ObjectDestroy;
        }

        protected override Vector3 GetPositionForSpawn => MapClass.GetRandomPointOnTheMap;

        protected override void SpawnObject()
        {
            base.SpawnObject();
            if (CountChanged != null) CountChanged();
        }

        protected override void ObjectDestroy(GameObject objToDestroyOnTheScene)
        {
            base.ObjectDestroy(objToDestroyOnTheScene);
            if (CountChanged != null)
            {
                CountChanged();
                Debug.Log("Crystal father count changed");
            }
        }
    }
}
