using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameManage : MonoBehaviour {

    public GameObject[] m_Game;
    //public GameObject[] m_Map;
    //public Transform map_Pos;
    //SpawnPool spawnPool;//对象池 工厂
	// Use this for initialization
	void Start () {

        //spawnPool = GameObject.Find("Pool_Particles").GetComponent<SpawnPool>();
        //map_Pos= GameObject.Find("MapPos").GetComponent<Transform>();
        //int mon_ID = Random.Range(0, m_Map.Length);
        //var obj = spawnPool.Spawn(m_Map[mon_ID], map_Pos);//对象池 创建  与Instantiate使用相同 有多个重载可以选择
        //obj.position = Vector3.zero;
        for (int i = 0; i < m_Game.Length; i++)
        {
            m_Game[i].SetActive(false);
        }
        for (int i = 0; i < m_Game.Length; i++)
        {
            m_Game[i].SetActive(true);
        }

        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

}
