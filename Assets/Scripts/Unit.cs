using UnityEngine;

namespace Assets.Scripts
{

    public class Unit : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        private Vector2 destination;
        private float stepAccuracy = 0.5f;

        private UnitStats myStat = new UnitStats();

        public virtual UnitStats MyStat
        {
            get { return myStat; }
            set { myStat = value; }
        }

        public bool IsArived => (destination - myPosV2).sqrMagnitude < stepAccuracy;
        private Vector2 myPosV2 => new Vector2(transform.position.x, transform.position.z);

        private void Start() => destination = new Vector2(transform.position.x, transform.position.z);

        public void ResetPath()
        {
            rb.velocity = Vector3.zero;
            destination = new Vector2(transform.position.x, transform.position.z);
        }

        public void MoveTo(Vector2 dest)
        {
            destination = dest;
            RotateTo(dest);
            rb.velocity = transform.forward * myStat.speed;
        }

        private void RotateTo(Vector2 pos)
        {
            Vector2 delta = pos - myPosV2;
            transform.rotation = Quaternion.Euler(0f, Mathf.Atan2(delta.x, delta.y) * Mathf.Rad2Deg, 0f);
        }

        protected void Update()
        {
            if (IsArived)
                rb.velocity = Vector3.zero;
        }
    }
}
