using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum EventType_Game
{
    ET_LvUp,
    ET_AttributeUpdate,//属性更新
    ET_Task,//任务
    ET_Gold,//金币
    ET_MonHp,//怪物掉血
    ET_MonProduce,//怪物产生
    ET_MonDie,//怪物死亡
}
public class EntrustCtrl
{
    Dictionary<EventType_Game, Action> EventVessel = new Dictionary<EventType_Game, Action>();//创建容器
    //事件绑定
    public void AddVessel(EventType_Game _type,Action _fun)
    {
        if (EventVessel.ContainsKey(_type))//判定字典里是否有该项
        {
            EventVessel[_type] += _fun;//绑定指定事件
            return;
        }
        EventVessel[_type] = _fun;
    }
    //解除绑定
    public void PopVessel(EventType_Game _type,Action _fun)
    {
        if (EventVessel.ContainsKey(_type))
        {
            EventVessel[_type] -= _fun;
        }
    }
    //事件触发
    public void TriggerEvent(EventType_Game _type)
    {
        if (EventVessel.ContainsKey(_type))
        {
            EventVessel[_type]();//触发
        }
    }
    #region 单列
    EntrustCtrl() { }
    public static readonly EntrustCtrl Instance = new EntrustCtrl();
    #endregion

}
