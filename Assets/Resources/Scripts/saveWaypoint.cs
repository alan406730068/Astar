using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class saveWaypoint : MonoBehaviour
{
    void Start()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("WP");
        StreamWriter sw = new StreamWriter("Assets/Resources/abc.txt",false);
        string s = "";
        for (int i = 0; i < gos.Length; i++) 
        {
            s = "";
            s += gos[i].name;                      //第一格顯示物體名字
            s += ",";
            s += gos[i].transform.position.x;      //第二格顯示物體x座標
            s += ",";
            s += gos[i].transform.position.y;      //第三格顯示物體y座標
            s += ",";
            s += gos[i].transform.position.z;      //第四格顯示物體z座標
            s += ",";
            WP wP = gos[i].GetComponent<WP>();
            s += wP.floor;                         //第五格顯示在第幾層
            s += ",";
            s += wP.bLink;                         //第六格顯示是否連接
            s += ",";
            s += wP.Neighbor.Count;                //第七格顯示鄰居數量
            s += ",";
            for (int j = 0; j < wP.Neighbor.Count; j++)  //第八格以後顯示鄰居物體
            {
                s += wP.Neighbor[j].name;
                s += ",";
            }
            sw.WriteLine(s);
        }
        sw.Close();
    }
}
