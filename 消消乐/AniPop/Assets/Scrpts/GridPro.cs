using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPro : MonoBehaviour 
{
    public int row;
    public int col;

    public Sprite[] SpriteArr;
    public SpriteRenderer sRender;

    public void Init(int _row,int _col)
    {
        row = _row;
        col = _col;
        //选用精灵
        sRender.sprite = SpriteArr[(row + col) % 2];
        //给对象取名
        gameObject.name = "Grid:" + row + "," + col;


        //设置位置
        transform.position = new Vector2(
            GameSet.Instance.TopPlace.x + _col * GameSet.mWidth,
            GameSet.Instance.TopPlace.y - _row * GameSet.mHight

            );


    }


	// 用于初始化 只执行一次
	void Start () 
	{
		
	}
	
	// 每帧都调用
	void Update () 
	{
		
	}
}
