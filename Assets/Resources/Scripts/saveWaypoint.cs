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
            s += gos[i].name;                      //�Ĥ@����ܪ���W�r
            s += ",";
            s += gos[i].transform.position.x;      //�ĤG����ܪ���x�y��
            s += ",";
            s += gos[i].transform.position.y;      //�ĤT����ܪ���y�y��
            s += ",";
            s += gos[i].transform.position.z;      //�ĥ|����ܪ���z�y��
            s += ",";
            WP wP = gos[i].GetComponent<WP>();
            s += wP.floor;                         //�Ĥ�����ܦb�ĴX�h
            s += ",";
            s += wP.bLink;                         //�Ĥ�����ܬO�_�s��
            s += ",";
            s += wP.Neighbor.Count;                //�ĤC����ܾF�~�ƶq
            s += ",";
            for (int j = 0; j < wP.Neighbor.Count; j++)  //�ĤK��H����ܾF�~����
            {
                s += wP.Neighbor[j].name;
                s += ",";
            }
            sw.WriteLine(s);
        }
        sw.Close();
    }
}
