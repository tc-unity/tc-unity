using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.IO;
using System.Linq;
using System;

public class PlayerCtrl : MonoBehaviour {
    #region 属性

    public int mId { get; set; }
    public string mName { get; set; }
    public string mProfession { get; set; }
    public int mLv { get; set; }
    public int mMaxHp { get; set; }
    public int mHp { get; set; }
    public int mMaxMp { get; set; }
    public int mMp { get; set; }

    public int mExp { get; set; }
    public int mMaxExp { get; set; }
    public int mHurt { get; set; }//伤害
    public int mArmor { get; set; }//速度

    public int mKill { get; set; }
    public int mSurplus { get; set; }
    public int mScore { get; set; }//伤害
    public int mLevel { get; set; }//速度



    #endregion
    #region 单例
    public static PlayerCtrl Instance;
    #endregion
    XmlCtrl m_Xml;
    // Use this for initialization
    void Awake() {

        Instance = this;

    }
	void Start()
    {
        mKill = 0;
        mSurplus = 2;
        mScore = 0;
        mLevel = 1;

        m_Xml = XmlCtrl.xml_Instance;
        var root = m_Xml.GetNode();
        foreach (XmlElement role in root.ChildNodes)
        {
            mId = int.Parse(role.GetAttribute("id"));
            mName = role.GetAttribute("name");
            mLv = int.Parse(role.GetAttribute("lv"));
            mMaxHp = int.Parse(role.GetAttribute("maxHp"));
            mHp = PlayerCtrl.Instance.mMaxHp;
            mMaxMp = int.Parse(role.GetAttribute("maxMp"));
            mMp = PlayerCtrl.Instance.mMaxMp;
            mHurt = int.Parse(role.GetAttribute("hurt"));
            mArmor = int.Parse(role.GetAttribute("armor"));
            mMaxExp = int.Parse(role.GetAttribute("maxExp"));
        }
        Debug.Log(mName);
        //StartCoroutine(InitXml());
    }
    // Update is called once per frame
    void Update () {
		
	}
    #region 升级
    void LvUp()
    {
        mLv++;
        mMaxHp += (int)(mMaxHp * 0.2f);//血量提升
        mHp = mMaxHp;
        mMaxMp += (int)(mMaxMp * 0.2f);//血量提升
        mMp = mMaxMp;
        //经验
        mExp -= mMaxExp;
        mMaxExp += (int)(mMaxExp * 0.2f);
        if (mExp >= mMaxExp)
        {
            LvUp();
        }
    }
    //经验获取
    public void GexExp(int _exp)
    {
        mExp += _exp;
        if (mExp >= mMaxExp)
        {
            LvUp();
            EntrustCtrl.Instance.TriggerEvent(EventType_Game.ET_LvUp);
        }
        EntrustCtrl.Instance.TriggerEvent(EventType_Game.ET_AttributeUpdate);
    }
    #endregion

    //IEnumerator InitXml()
    //{
    //    string filePath;
    //    if (Application.platform == RuntimePlatform.Android)
    //    {
    //        //安卓端写法
    //        filePath = Application.streamingAssetsPath + GameFind.roleXmlPath;
    //    }
    //    else
    //    {
    //        //PC端写法
    //        filePath = "file://" + Application.streamingAssetsPath + GameFind.roleXmlPath;
    //    }
    //    Debug.Log(filePath);
    //    WWW www = new WWW(filePath);
    //    if (www.isDone)
    //    {
    //        //xml读取
    //        XmlDocument doc = new XmlDocument();
    //        doc.LoadXml(www.text);

    //        var root = doc.SelectSingleNode("Root") as XmlElement;
    //        foreach (XmlElement role in root.ChildNodes)
    //        {
    //            mId = int.Parse(role.GetAttribute("id"));
    //            mName = role.GetAttribute("name");
    //            mLv = int.Parse(role.GetAttribute("lv"));
    //            mMaxHp = int.Parse(role.GetAttribute("maxHp"));
    //            mHp = PlayerCtrl.Instance.mMaxHp;
    //            mMaxMp = int.Parse(role.GetAttribute("maxMp"));
    //            mMp = PlayerCtrl.Instance.mMaxMp;
    //            mHurt = int.Parse(role.GetAttribute("hurt"));
    //            mArmor = int.Parse(role.GetAttribute("armor"));
    //            mMaxExp = int.Parse(role.GetAttribute("maxExp"));
    //        }
    //    }
    //    Debug.Log(mName);
    //    yield return null;
    //}
}
