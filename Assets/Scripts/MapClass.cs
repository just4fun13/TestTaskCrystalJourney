using UnityEngine;

namespace Assets.Scripts
{
    public class MapClass 
    {
        private static float maxX = 10f;
        private static float maxY = 10f;
        private static float minX = -10f;
        private static float minY = -10f;
        private static float objectsWidth = 1f;
        private static float groundYvalue = 0f;
        public static Vector3 GetRandomPointOnTheMap =>
            new Vector3(Random.Range(minX, maxX), groundYvalue, Random.Range(minY, maxY));

        public static float MaxDistanceSqr => new Vector2(maxX - minX, maxY - minY).sqrMagnitude;

        public static Vector3 GetPointOnTheMapInsideCircle(Vector3 circleCenter, float circleRadius)
        {
            Vector2 delta = Random.insideUnitCircle * circleRadius;
            return new Vector3(
                Mathf.Clamp(delta.x + circleCenter.x, minX, maxX),
                groundYvalue,
                Mathf.Clamp(delta.y + circleCenter.z, minY, maxY) );
        }
        public bool PointIsEmpty(Vector3 pos)
        {
            Physics.SyncTransforms();
            Collider[] cols = Physics.OverlapSphere(pos, objectsWidth);
            if (cols.Length == 0)
                return true;
            else
                return false;
        }
    }
}
