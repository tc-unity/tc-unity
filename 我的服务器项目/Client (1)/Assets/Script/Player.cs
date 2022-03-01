using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour {

    public GameObject m_Camera;
    public RockerContro _rockerContro;//摇杆
    public Animator _anim;//状态机
    public float m_speed = 5.0f;//移动速度

    private int id;
    private Client clientScript = null;
    private float time = 0.0f;//计时器
    public float asynRate = 0.1f;//同步间隔
    private bool isMoving = false;

    public GameObject m_Atk01;//子弹
    public float Scatter;
    public float ProjectileForce;
    public Transform m_AtkPos;//子弹产生位置
    public float bulletSpeed = 10.0f;//子弹飞行速度
    public int int_Atk = 0;

    public Transform tager;//目标怪物
    public float distance_min = 20;

   
    public void SetId(int id)
    {
        this.id = id;
    }
    // Use this for initialization
    void Start () {
        m_Camera = GameObject.Find("Camera");
        _rockerContro = GameObject.Find("Canvas").transform.GetChild(0).GetChild(0).GetComponent<RockerContro>();
        _anim = GetComponent<Animator>();
        clientScript = GameObject.Find("Client").GetComponent<Client>();
        m_Camera.AddComponent<CameraMove>();
        Physics.gravity = new Vector3(0, -500, 0);  // gravity= -35 其他的默认

      
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (clientScript.GetSocket() != null && clientScript.GetSocket().Connected && id == clientScript.GetId())
        {
            float hor = _rockerContro.X;
            float ver = _rockerContro.Y;

            Vector3 direction = new Vector3(hor, 0, ver);

            if (direction != Vector3.zero)
            {
                isMoving = true;
                //控制转向
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 10);
                //向前移动
                transform.Translate(Vector3.forward * Time.deltaTime * m_speed);
                _anim.SetBool("Run", true);

            }
            else
            {
                isMoving = false;
                _anim.SetBool("Run", false);
            }


            //同步逻辑
            if (time >= asynRate && isMoving)
            {
                Vector3 pos = transform.position;
                Vector3 rot = transform.rotation.eulerAngles;
                string message = ((int)MessageType.AsynInfo).ToString() + "_" + id.ToString() + "_"
                    + pos.x + "_" + pos.y + "_" + pos.z + "_"
                    + rot.x + "_" + rot.y + "_" + rot.z;

                clientScript.Send(message);
                time = 0.0f;
            }
            time += Time.fixedDeltaTime;
        }
    }
    public void ATK01()
    {

        tager = OnGetEnemy();
        if (tager)
        {
            transform.LookAt(tager);
        }
        m_speed = 0.0f;
        int_Atk = 1;
        _anim.SetInteger("Atk", int_Atk);
        StartCoroutine(StopAtk());

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
                    if (cols[i].tag.Equals("Monster") || cols[i].tag.Equals("BossMon"))
                        return cols[i].transform;
            //没有检测到Enemy,将检测半径扩大2米
            radius += 2;
        }
        return null;
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


    IEnumerator StopAtk()
    {
        yield return new WaitForSeconds(0.6f);
        int_Atk = 0;
        _anim.SetInteger("Atk", int_Atk);
        m_speed = 5.0f;
    }
}
