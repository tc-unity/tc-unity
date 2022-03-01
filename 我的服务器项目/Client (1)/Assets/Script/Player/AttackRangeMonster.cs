//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class AttackRangeMonster : MonoBehaviour {

//    private PlayerMove atk_Tager;
//	// Use this for initialization
//	void Start () {
//        atk_Tager = GameObject.Find("Player").GetComponent<PlayerMove>();
//    }
	
//	// Update is called once per frame
//	void Update () {
		
//	}
//    public void OnTriggerEnter(Collider collider)
//    {
//        if (collider.tag=="Monster"|| collider.tag == "BossMon")
//        {
//            atk_Tager.m_MonsterList.Add(collider.gameObject);
//        }
//    }
//    public void OnTriggerStay(Collider collider)
//    {
       
//    }
//    public void OnTriggerExit(Collider collider)
//    {
//        if (collider.tag == "Monster" || collider.tag == "BossMon")
//        {
//            foreach (var item in atk_Tager.m_MonsterList)
//            {
//                if (item.gameObject==collider)
//                {
//                    atk_Tager.m_MonsterList.Remove(item);
//                    atk_Tager.tager = null;
//                }
//            }
            
//        }
        
//    }
//}
