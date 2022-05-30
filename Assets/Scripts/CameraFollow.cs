using UnityEngine;

namespace Assets.Scripts
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        private Vector3 delta;

        private void Start() => delta = transform.position - target.position;

        private void Update() => transform.position = target.position + delta;

    }
}
