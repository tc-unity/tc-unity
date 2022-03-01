using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {
    private Vector3 offset = new Vector3(0, 10, -8);//相机相对于玩家的位置
    private Transform target;
    private Vector3 pos;
    public float speed = 2;

    // Use this for initialization
    void Start()
    {
        if (target==null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        pos = target.position + offset;
        this.transform.position = Vector3.Lerp(this.transform.position, pos, speed * Time.deltaTime);//调整相机与玩家之间的距离
        Quaternion angel = Quaternion.LookRotation(target.position - this.transform.position);//获取旋转角度
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, angel, speed * Time.deltaTime);

    }

}
