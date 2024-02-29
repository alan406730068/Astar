using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class waypointTerrain    //創立三份資料 : pathnodeList，wallArray，GOarray
{
    public List<pathNode> WPList;    
    public GameObject[] WallArray;
    public GameObject[] WPs;
    private TextAsset wpText;
    public void init() 
    {
        WPList = new List<pathNode>();  //每隔節點資料
        WallArray = GameObject.FindGameObjectsWithTag("wall"); //把每個牆存成一個陣列
        WPs = GameObject.FindGameObjectsWithTag("WP");   //把每個WP存成一個陣列
        foreach (GameObject G in WPs)       //先初始化list中每個節點，並設定好list的長度  (設定節點的GAMEoBJECT、名字、位置)
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
        loadWP();    //把UNITY中的WP寫入到LIST中給A*座使用
    }
    void loadWP() 
    {
        StreamReader sr = new StreamReader("Assets/Resources/abc.txt", false);   //確認裡面是有資料的
        if (sr == null)
            return;
        //string allText = sr.ReadToEnd();
        sr.Close();
        wpText = Resources.Load("abc") as TextAsset;    //透過textasset去讀取wp資料並寫入list中
        string allText = wpText.text;
        string[] lineText = allText.Split('\n');     //分解成n行文字
        int index = 0;
        while(index < lineText.Length-1)               //開始灌入資料每個=陣列灌入  ， 因有一行是空白的要剪掉
        {
            string line = lineText[index];            //先選取分解文字
            index++;
            string[] text = line.Split(',');          //在利用,去分解
            pathNode currentNode = null;
            for (int i = 0; i < WPList.Count; i++) 
            {
                if (WPList[i].wpGo.name == text[0]) 
                {
                    currentNode = WPList[i];   //先透過名字找到節點，並繼續填入資料
                    break;
                }
            }
            if (currentNode == null)       //防呆
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
    public void clearNodeInfo() //做完a*後清除pathnode 地的a*資訊
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
    public pathNode GetWayPoint(Vector3 pos, int floor)  //用擂台方式找出離起點和終點的最佳node
    {
        pathNode Cnode = null;
        pathNode Rnode = null;
        float maxD = 1000000.0F;
        for (int i = 0; i < WPList.Count; i++)
        {
            Cnode = WPList[i];
            if(Cnode.floor != floor)    //檢查是否為同一樓層
                continue;
            if(Physics.Linecast(pos, Cnode.wp_pos,1 << LayerMask.NameToLayer("wall")))  //檢查此點和點之間有牆壁
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
