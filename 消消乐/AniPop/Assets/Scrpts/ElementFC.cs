using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementFC : MonoBehaviour 
{
    public ElementCtrl prefabElement;
    
    public ElementCtrl RandomCreateElement(int _row,int _col,Transform _parent)
    {
        var element=Instantiate<ElementCtrl>(prefabElement);
        element.Init(_row,_col);
        element.transform.parent=_parent;

        return element;
    }


	// 用于初始化 只执行一次
	void Start () 
	{
        Instance.prefabElement = this.prefabElement;
	}
	
	// 每帧都调用
	void Update () 
	{

    }
    #region 单例

    private ElementFC() { }

    public static readonly ElementFC Instance = new ElementFC();


    #endregion

}
