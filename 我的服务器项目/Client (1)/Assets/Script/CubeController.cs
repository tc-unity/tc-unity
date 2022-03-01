using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubeController : MonoBehaviour {

	private int id;
	private Client clientScript=null;
    private float time = 0.0f;//计时器
    public float asynRate =0.1f;//同步间隔
    private bool isMoving = false;
    public GameObject skillPartical;

	public void SetId(int id){
		this.id=id;
	}

	// Use this for initialization
	void Start () {
		clientScript = GameObject.Find ("Client").GetComponent<Client>();
	}

	
	//以固定时间间隔向服务器端发送主角移动数据，防止客户端和服务器端不同步
	void FixedUpdate() {
		// 客户端套接字不能为空&&客户端套接字处于连接状态&&当前对象的ID要与当前客户端的ID相同
		if (clientScript.GetSocket()!=null && clientScript.GetSocket().Connected && id==clientScript.GetId()) {

               float dx=Input.GetAxis("Horizontal"); //在X轴上移动
               float dy=Input.GetAxis("Vertical"); //在y轴上移动

               if (Mathf.Abs(dx) > 0 || Mathf.Abs(dy) > 0)
                   isMoving = true;
               else
                   isMoving = false;

               Vector3 moveDir = new Vector3(dx, 0, dy);
               transform.Translate(5 * moveDir * Time.deltaTime);

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

                //检测技能按键
               if (Input.GetKeyDown(KeyCode.Space))
               {
                   string message = ((int)MessageType.SkillInfo).ToString() + "_" + id.ToString();
                   clientScript.Send(message);
               }


               //if (dx > 0) //向右
               //{
               //    消息类型+移动方向右
               //    clientScript.Send((int)MessageType.MoveInfo + "_" + "right");
               //}
               //if (dx < 0) //向左
               //{
               //    消息类型+移动方向左
               //    clientScript.Send((int)MessageType.MoveInfo + "_" + "left");
               //}
               //if (dy > 0) //向上
               //{
               //    消息类型+移动方向上
               //    clientScript.Send((int)MessageType.MoveInfo + "_" + "up");
               //}
               //if (dy < 0) //向下
               //{
               //    消息类型+移动方向下
               //    clientScript.Send((int)MessageType.MoveInfo + "_" + "down");
               //}
		}
	}


	//模型移动
	public void Moving(string dir)
	{
		int vertical = 0;
		int horizontal = 0;
		switch (dir) {
		case  "up":
			vertical = 1;
			break;
		case "down":
			vertical = -1;
			break;
		case "left":
			horizontal = -1;
			break;
		case "right":
			horizontal = 1;
			break;
		}
		
		Vector3 moveDir = new Vector3(horizontal, 0, vertical);
		transform.Translate(5 * moveDir * Time.deltaTime);
	}

    public void SkillEffect()
    {
        GameObject obj = Instantiate(skillPartical, transform.position, transform.rotation);
        Destroy(obj, 3.0f);
    }

}
