using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
    public class Enemy : Unit
    {
        public delegate void BackToPoolEvent(GameObject objToReturn);
        public static event BackToPoolEvent BackToPool;

        private float interval = 0.5f;
        private float mapSize = 10f;
        private float unitWidth = 1f;
        private float Len => mapSize - unitWidth;


        public void Init()
        {
            ResetPath();
            StartCoroutine(MoveRandomly());
        }
        private void OnDisable() =>
            StopCoroutine(MoveRandomly());

        private IEnumerator MoveRandomly()
        {
            while (true)
            {
                if (IsArived)
                {
                    MoveTo(RandomPointOnMap);
                }
                yield return new WaitForSeconds(interval);
            }
        }

        private Vector2 RandomPointOnMap =>
              new Vector2(Random.Range(-Len, Len), Random.Range(-Len, Len));

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Crystal"))
            {
                if (BackToPool != null)
                    BackToPool(collision.gameObject);
                collision.gameObject.SetActive(false);
            }
            MoveTo(RandomPointOnMap);
        }
        private void OnCollisionExit(Collision collision)
        {
            MoveTo(RandomPointOnMap);
        }
    }
}
