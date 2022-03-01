using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System;

public enum MonsterAIState
{
    MS_Idle = 1,
    MS_Patroll, //巡逻
    MS_Atk1,
   // MS_Death,
    MS_Chase,//追击
    MS_Return,//返回
    MS_Did,//死亡
    MS_DidToo

}
[Serializable]
public class BoosMon :MonoBehaviour
{
    public MonsterCtrl mon;
    public AudioClip shootSound;
    //AudioSource audio;
    //public Text broadcast;

   // public Transform monBra;
    public Camera cam;
    public TextMesh monsterName;
   // public TextMesh Hurt;
    public Transform MonUI;
    public int monId;


    MonsterAIState monsterState = MonsterAIState.MS_Idle;
    Animator animator;
    NavMeshAgent meshAgent;
    public Transform target;//玩家
    //巡逻区域
    public Transform patrollPos;//中心点
    public float patrollRadius = 6;//巡逻半径
    public float patrollMoveSpeed = 3;//移动速度
    Vector3 curPatrollPos;//当前巡逻点
    //控制巡逻频率
    public float patrollDelta = 3;//巡逻频率
    float nexPartollTime;
    //警戒范围
    public float warningRadius = 3;//警戒半径
    float warningAngle = 120;
    bool isdie=true;//是死亡
    float dieTime=5;

    public Transform Item;//掉落的物品
	// 用于初始化 只执行一次
	void Start () 
	{
        //print(this.GetType().GetHashCode());
        
        animator = GetComponent<Animator>();
        meshAgent = GetComponent<NavMeshAgent>();
       // MonsterCtrl.LoadItemLib(mon, GameFind.roleXmlPath, "Monster.xml", monId);
       // Hurt.gameObject.SetActive(false);
        //audio = GetComponent<AudioSource>();
	}
   
	
	// 每帧都调用
	void Update () 
	{

         monsterName.transform.forward = cam.transform.forward;
         //Hurt.transform.forward = cam.transform.forward;



		//不在返回中
        var distance = Vector3.Distance(transform.position, target.position);
        if (distance < warningRadius && monsterState != MonsterAIState.MS_Return)
        {
            //进入扇形区域
            var angle = Vector3.Angle(transform.forward, target.position - transform.position);

            if (angle < warningAngle / 2)
            {
                monsterState = MonsterAIState.MS_Chase;
                meshAgent.isStopped = false;
                meshAgent.destination = target.position;

            }
        }
        //if (mon.m_Hp <= 0)
        //{
        //    mon.m_Hp = 0;
        //    monsterState = MonsterAIState.MS_Did;
        //    meshAgent.isStopped = true;//关闭导航

        //}




        #region 切换状态
        switch (monsterState)
        {
            case MonsterAIState.MS_Idle:
               
                OnIdle();
                break;
            case MonsterAIState.MS_Patroll:
              
                OnPatroll();
                break;
            case MonsterAIState.MS_Atk1:
               
                OnAtk();
                break;
            case MonsterAIState.MS_Chase:
               
                OnChase();//追击
                break;
            case MonsterAIState.MS_Return:
                OnReturn();
                break;
            case MonsterAIState.MS_Did:

                OnDid();
                break;
            default:
                break;
        }
        //print(monsterState.GetType().Name + (int)monsterState);

        animator.SetInteger("State", (int)monsterState);//根据状态切换动画
        #endregion
      
        
	}

    #region 状态函数
    Vector3 GetPosRandomInArea()
    {
        var direction = UnityEngine.Random.insideUnitCircle;
        var length = UnityEngine.Random.Range(0f, 5f);
        var areaPos = patrollPos.position + new Vector3((direction * length).x, 0, (direction * length).y);
        return areaPos;
    }
    //待机
    void OnIdle()
    {
        nexPartollTime += Time.deltaTime;
        if (nexPartollTime > patrollDelta)
        {
            //切巡逻状态
            meshAgent.isStopped = false;//开启导航
            monsterState = MonsterAIState.MS_Patroll;
            curPatrollPos = GetPosRandomInArea();//区域范围随机一点
            meshAgent.destination = curPatrollPos;//设置导航目标点
            meshAgent.speed = patrollMoveSpeed;//设置移动速度
            nexPartollTime = 0;
        }
    }
    //巡逻
    void OnPatroll()
    {
       
        var distance = Vector3.Distance(transform.position, curPatrollPos);
        if (distance < 1f)
        {

            monsterState = MonsterAIState.MS_Idle;
            meshAgent.isStopped = true;
        }
    }
    //追击
    void OnChase()
    {
        //audio.clip = shootSound;
        //audio.Play();
        meshAgent.destination = target.position;
        var distance = Vector3.Distance(transform.position, patrollPos.position);
        var atkDistance = Vector3.Distance(transform.position, target.position);
        if (atkDistance < 3f)
        {
            patrollMoveSpeed = 0;
            //print(4);
            monsterState = MonsterAIState.MS_Atk1;
            meshAgent.isStopped = true;//关闭导航
            transform.LookAt(target.transform);
        }
        if (distance > patrollRadius * 2f)//大于巡逻半径1.5倍 返回
        {
           
            //print(3);
            patrollMoveSpeed *= 2;
            monsterState = MonsterAIState.MS_Return;
            meshAgent.destination = patrollPos.position;//返回巡逻中心点
        }



    }
    //返回巡逻点
    void OnReturn()
    {
        MonUI.gameObject.SetActive(false);
        var distance = Vector3.Distance(transform.position, patrollPos.position);
        if (distance < 1f)//已经返回
        {
            
            patrollMoveSpeed = 3;
            //切待机
            monsterState = MonsterAIState.MS_Idle;
            meshAgent.isStopped = true;//关闭导航
        }
    }
    //攻击
    void OnAtk()
    {
        //audio.clip = shootSound;
        //    audio.Play();
        //MonsterPanel.boss1 = this.gameObject;
        MonUI.gameObject.SetActive(true);
        var distanceA = Vector3.Distance(transform.position, target.position);
        if (distanceA < warningRadius && monsterState != MonsterAIState.MS_Return)
        {
            //进入扇形区域
            var angle = Vector3.Angle(transform.forward, target.position - transform.position);

            if (angle < warningAngle / 2)
            {
                monsterState = MonsterAIState.MS_Chase;
                meshAgent.isStopped = false;
                meshAgent.destination = target.position;

            }
        }
        if (distanceA > 3)
        {
            monsterState = MonsterAIState.MS_Chase;
            meshAgent.isStopped = false;
            meshAgent.destination = target.position;
        }
        var distance = Vector3.Distance(transform.position, patrollPos.position);
        if (distance > patrollRadius * 2f)//大于巡逻半径1.5倍 返回
        {
            print(2);
            patrollMoveSpeed *= 2;
            monsterState = MonsterAIState.MS_Return;
            meshAgent.destination = patrollPos.position;//返回巡逻中心点
        }




    }
    //死亡
    void OnDid()
    {
        
        //if (mon.name=="巨魔")
        //{
        //    //TaskCtrl.curSchedule = 1;
        //    TaskCtrl.Instance.curSchedule = 1;
        //    EntrustCtrl.Instance.TriggerEvent(EventType.ET_Task);
        //}

       // broadcast.text = "恭喜你击杀了" + mon.name;
        if (Item!=null)
        {
            Item.gameObject.SetActive(true);
            Item.parent = null;
        }
       
        if (isdie)
        {
            //Property.Instance.GexExp(mon.exp);
            isdie = false;
        }
        MonUI.gameObject.SetActive(false);
        Destroy(gameObject, dieTime);


    }
    #endregion
    //区域随机坐标


    #region 动画事件
    void Atk01()
    {
        var distance = Vector3.Distance(transform.position, target.position);
        if (distance < 3)
        {
           // Property.Instance.mHp = Property.Instance.mHp - mon.atk;
           // EntrustCtrl.Instance.TriggerEvent(EventType.ET_AttributeUpdate);
            //if (Property.Instance.mHp <=0)
            //{
            //    Property.Instance.mHp = 0;
            //}
        }

    }
    #endregion

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag=="Player")
        {
            transform.LookAt(collision.transform.position);
            monsterState = MonsterAIState.MS_Chase;
        }
    }




   
    #region 画辅助线
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(patrollPos.position, patrollRadius);//巡逻区域

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, warningRadius);//警戒区域

        //扇形区域
        var right=Vector3.Slerp(transform.right,transform.forward,1f/3f)*warningRadius;
        var left = Vector3.Slerp(-transform.right, transform.forward, 1f / 3f) * warningRadius;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + right);
        Gizmos.DrawLine(transform.position, transform.position + left);

    }
    #endregion
}