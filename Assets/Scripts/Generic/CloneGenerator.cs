using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CloneGenerator : MonoBehaviour
{
    public int rowCount = 1;
    public Vector3 direction;
    public Vector3 direction2;
    public Vector3 randomOffset;

    private List<Vector3> GetPoints()
    {
        List<Vector3> points = new List<Vector3>();
        for (int i = 0; i < rowCount; i++)
        {
            if (direction2.sqrMagnitude > 0)
            {
                for (int j = 0; j < rowCount; j++)
                {
                    Vector3 newPoint = new Vector3(direction.x * i + direction2.x * j, direction.y * i + direction2.y * j, direction.z * i + direction2.z * j);
                    points.Add(newPoint + GetRandomOffset());
                }
            }
            else
            {
                Vector3 newPoint =new Vector3(direction.x * i, direction.y * i, direction.z * i);
                points.Add(newPoint + GetRandomOffset());
            }
        }
        points.RemoveAt(0);
        return points;
    }

    private Vector3 GetRandomOffset()
    {
        return new Vector3(Random.Range(-randomOffset.x, randomOffset.x),
            Random.Range(-randomOffset.y, randomOffset.y),
            Random.Range(-randomOffset.z, randomOffset.z));
    }

    void Start()
    {
        foreach (Vector3 point in GetPoints())
        {
            GameObject go = Instantiate(gameObject, transform.position + point, transform.rotation) as GameObject;
            go.transform.parent = gameObject.transform.parent;
            Destroy(go.GetComponent<CloneGenerator>());
            go.GetComponent<ChargedObject>().UpdateAppearance();
        }
        Destroy(this);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (GetComponent<ChargedObject>() != null && GetComponent<ChargedObject>().charge > 0)
            Gizmos.color = Color.green;

        foreach (Vector3 point in GetPoints())
            Gizmos.DrawSphere(gameObject.transform.position + point, 0.2f);

    }
}
