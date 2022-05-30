using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerControl : MonoBehaviour
    {
        [SerializeField] private Player player;
        private Camera camera;
        private RaycastHit hit;
        private void Start() =>  camera = Camera.main;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                    player.MoveTo(new Vector2(hit.point.x, hit.point.z));
            }
        }
    }
}
