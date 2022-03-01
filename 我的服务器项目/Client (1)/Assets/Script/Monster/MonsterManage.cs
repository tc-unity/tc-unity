using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;//对象池插件 命名空间

public class MonsterManage : MonoBehaviour {

    SpawnPool spawnPool;//对象池 工厂
    PrefabPool prefabPool;//生产线
    PrefabPool prefabPool2;//生产线
    public GameObject[] prefab;
    public PlayerMove playerCount;
    public HPScript mon_Pro;
    // Use this for initialization
    void Start () {
        //playerCount = GameObject.Find("Player").GetComponent<PlayerMove>();
        spawnPool =GameObject.Find("Pool_Particles").GetComponent<SpawnPool>();
       // MakerMon();
        EntrustCtrl.Instance.AddVessel(EventType_Game.ET_MonProduce, MakerMon);   
        //EntrustCtrl.Instance.AddVessel(EventType_Game.ET_MonDie, DelMon);
    }
    // Update is called once per frame
    void Update () {
		
	}
    public void MakerMon()
    {
        //prefab[0]= Resources.Load<GameObject>("Prefabs/monster/B0201@1");
        //prefab[1] = Resources.Load<GameObject>("Prefabs/monster/Titan");

        if (prefab!=null)
        {
            int mon_ID = Random.Range(0, prefab.Length);
            var obj = spawnPool.Spawn(prefab[mon_ID], this.transform);//对象池 创建  与Instantiate使用相同 有多个重载可以选择
            obj.position = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            mon_Pro = obj.GetComponent<HPScript>();
            if (mon_ID == 0)
            {
                mon_Pro.mon_Pro = MonsterFC.Instance.CreatItem(1001);
            }
            else if (mon_ID == 1)
            {
                mon_Pro.mon_Pro = MonsterFC.Instance.CreatItem(1002);
            }
        }
      
    }
    public void DelMon(Transform obj)
    {
        
        spawnPool.Despawn(obj,2);//对象池 删除  与Destroy用法相同
       
    }
}
