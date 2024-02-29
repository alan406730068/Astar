using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar 
{
    static public Astar instance;

    List<pathNode> Openlist;
    List<pathNode> Closedlist;
    List<Vector3> pathList;
    waypointTerrain WPT;
    public void init(waypointTerrain wpt) 
    {
        WPT = wpt;
        Openlist = new List<pathNode>();
        Closedlist = new List<pathNode>();
        pathList = new List<Vector3>();
        instance = this;
    }
    public List<Vector3> GetPath() 
    {
        return pathList;
    }
    public pathNode GetBestNode() //透過擂台方式找openlist中的ttc最小點
    {
        pathNode cNode = null;
        float ttcDis;
        float maxdis = 100000.0f;
        foreach (pathNode p in Openlist) //在openlist找節點
        {
            ttcDis = p.TTC;
            if (ttcDis < maxdis) 
            {
                maxdis = ttcDis;
                cNode = p;
            }
        }
        Openlist.Remove(cNode);
        return cNode;
    }
    public void BuildPath(Vector3 sPos,Vector3 ePos,pathNode sNode,pathNode eNode) 
    {
        pathList.Clear();
        pathList.Add(sPos);             //加入起點
        if (sNode == eNode) 
        {
            pathList.Add(eNode.wp_pos);
        }
        else
        {                              //加入從endnode加到startnode    (sPos,sNode,...........,eNode,ePos)
            pathNode current = eNode;
            while (current != null)
            {
                pathList.Insert(1, current.wp_pos); 
                current = current.parent;
            }
        }
        pathList.Add(ePos);            //加入終點
    }
    public bool performAstar(Vector3 sPos, Vector3 ePos)
    {
        int startFloor = 0;
        int endFloor = 0;
        if (sPos.y > 4.0f)
            startFloor = 1;
        if (ePos.y > 4.0f)
            endFloor = 1;
        pathNode startNode = WPT.GetWayPoint(sPos, startFloor);
        pathNode endNode = WPT.GetWayPoint(ePos, endFloor);
        if (startNode == null || endNode == null)   // 先判斷兩個點的資訊
        {
            Debug.Log("there's no path");
            return false;
        }    
        else if (startNode == endNode)
        {
            Debug.Log("Build Path");
            BuildPath(sPos, ePos, startNode,endNode);
            return true;
        }
        Openlist.Clear();
        Closedlist.Clear();
        WPT.clearNodeInfo();
        startNode.CFS = 0;
        startNode.CTE = (endNode.wp_pos - startNode.wp_pos).magnitude;
        startNode.TTC = startNode.CFS + startNode.CTE;
        startNode.parent = null;
        Openlist.Add(startNode);
        pathNode currentNode;
        int nodeCount = Openlist.Count;
        while (nodeCount > 0)    //要運轉道buildPath(current = end) return true，不然都是return false
        {
            currentNode = GetBestNode();
            if (currentNode == null) 
            {
                Debug.Log("Get BestNode error");
                return false;
            }
            if (currentNode == endNode) 
            {
                Debug.Log("Build Path");
                BuildPath(sPos, ePos, startNode, endNode);
                return true;
            }
            foreach (pathNode node in currentNode.neighbors) 
            {
                if (node.state == pathNodeState.CLOSE)            //第一種情況，若此node已關閉，則忽略它
                    continue;
                if (node.state == pathNodeState.OPEN)             //第二種情況，若此node有被開過，則計算它的他走過的路，落有比較短則進行更新
                {
                    float newCFS = currentNode.CFS + (node.wp_pos - currentNode.wp_pos).magnitude;
                    if (newCFS < node.CFS) 
                    {
                        node.CFS = newCFS;
                        node.TTC = node.CFS + node.CTE;
                        node.parent = currentNode;
                    }
                    continue;
                }
                node.parent = currentNode;
                node.CFS = currentNode.CFS + (node.wp_pos - currentNode.wp_pos).magnitude;  //把它爸爸的cfs和離他爸爸的距離加起來
                node.CTE = (endNode.wp_pos - node.wp_pos).magnitude;
                node.TTC = node.CFS + node.CTE;
                node.state = pathNodeState.OPEN;
                Openlist.Add(node);
                nodeCount++;
            }
            currentNode.state = pathNodeState.CLOSE;
            nodeCount--;
        }
        return false;
    }
}
