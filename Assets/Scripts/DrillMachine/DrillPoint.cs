using UnityEngine;

public class DrillPoint : MonoBehaviour
{
    public Color gizmoColor = Color.red;

    public float gizmoSize = 0.1f;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(transform.position, gizmoSize);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(transform.position, new Vector3(gizmoSize, gizmoSize, gizmoSize));
    }
}
