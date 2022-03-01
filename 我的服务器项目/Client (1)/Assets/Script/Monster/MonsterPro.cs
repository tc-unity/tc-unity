using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPro {
    #region 属性
    public int mId { get; set; }
    public string mName { get; set; }
    public int mLv { get; set; }
    public float mMaxHp { get; set; }
    public float mHp { get; set; }
    public float mHurt { get; set; }//伤害
    public float mArmor { get; set; }//速度
    #endregion
    //无参构造
    public MonsterPro() { }
    //拷贝构造
    public MonsterPro(MonsterPro _mon)
    {
        mId = _mon.mId;
        mName = _mon.mName;
        mLv = _mon.mLv;
        mMaxHp = _mon.mMaxHp;
        mHp = _mon.mHp;
        mHurt = _mon.mHurt;
        mArmor = _mon.mArmor;

    }

}
