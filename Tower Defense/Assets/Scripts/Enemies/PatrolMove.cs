using System.Collections.Generic;
using UnityEngine;

public class PatrolMove : MonoBehaviour
{
    [SerializeField] private List<Transform> patrolPoints = new List<Transform>();
    public Color ColorSelect;
    public List<Transform> PatrolPoints() => patrolPoints;

    private void OnDrawGizmos()
    {
        DrawPoints();
        DrawPaths();
    }
    private void DrawPoints()
    {
        Gizmos.color = Color.yellow;
        foreach (Transform point in patrolPoints)
        {
            if (point != null)
                Gizmos.DrawSphere(point.transform.position, .25f);
        }
    }

    private void DrawPaths()
    {
        Gizmos.color = ColorSelect;
        for (int i = 0; i < patrolPoints.Count - 1; i++)
        {
            if (patrolPoints[i] != null && patrolPoints[i + 1] != null)
            {
                Vector3 thisPoint = patrolPoints[i].transform.position;
                Vector3 nextPoint = patrolPoints[i + 1].transform.position;
                Gizmos.DrawLine(thisPoint, nextPoint);
            }

        }
    }
}
