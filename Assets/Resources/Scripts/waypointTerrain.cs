using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class waypointTerrain    //�ХߤT����� : pathnodeList�AwallArray�AGOarray
{
    public List<pathNode> WPList;    
    public GameObject[] WallArray;
    public GameObject[] WPs;
    private TextAsset wpText;
    public void init() 
    {
        WPList = new List<pathNode>();  //�C�j�`�I���
        WallArray = GameObject.FindGameObjectsWithTag("wall"); //��C����s���@�Ӱ}�C
        WPs = GameObject.FindGameObjectsWithTag("WP");   //��C��WP�s���@�Ӱ}�C
        foreach (GameObject G in WPs)       //����l��list���C�Ӹ`�I�A�ó]�w�nlist������  (�]�w�`�I��GAMEoBJECT�B�W�r�B��m)
        {
            pathNode pathNode = new pathNode();
            pathNode.wpGo = G;
            pathNode.wpGo.name = G.name;
            pathNode.wp_pos = G.transform.position;
            pathNode.neighbors = new List<pathNode>();
            pathNode.floor = 0;
            pathNode.blink = 0;
            pathNode.parent = null;
            pathNode.CFS = 0;
            pathNode.CTE = 0;  
            pathNode.TTC = 0;
            pathNode.state = pathNodeState.None;
            WPList.Add(pathNode);
        }
        loadWP();    //��UNITY����WP�g�J��LIST����A*�y�ϥ�
    }
    void loadWP() 
    {
        StreamReader sr = new StreamReader("Assets/Resources/abc.txt", false);   //�T�{�̭��O����ƪ�
        if (sr == null)
            return;
        //string allText = sr.ReadToEnd();
        sr.Close();
        wpText = Resources.Load("abc") as TextAsset;    //�z�Ltextasset�hŪ��wp��ƨüg�Jlist��
        string allText = wpText.text;
        string[] lineText = allText.Split('\n');     //���Ѧ�n���r
        int index = 0;
        while(index < lineText.Length-1)               //�}�l��J��ƨC��=�}�C��J  �A �]���@��O�ťժ��n�ű�
        {
            string line = lineText[index];            //��������Ѥ�r
            index++;
            string[] text = line.Split(',');          //�b�Q��,�h����
            pathNode currentNode = null;
            for (int i = 0; i < WPList.Count; i++) 
            {
                if (WPList[i].wpGo.name == text[0]) 
                {
                    currentNode = WPList[i];   //���z�L�W�r���`�I�A���~���J���
                    break;
                }
            }
            if (currentNode == null)       //���b
                continue;
            currentNode.floor = int.Parse(text[4]);
            currentNode.blink = int.Parse(text[5]);
            int neigberCount = int.Parse(text[6]);
            for (int i = 0;i < neigberCount; i++)
            {
                string neighborString = text[ 7 + i];
                if (neighborString == null)
                    break;
                for (int j = 0; j < WPList.Count; j++) 
                {
                    if (WPList[j].wpGo.name == neighborString) 
                    {
                        currentNode.neighbors.Add(WPList[j]);
                        break;
                    }
                }
            }
        }
    }
    public void clearNodeInfo() //����a*��M��pathnode �a��a*��T
    {
        foreach (pathNode currentNode in WPList) 
        {
            currentNode.parent = null;
            currentNode.CFS = 0;
            currentNode.CTE = 0;
            currentNode.TTC = 0;
            currentNode.state = pathNodeState.None;
        }
    }
    public pathNode GetWayPoint(Vector3 pos, int floor)  //�ξݥx�覡��X���_�I�M���I���̨�node
    {
        pathNode Cnode = null;
        pathNode Rnode = null;
        float maxD = 1000000.0F;
        for (int i = 0; i < WPList.Count; i++)
        {
            Cnode = WPList[i];
            if(Cnode.floor != floor)    //�ˬd�O�_���P�@�Ӽh
                continue;
            if(Physics.Linecast(pos, Cnode.wp_pos,1 << LayerMask.NameToLayer("wall")))  //�ˬd���I�M�I���������
                continue;
            Vector3 dis =  Cnode.wp_pos - pos;
            dis.y = 0;
            if (dis.magnitude < maxD) 
            {
                maxD = dis.magnitude;
                Rnode = Cnode;
            }
        }
        return Rnode;
    }

}
