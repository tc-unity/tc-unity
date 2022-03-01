
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EntrustCtrl
{
    public enum EventType
    {
        ET_UpLeftHand,//左边玩家举手
        ET_UpRightHand,//右边玩家举手
        ET_LeftReady,//左边玩家准备
        ET_RightReady,//右边玩家准备
        ET_LeftHeadRaw,//左边玩家头像
        ET_RightHeadRaw,//右边玩家头像
        ET_HpLeft,//左边玩家血量
        ET_HpRight,//右边玩家血量
        ET_GameReady,//游戏准备
        ET_GameTime,//游戏时间
        ET_Explain,//说明面板
        ET_Setting,//设置面板
        ET_LeftComplete,//左边是否完成面板
        ET_RightComplete,//右边是否完成面板
        ET_LeftVictory,//左边是否胜利面板
        ET_RightVictory,//右边是否胜利面板
        ET_LeftRawActive,//左边头像显示
        ET_RightRawActive,//右边头像显示

    }

    Dictionary<EventType, Action<object[]>> EventVessel = new Dictionary<EventType, Action<object[]>>();//创建容器
    //事件绑定
    public void AddVessel(EventType _type, Action<object[]> _fun)
    {
        if (EventVessel.ContainsKey(_type))//判定字典里是否有该项
        {
            EventVessel[_type] += _fun;//绑定指定事件
            return;
        }
        EventVessel[_type] = _fun;
    }
    //解除绑定
    public void PopVessel(EventType _type, Action<object[]> _fun)
    {
        if (EventVessel.ContainsKey(_type))
        {
            EventVessel[_type] -= _fun;
        }
    }
    //事件触发
    public void TriggerEvent(EventType _type, object[] objs)
    {
        if (EventVessel.ContainsKey(_type))
        {
            EventVessel[_type](objs);//触发
        }
    }
    #region 单列
    EntrustCtrl() { }
    public static readonly EntrustCtrl Instance = new EntrustCtrl();
    #endregion
}
