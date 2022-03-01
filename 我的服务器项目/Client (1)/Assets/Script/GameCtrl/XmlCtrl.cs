using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;

public class XmlCtrl : MonoBehaviour {


    public static XmlCtrl xml_Instance;

    XmlDocument doc;
    string path;
    // Use this for initialization
    void Start () {
        xml_Instance = this;
        path = Application.persistentDataPath + "/Role.xml";
        SetFileToPersistent();
        LoadXml();
    }
	
	// Update is called once per frame
	void Update () {	
	}
    void SetFileToPersistent()
    {//StreamWriter一定要记得关闭.不关闭人家不鸟你之前做什么,
        FileInfo info = new FileInfo(path);
        Debug.Log(path);
        if (!info.Exists)
        {
            TextAsset ts = Resources.Load("Role") as TextAsset;
            string content = ts.text;
            StreamWriter sw = info.CreateText();
            sw.Write(content);
            sw.Close();
            sw.Dispose();
            Debug.Log("添加Xml文件成功");
        }
        else Debug.Log("已存在Xml文件");
    }
    /// <summary>
    /// 读取Xml 
    /// </summary>
    void LoadXml()
    {
        doc = new XmlDocument();
        doc.Load(path);
    }
    /// <summary>
    /// 读取Xml到结果面板上行 
    /// </summary>
    void ReadXml()
    {
        XmlNode node = GetNode();
        string str = node.InnerText;

    }
    /// <summary>
    /// 修改Xml数据
    /// </summary>
    void ChangeXmlData()
    {
        ////string value = inputBox.value;
        //if (value.Equals(""))
        //    return;


        //XmlNode node = GetNode();
        //node.InnerText = value;
        //Debug.Log(doc.OuterXml);
    }
    /// <summary>
    /// 保存xml
    /// </summary>
    void SaveXml()
    {
        doc.Save(path);
        Debug.Log("对保存有反应");
    }
    /// <summary>
    /// 获取那个唯一的节点
    /// </summary>
    /// <returns></returns>
    public XmlNode GetNode()
    {
        return doc.SelectSingleNode("Root");
    }
}
