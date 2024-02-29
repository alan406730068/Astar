using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WP : MonoBehaviour
{
    public List<GameObject> Neighbor = new List<GameObject>();
    public int floor;
    public int bLink;
    private void OnDrawGizmos()
    {
        if (Neighbor.Count != 0 || Neighbor != null) 
        {
            foreach (GameObject go in Neighbor) 
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(this.transform.position, go.transform.position);
            }
        }
    }
}
