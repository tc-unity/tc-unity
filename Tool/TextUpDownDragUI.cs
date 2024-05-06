using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class TextUpDownDragUI : MonoBehaviour
{
    [Header("文本数组:奇数，根据此数量判定容器添加多少空位")]
    [SerializeField] private TextMeshProUGUI[] numTexts;
    [Header("数值范围")]
    [SerializeField] private int minNum = 1;                   
    [SerializeField] private int maxNum = 100;                  
    [Header("拖拽时的量,大于此值拖拽有效")]
    [SerializeField] private float dragMoveOffset = 5;       
    
    private List<string> numList = new List<string>();  //数值数组
    private int numListIndex = 25;                      //当前数组的下标，中心位
    private int rowCount = 3;                           //行数，通过文本数组初始化
    private int isAddOffset = 1;                        //通过上划或者下划设置加减 +1 -1
    private Vector3 oldDragPos;                         //上一次拖拽位置

    /// <summary>
    /// 获取当前中心选择的数值
    /// </summary>
    public int GetCurNum { get { return int.Parse(numList[numListIndex]); } }
    // Start is called before the first frame update
    void Start()
    {
        //绑定事件
        Utils.AddTriggerEvent(this.gameObject, EventTriggerType.PointerDown, OnPointerDown);
        Utils.AddTriggerEvent(this.gameObject, EventTriggerType.Drag, OnDrag);
        Utils.AddTriggerEvent(this.gameObject, EventTriggerType.PointerUp, OnPointerUp);
        //添加数值进数组
        for (int i = minNum; i <= maxNum; i++)
            numList.Add(i.ToString());
        //收尾添加空字符
        rowCount = numTexts.Length;
        int insert = rowCount / 2;
        for (int i = 0; i < insert; i++)
        {
            numList.Insert(0," ");
            numList.Insert(numList.Count, " ");
        }
        //更新UI
        UpdateTextUI();
    }
    //更新UI
    private void UpdateTextUI()
    {
        //获取除去中位的数量
        int other = rowCount / 2;
        //增加或减少数组的index
        numListIndex += isAddOffset;
        //设置拖拽时的最大和最小范围
        if (numListIndex<= other)
        {
            numListIndex = other;
        }
        if (numListIndex >= numList.Count -1 - other)
        {
            numListIndex = numList.Count - 1 - other;
        }
        //设置除中位的其他文本和颜色
        for (int i = 1; i <= other; i++)
        {
            numTexts[i - 1].text = numList[numListIndex- (other - i + 1)];
            numTexts[i - 1].color = new Color(0, 0, 0, 0.5f);
            numTexts[rowCount - i].text = numList[numListIndex + (other - i+1)];
            numTexts[rowCount - i].color = new Color(0, 0, 0, 0.5f);
        }
        //设置中位文本和颜色
        numTexts[numTexts.Length - 1 - other].text = numList[numListIndex];
        numTexts[numTexts.Length - 1 - other].color = new Color(0, 0, 0, 1);
    }
    //鼠标按下
    private void OnPointerDown(BaseEventData data)
    {
        oldDragPos = Input.mousePosition;
    }
    //拖拽
    private void OnDrag(BaseEventData data)
    {
        //判断加减
        if (Input.mousePosition.y > oldDragPos.y)
        {
            //+
            isAddOffset = 1;
        }
        else
        {
            //-
            isAddOffset = -1;
        }
        //达到拖拽范围则更新UI
        if (Mathf.Abs(Input.mousePosition.y - oldDragPos.y)>= dragMoveOffset)
        {
            UpdateTextUI();
            oldDragPos = Input.mousePosition;
        }

    }

    private void OnPointerUp(BaseEventData data)
    {
        
    }

}
