using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterCtrl : MonoBehaviour {

    
    MonsterManage mon_Mansge;
    public Transform target;//玩家
    public float m_Distance;//距离

   // public GameObject hp_Bar;

    NavMeshAgent meshAgent;
    Animator anim;

    public enum EnumChangeHPType { Normal = 0, CustomColor = 1, CustomForce = 2, CustomColorandForce = 3, CustomText = 4 };
    public EnumChangeHPType ChangeHPType = EnumChangeHPType.Normal;

    public Color CustomColor;
    public Vector3 CustomForce;
    public float CustomForceScatter;

    private PlayerMove PlayerTager;
    private HPScript mon_Hp;

    private HeadEvent headUpdate;
    // Use this for initialization
    void Start () {
        mon_Mansge= GameObject.Find("MonsterManage").GetComponent<MonsterManage>();
        target = GameObject.Find("Player").GetComponent<Transform>();
        anim = GetComponentInChildren<Animator>();
        meshAgent = GetComponent<NavMeshAgent>();
        meshAgent.isStopped = false;
        mon_Hp = GetComponent<HPScript>();
        PlayerTager = GameObject.Find("Player").GetComponent<PlayerMove>();
        meshAgent.speed = 3;
        headUpdate= GameObject.Find("Canvas").GetComponentInChildren<HeadEvent>();


    }
	
	// Update is called once per frame
	void Update () {
      
        m_Distance = Vector3.Distance(transform.position, target.position);
        if (m_Distance>3.0f&& m_Distance < 8.0f)
        {
            meshAgent.isStopped = false;
            //transform.Rotate(transform.up,180);
            transform.LookAt(target.position);
            meshAgent.destination = target.position;
            anim.SetInteger("State", 1);
        }
        if(m_Distance <= 3.0f)
        {
            if (transform.tag== "Monster")
            {
                anim.SetInteger("State", 0);
            }
            else
            {
                anim.SetInteger("State", 2);
            }            
            meshAgent.isStopped = true;//关闭导航
            transform.LookAt(target.position);
            //transform.Rotate(transform.up, 180);
        }
        if (m_Distance > 8.0f)
        {
            meshAgent.isStopped = true;//关闭导航
            anim.SetInteger("State",0);
        }
        if (mon_Hp.mon_Pro.mHp <= 0)
        {
            if (transform.tag=="Monster")
            {
                PlayerCtrl.Instance.mScore += 10;
            }
            else if (transform.tag == "BossMon")
            {
                PlayerCtrl.Instance.mScore += 15;
            }  
            transform.GetComponent<Collider>().enabled = false;
            meshAgent.isStopped = true;//关闭导航
            if (transform.tag == "Monster")
            {
                anim.SetInteger("State", 0);
            }
            else
            {
                anim.SetInteger("State", 4);
            }
            mon_Hp.mon_Pro.mHp = 1;
            PlayerTager.tager = null;
            DelMon();
        }

    }
    public void  DelMon()
    { 
        Destroy(gameObject,2);
        PlayerCtrl.Instance.mKill++;
        headUpdate.PanelUpdate();
        //EntrustCtrl.Instance.TriggerEvent(EventType_Game.ET_AttributeUpdate);
    }
    public void OnCollisionEnter(Collision colliderInfo)
    {
        if (colliderInfo.gameObject.tag== "Projectile")
        {
            if (ChangeHPType == EnumChangeHPType.Normal)
            {
                gameObject.GetComponent<HPScript>().ChangeHP(colliderInfo.gameObject.GetComponent<ProjectileScript>().Damage, colliderInfo.contacts[0].point);
            }
            else if (ChangeHPType == EnumChangeHPType.CustomColor)
            {
                gameObject.GetComponent<HPScript>().ChangeHP(colliderInfo.gameObject.GetComponent<ProjectileScript>().Damage, colliderInfo.contacts[0].point, CustomColor);
            }
            else if (ChangeHPType == EnumChangeHPType.CustomForce)
            {
                gameObject.GetComponent<HPScript>().ChangeHP(colliderInfo.gameObject.GetComponent<ProjectileScript>().Damage, colliderInfo.contacts[0].point, CustomForce, CustomForceScatter);
            }
            else if (ChangeHPType == EnumChangeHPType.CustomColorandForce)
            {
                gameObject.GetComponent<HPScript>().ChangeHP(colliderInfo.gameObject.GetComponent<ProjectileScript>().Damage, colliderInfo.contacts[0].point, CustomForce, CustomForceScatter, CustomColor);
            }
            else if (ChangeHPType == EnumChangeHPType.CustomText)
            {
                gameObject.GetComponent<HPScript>().ChangeHP(colliderInfo.gameObject.GetComponent<ProjectileScript>().Damage, colliderInfo.contacts[0].point, "Custom Text");
            }
        }
     
    }


}
