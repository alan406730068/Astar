using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum pathNodeState 
{
    None = -1,
    OPEN = 0,
    CLOSE = 1,
}
public class pathNode                       //A*要計算的資料
{
    public GameObject wpGo;
    public List<pathNode> neighbors;
    public int floor;
    public int blink;
    public Vector3 wp_pos;

    //a*才會用到的
    public pathNode parent;
    public float CFS;
    public float CTE;
    public float TTC;
    public pathNodeState state;
}
