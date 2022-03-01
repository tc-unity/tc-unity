using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;





/*
 拖拽检测注意项：
 *  1.设置碰撞器组件
 *  2.为摄像机加入 射线2D发射组件
 *  3.选择UI为场景添加EventSystem对象 用于捕捉拖拽事件
 *  4.引用事件系统命名空间UnityEngine.EventSystems
 *  5.继承Unity事件系统中的 拖拽IDragHandler，开始拖拽IBeginDragHandler，结束拖拽接口IEndDragHandler
 */

public class GameCtrl : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler
{


    public GridPro Grid;
    public Transform FatherObject;
    public Transform ElementObject;

    ElementCtrl[,] elementArr;
    GridPro[,] gridArr;

    ElementCtrl beginDrag;
    ElementCtrl endDrag;

    bool isCanDrag = false;

    //可删除的元素容器
    List<ElementCtrl> listCanClear = new List<ElementCtrl>();

    public void GameInitGrid()
    {
        gridArr = new GridPro[GameSet.mRow, GameSet.mCol];
        for (int i = 0; i < GameSet.mRow; i++)
        {
            for (int j = 0; j < GameSet.mCol; j++)
            {
                var grid = GameObject.Instantiate<GridPro>(Grid);
                grid.Init(i, j);
                grid.transform.parent = FatherObject;
                gridArr[i, j] = grid;
            }
        }
    }
    public void InitElement()
    {
        elementArr = new ElementCtrl[GameSet.mRow, GameSet.mCol];
        foreach (var grid in gridArr)
        {
            var element = ElementFC.Instance.RandomCreateElement(grid.row, grid.col, ElementObject);
            element.transform.position = grid.transform.position;
            elementArr[grid.row, grid.col] = element;
        }
    }


	// 用于初始化 只执行一次
	void Start () 
	{
        GameInitGrid();
        InitElement();

        ElementObject.DOMoveY(10, 1).SetEase(Ease.OutBounce).From();

        StartCoroutine(GamePlay());
	}
	
	// 每帧都调用
	void Update () 
	{
        foreach (var e in elementArr)
        {
            //DOTween.IsTweening(e)   判断e元素 是否在做动画
            if (e != null && DOTween.IsTweening(e.transform))
            {
                isCanDrag = false;
                return;
            }

        }
        isCanDrag = true;
	}

    //游戏逻辑协程
    IEnumerator GamePlay()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            ClearElement();//清除可删除元素

            yield return new WaitForSeconds(0.7f);
            //下落元素逻辑
            FallElement();

            yield return new WaitForSeconds(0.6f);
            //生成新元素
            CreatElementInNull();

        }
    }
    void CreatElementInNull()
    {
        for (int i = 0; i < GameSet.mRow; i++)
        {
            for (int j = 0; j < GameSet.mCol; j++)
            {
                if (elementArr[i, j] == null)
                {
                    //创建元素
                    var element = ElementFC.Instance.RandomCreateElement(i, j, ElementObject);
                    elementArr[i, j] = element;//加入管控
                    element.transform.position = gridArr[i, j].transform.position;

                    //做动画
                    element.transform.DOMoveY(15, 0.5f).From();


                }
            }
        }
    }

    void FallElement()
    {
        for (int _col = 0; _col < GameSet.mCol; _col++)//从左至右
        {
            int count = 0;
            for (int _row = GameSet.mRow - 1; _row >= 0; _row--)//从下至上 遍历行
            {
                if (elementArr[_row, _col] == null)
                {
                    count++;
                }
                else
                {
                    if (count > 0)
                    {
                        //做动画
                        elementArr[_row, _col].transform.DOMove(gridArr[_row + count, _col].transform.position, 0.5f);

                        //数组引用赋值
                        elementArr[_row + count, _col] = elementArr[_row, _col];
                        elementArr[_row, _col] = null;

                        //更新行属性
                        elementArr[_row + count, _col].row = _row + count;
                    }
                }
            }
        }
    }

    public  void OnBeginDrag(PointerEventData eventData)
    {
        beginDrag = null;
        endDrag = null;

        if (isCanDrag == false)
        {
            return;
        }

        foreach (var e in elementArr)
        {
            //判断点击的位置 是否在 元素的矩形区域内
            if (e != null && e.PosInRect(eventData.pointerPressRaycast.worldPosition))
            {
                //点中元素
                beginDrag = e;
                print(e.transform.name);
                break;
            }
        }
    }
   public  void OnEndDrag(PointerEventData eventData)
    {
        if (isCanDrag == false)
        {
            return;
        }

        foreach (var e in elementArr)
        {
            //判断拖拽结束的鼠标位置 是否在 元素的矩形区域内
            if (e != null && e.PosInRect(eventData.pointerCurrentRaycast.worldPosition))
            {
                //点中元素
                endDrag = e;
                print(e.transform.name);
                break;
            }
        }

        if (beginDrag && endDrag)
        {
            //同行 列相差1  同列 行相差1
            if (beginDrag.row == endDrag.row && Mathf.Abs(beginDrag.col - endDrag.col) == 1
                || beginDrag.col == endDrag.col && Mathf.Abs(beginDrag.row - endDrag.row) == 1)
            {

                if (isCanClear())//判断是否能够消除
                {
                    beginDrag.transform.DOMove(endDrag.transform.position, 0.5f);
                    endDrag.transform.DOMove(beginDrag.transform.position, 0.5f);
                }
                else
                {
                    beginDrag.transform.DOMove(endDrag.transform.position, 0.5f).SetLoops(2, LoopType.Yoyo);
                    endDrag.transform.DOMove(beginDrag.transform.position, 0.5f).SetLoops(2, LoopType.Yoyo);
                }

            }


        }

    }

   bool isCanClear()
   {
       //假设能交换  更新数组 和  两个元素的属性
       elementArr[beginDrag.row, beginDrag.col] = endDrag;
       elementArr[endDrag.row, endDrag.col] = beginDrag;
       beginDrag.Swap(endDrag);//更换名字 行列信息

       //第一个元素
       List<ElementCtrl> liHor = new List<ElementCtrl>();
       GetHorElement(elementArr[beginDrag.row, beginDrag.col], liHor);
       List<ElementCtrl> liVer = new List<ElementCtrl>();//纵向容器
       GetVerElement(elementArr[beginDrag.row, beginDrag.col], liVer);
       if (liHor.Count >= 3 || liVer.Count >= 3)
       {
           return true;
       }

       //第二个元素
       liHor.Clear();
       liVer.Clear();
       GetHorElement(elementArr[endDrag.row, endDrag.col], liHor);
       GetVerElement(elementArr[endDrag.row, endDrag.col], liVer);
       if (liHor.Count >= 3 || liVer.Count >= 3)
       {
           return true;
       }


       //不能交换逻辑  则交换回来
       elementArr[beginDrag.row, beginDrag.col] = endDrag;
       elementArr[endDrag.row, endDrag.col] = beginDrag;
       beginDrag.Swap(endDrag);//更换名字 行列信息
       return false;
   }

   void ClearElement()
   {
       foreach (var e in elementArr)
       {
           if (e != null)
           {
               List<ElementCtrl> liHor = new List<ElementCtrl>();//横向容器
               GetHorElement(e, liHor);
               List<ElementCtrl> liVer = new List<ElementCtrl>();//纵向容器
               GetVerElement(e, liVer);
           }

       }

       //遍历可删除容器  删除元素
       foreach (var e in listCanClear)
       {
           if (e != null)
           {
               e.transform.DOScale(Vector3.zero, 0.5f);
               GameObject.Destroy(e.gameObject, 0.6f);
           }
       }
       listCanClear.Clear();//清空容器
   }

   void GetHorElement(ElementCtrl _e, List<ElementCtrl> _li)
   {
       //装自己
       _li.Add(_e);

       //以参考点为中心 向左循环
       for (int _col = _e.col - 1; _col >= 0; _col--)
       {
           if (_e != null && _e.type == elementArr[_e.row, _col].type)//相同元素
           {
               _li.Add(elementArr[_e.row, _col]);//装入容器
           }
           else
           {
               break;
           }
       }
       //以参考点为中心 向右循环
       for (int _col = _e.col + 1; _col < GameSet.mCol; _col++)
       {
           if (_e != null && _e.type == elementArr[_e.row, _col].type)//相同元素
           {
               _li.Add(elementArr[_e.row, _col]);//装入容器
           }
           else
           {
               break;
           }
       }

       if (_li.Count >= 3)
       {
           listCanClear.AddRange(_li);//将可以删除的元素 装入可删除的大容器
       }
   }



   void GetVerElement(ElementCtrl _e, List<ElementCtrl> _li)
   {
       //装自己
       _li.Add(_e);

       //以参考点为中心 向上循环
       for (int _row = _e.row - 1; _row >= 0; _row--)
       {
           if (_e != null && _e.type == elementArr[_row, _e.col].type)//相同元素
           {
               _li.Add(elementArr[_row, _e.col]);//装入容器
           }
           else
           {
               break;
           }
       }
       //以参考点为中心 向下循环
       for (int _row = _e.row + 1; _row < GameSet.mRow; _row++)
       {
           if (_e != null && _e.type == elementArr[_row, _e.col].type)//相同元素
           {
               _li.Add(elementArr[_row, _e.col]);//装入容器
           }
           else
           {
               break;
           }
       }

       if (_li.Count >= 3)
       {
           listCanClear.AddRange(_li);//将可以删除的元素 装入可删除的大容器
       }

   }
   public  void OnDrag(PointerEventData eventData)
    {

    }

}
