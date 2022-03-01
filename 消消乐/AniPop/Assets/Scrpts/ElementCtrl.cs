using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementCtrl : MonoBehaviour 
{
    public int row;
    public int col;

    public Sprite[] spriteArr;

    public int type;

    Rect rect;
    public void Init(int _row,int _col)
    {
        type = Random.Range(0, spriteArr.Length);
        GetComponent<SpriteRenderer>().sprite = spriteArr[type];

        row = _row;
        col = _col;
        transform.name = "Element:" + row + "," + col;

    }

    public bool PosInRect(Vector2 pos)
    {
        rect = new Rect(
            transform.position.x - GameSet.mWidth / 2,
            transform.position.y - GameSet.mHight / 2,
            GameSet.mWidth,
            GameSet.mHight
            );
        return rect.Contains(pos);
    }
    public void Swap(ElementCtrl _e)
    {
        string s = _e.name;
        _e.name = this.name;
        this.name = s;

        int t = _e.col;
        _e.col = this.col;
        this.col = t;

        t = _e.row;
        _e.row = this.row;
        this.row = t;

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
