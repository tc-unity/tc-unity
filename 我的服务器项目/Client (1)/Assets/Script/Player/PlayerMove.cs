using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;//对象池插件 命名空间
using DG.Tweening;//补间动画命名空间

public class PlayerMove : MonoBehaviour {

    public PlayerCtrl m_Player;

    SpawnPool spawnPool;//对象池 工厂
    public RockerContro _rockerContro;//摇杆
    public Animator _anim;//状态机
    public float m_speed = 5.0f;//移动速度

    public GameObject m_Atk01;//子弹
    public float ProjectileForce;
    public float Scatter;
    public Transform m_AtkPos;//子弹产生位置
    public float bulletSpeed = 10.0f;//子弹飞行速度
    public int int_Atk = 0;

    //倒计时image
    public Transform[] CountDown;

    //范围内怪物数量
    //public List<GameObject> m_MonsterList; 
    public Transform tager;//目标怪物
    public float distance_min = 100;

    //public int mon_Count = 0;//击杀数量
    //public int mon_MaxCount = 1;//击杀数量

    //胜利
    public GameObject win_Image;
    public GameObject[] img_Star;

    //技能
    public Transform[] m_Attack;
    public Transform m_AttackPos;

    //重新刷怪
    private bool is_UpdateMon = false;

    MonsterManage mon_MansgeIns;
    private HeadEvent headUpdate;
    //private float totalTime = 0;
    //public int second = 3;
    void Start()
    {

        
        m_Player = PlayerCtrl.Instance;
           m_AttackPos = this.transform;
        for (int i = 0; i < img_Star.Length; i++)
        {
            img_Star[i].SetActive(true);
        }
        win_Image.SetActive(false);
        //m_MonsterList = new List<GameObject>();
        m_AtkPos = transform.GetChild(2);
        spawnPool = GameObject.Find("Pool_Particles").GetComponent<SpawnPool>();
        mon_MansgeIns = GameObject.Find("MonsterManage").GetComponent<MonsterManage>();
        headUpdate = GameObject.Find("Canvas").GetComponentInChildren<HeadEvent>();
        for (int j = 0; j < PlayerCtrl.Instance.mSurplus; j++)
        {
            mon_MansgeIns.MakerMon();
            Debug.Log(PlayerCtrl.Instance.mSurplus);
            //EntrustCtrl.Instance.TriggerEvent(EventType_Game.ET_MonProduce);
        }
    }

	// Update is called once per frame
	void Update () {
       
       // Physics.gravity = new Vector3(0, gravity, 0);  // gravity= -35 其他的默认
        float hor = _rockerContro.X;
        float ver = _rockerContro.Y;

        Vector3 direction = new Vector3(hor, 0, ver);

        if (direction != Vector3.zero)
        {
            //控制转向
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 10);
            //向前移动
            transform.Translate(Vector3.forward * Time.deltaTime * m_speed);
            _anim.SetBool("Run", true);
        }
        else
        {
            _anim.SetBool("Run", false);           
        }

        if (m_Player.mKill >= m_Player.mSurplus)
        {         
            is_UpdateMon = true;
            StartCoroutine(Next());
        }

    }
    public void ATK01()
    {
        
        tager= OnGetEnemy();
        if (tager)
        {
            transform.LookAt(tager);
        }
        m_speed = 0.0f;
        int_Atk = 1;
        _anim.SetInteger("Atk", int_Atk);    
        StartCoroutine(StopAtk());
        
    }
    public void ATK02()
    {
        m_speed = 0.0f;
        //m_AttackPos=this.transform;
        GameObject obj = Instantiate(m_Attack[2].gameObject) as GameObject;
        //obj.transform.position = Vector3.zero;
        obj.transform.position = new Vector3(this.transform.position.x+Random.Range(-5, 5), this.transform.position.y, this.transform.position.z+Random.Range(-5, 5));
        //Destroy(obj, 2);
        StartCoroutine(StopAtk());
    }
    public void ATK03()
    {
        m_speed = 0.0f;
        //m_AttackPos = this.transform;
        GameObject obj = Instantiate(m_Attack[1].gameObject) as GameObject;
        //obj.transform.position = Vector3.zero;
        obj.transform.position = new Vector3(this.transform.position.x+Random.Range(-5, 5), this.transform.position.y, this.transform.position.z+Random.Range(-5, 5));
        //Destroy(obj, 2);
        StartCoroutine(StopAtk());
    }
    public void ATK04()
    {
        m_speed = 0.0f;
        // m_AttackPos = this.transform;
        GameObject obj = Instantiate(m_Attack[0].gameObject) as GameObject;
        //obj.transform.position = Vector3.zero;
        obj.transform.position = new Vector3(this.transform.position.x+Random.Range(-5, 5), this.transform.position.y, this.transform.position.z+Random.Range(-5, 5));
       // Destroy(obj, 2);
        StartCoroutine(StopAtk());
    }
    public void BulletMove()
    {
        //Instantiate a new projectile
        GameObject ThisProjectile = Instantiate(m_Atk01, m_AtkPos.transform.position, m_AtkPos.transform.rotation) as GameObject;

        //add force to the projectile
        ThisProjectile.GetComponent<Rigidbody>().AddRelativeForce(Random.Range(Scatter * -1f, Scatter), Random.Range(Scatter * -1f, Scatter), ProjectileForce + Random.Range(Scatter * -1f, Scatter));

        //set the owner of the projectile...this will allow the shield to determine weather or not to let the projectile pass through
        if (gameObject.transform.parent == null)
        {
            ThisProjectile.GetComponent<ProjectileScript>().Owner = gameObject;
        }
        else
        {
            ThisProjectile.GetComponent<ProjectileScript>().Owner = gameObject.transform.parent.gameObject;
        }
    }

    IEnumerator Next()
    {
        while (is_UpdateMon)
        {
            is_UpdateMon = false;
            m_Player.mSurplus += 1;
            if (m_Player.mSurplus >= 5)
            {
                m_Player.mSurplus = 5;
            }
            m_Player.mKill = 0;
            CountDown[0].gameObject.SetActive(true);
            CountDown[0].DOScale(new Vector3(0, 0, 0), 1);
            yield return new WaitForSeconds(1);
            CountDown[0].gameObject.SetActive(false);
            CountDown[1].gameObject.SetActive(true);
            CountDown[1].DOScale(new Vector3(0, 0, 0), 1);
            yield return new WaitForSeconds(1);
            CountDown[1].gameObject.SetActive(false);
            CountDown[2].gameObject.SetActive(true);
            CountDown[2].DOScale(new Vector3(0, 0, 0), 1);
            yield return new WaitForSeconds(1);
            CountDown[2].gameObject.SetActive(false);
            CountDown[3].gameObject.SetActive(true);
            CountDown[3].DOScale(new Vector3(0, 0, 0), 1);
            yield return new WaitForSeconds(1);
            CountDown[3].gameObject.SetActive(false);
            m_Player.mLevel++;    
            //img_Star[0].SetActive(false);
            //yield return new WaitForSeconds(1);
            //img_Star[1].SetActive(false);
            //yield return new WaitForSeconds(1);
            //img_Star[2].SetActive(false);
            for (int i = 0; i < m_Player.mSurplus; i++)
            {
                mon_MansgeIns.MakerMon();
            }
            headUpdate.PanelUpdate();
        } 
    }
    IEnumerator StopAtk()
    {  
        yield return new WaitForSeconds(0.6f);
        int_Atk = 0;
        _anim.SetInteger("Atk", int_Atk);
        m_speed = 5.0f;
    }

    public Transform OnGetEnemy()
    {
        //正在搜索的半径
        int radius = 1;
        //一步一步扩大搜索半径,最大扩大到100
        while (radius < 50)
        {
            //球形射线检测,得到半径radius米范围内所有的物件
            Collider[] cols = Physics.OverlapSphere(transform.position, radius);
            //判断检测到的物件中有没有Enemy
            if (cols.Length > 0)
                for (int i = 0; i < cols.Length; i++)
                    if (cols[i].tag.Equals("Monster")|| cols[i].tag.Equals("BossMon"))
                        return cols[i].transform;
            //没有检测到Enemy,将检测半径扩大2米
            radius += 2;
        }
        return null;
    }


}
