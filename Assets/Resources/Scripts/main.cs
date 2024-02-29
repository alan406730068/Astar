using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditorInternal.VersionControl.ListControl;

public class main : MonoBehaviour
{
    waypointTerrain WPT;              //����a��(��Ƶ��c)�MAstar(�t��k)�[�JUNITY���A�æbSTART��k������l��
    Astar Astar;
    public Camera Camera;
    public Transform player;
    private bool bAstar;              //�ݭn�@��bool�Y�h��astar�O�_�]���\
    private bool nAstar;
    private Vector3 hitPoint;
    void Start()
    {
        WPT = new waypointTerrain();
        WPT.init();
        Astar = new Astar();
        Astar.init(WPT);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Ray ray = Camera.ScreenPointToRay(Input.mousePosition);   //�ϥΨ��yRAY��k�ݭnCAMERA
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000.0f, 1 << LayerMask.NameToLayer("terrain")))
            {
                if ((hit.point - player.position).magnitude < 10.0f)
                {
                    Debug.Log("ABC");
                    hitPoint = hit.point;
                    bAstar = false;
                    nAstar = true;
                }
                else 
                {
                    bAstar = Astar.instance.performAstar(player.position, hit.point);
                    nAstar = false;
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (bAstar)
        { 
            List<Vector3> aStarList = Astar.instance.GetPath();
            Gizmos.color = Color.blue;
            for (int i = 0; i < aStarList.Count-1; i++)   //�]��list�|�����I�h�e�u�A�ҥH�n-1�A�קK�L�j
            {
                Vector3 listStart = aStarList[i];
                listStart.y += 1.0f;
                if (i == aStarList.Count)
                    break;
                Vector3 listEnd = aStarList[i + 1];
                listEnd.y += 1.0f;
                Gizmos.DrawLine(listStart, listEnd);
            }
        }
        if (nAstar) 
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(player.position, hitPoint);
        }
    }
}
