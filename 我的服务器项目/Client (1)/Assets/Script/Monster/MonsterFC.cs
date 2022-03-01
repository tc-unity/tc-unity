using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class MonsterFC {

    Dictionary<int, MonsterPro> MonsterLib = new Dictionary<int, MonsterPro>();
    MonsterXML m_Xml= MonsterXML.monxml_Instance;
    void LoadItemLib()
    {
        //m_Xml = MonsterXML.monxml_Instance;
        var root = m_Xml.GetNode();
        foreach (XmlElement role in root.ChildNodes)
        {
         
            MonsterPro monster=new MonsterPro();
            monster.mId = int.Parse(role.GetAttribute("id"));
            monster.mName = role.GetAttribute("name");
            monster.mMaxHp = float.Parse(role.GetAttribute("maxHp"));
            monster.mHp = monster.mMaxHp;
            monster.mHurt = float.Parse(role.GetAttribute("hurt"));
            Debug.Log(role.GetAttribute("id"));
            Debug.Log(monster.mHp);

            MonsterLib.Add(monster.mId, monster);
        }
    }
    //构建怪物
    public MonsterPro CreatItem(int _id)
    {
        if (MonsterLib.ContainsKey(_id))
        {
          return new MonsterPro(MonsterLib[_id] as MonsterPro);
        }
        return null;
    }

    #region 单例
    MonsterFC()
    {
        LoadItemLib();
    }
    public static readonly MonsterFC Instance = new MonsterFC();
    #endregion
}
