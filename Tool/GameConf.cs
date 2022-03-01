using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using STA.Settings;
using System.IO;
using Excel;
using System.Data;
//玩家类
public class GamePalyer
{
    public int m_id { get; set; }
    public int m_score { get; set; }
    public Color m_color { get; set; }
    GamePalyer() { }
    public GamePalyer(int _id,int score)
    {
        m_id = _id;
        m_score = score;
    }
    public void SetColor()
    {
        if (m_score>=10)
        {
            m_color = GameConf.m_settlementColor[2];
        }
        else if (m_score < 10&& m_score>=5)
        {
            m_color = GameConf.m_settlementColor[1];
        }
        else if (m_score < 5)
        {
            m_color = GameConf.m_settlementColor[0];
        }
    }
}
//题目类
public class GameTopic
{
    public int m_index = 0;
    public string m_content = "";
    GameTopic() { }
    public GameTopic(int _id, string _content)
    {
        m_index = _id;
        m_content = _content;
    }
}
public class GameConf : MonoBehaviour {

    public static bool m_openTracking = true;//是否打开感应.
    public static bool m_rotaX = false;
    public static bool m_rotaY = false;
    public static float m_timer = 0;//每次答题时间
    public static int m_topicCount = 0;//题目数量
    public static int m_playerCount = 0;//玩家数量
    public static string m_topicFileName = " ";//题库文件名
    public static List<Color> m_colorList = new List<Color>();//所有颜色容器
    public static List<Color> m_settlementColor = new List<Color>();//结算颜色

    //玩家容器
    public static List<GamePalyer> m_gamePayer = new List<GamePalyer>();
    //题目容器
    public static List<GameTopic> m_gameTopic = new List<GameTopic>();

    /// <summary>
    /// 读取配置
    /// </summary>
    public static void ReadConfiguration()
    {
        m_gamePayer.Clear();
        m_colorList.Clear();
        //读INI
        INIFile iniFile = new INIFile(Application.streamingAssetsPath + "\\GameConf.ini");
        m_rotaX = iniFile.GetValue("Params", "RotaX", false);
        m_rotaY = iniFile.GetValue("Params", "RotaY", false);
        m_openTracking = iniFile.GetValue("Params", "OpenTracking", false);
        m_timer = iniFile.GetValue("Params", "Timer", 0);
       // m_topicCount = iniFile.GetValue("Params", "TopticCount", 0);
        m_playerCount = iniFile.GetValue("Params", "PlayerCount", 0);
        //初始化玩家
        for (int i = 0; i < m_playerCount; i++)
        {
            m_gamePayer.Add(new GamePalyer(i,0));
        }
        m_topicFileName = iniFile.GetValue("Params", "TopicFileName", " ");
        //读取颜色
        float r = 0;
        float g = 0;
        float b = 0;
        float a = 0;
        string colorAll = iniFile.GetValue("Params", "Colors", " ");
        string[] color = colorAll.Split(';');
        for (int i = 0; i < color.Length; i++)
        {
            string[] vaule = color[i].Split(',');
            r = float.Parse(vaule[0]);
            g = float.Parse(vaule[1]);
            b = float.Parse(vaule[2]);
            a = float.Parse(vaule[3]);
            m_colorList.Add(new Color(r / 255, g / 255, b / 255, a / 255));
        }

        //读取颜色
        float _r = 0;
        float _g = 0;
        float _b = 0;
        float _a = 0;
        string _colorAll = iniFile.GetValue("Params", "SettlementColor", " ");
        string[] _color = _colorAll.Split(';');
        for (int i = 0; i < _color.Length; i++)
        {
            string[] vaule = _color[i].Split(',');
            _r = float.Parse(vaule[0]);
            _g = float.Parse(vaule[1]);
            _b = float.Parse(vaule[2]);
            _a = float.Parse(vaule[3]);
            m_settlementColor.Add(new Color(_r / 255, _g / 255, _b / 255, _a / 255));
        }

    }
    /// <summary>
    /// 读取xlsx
    /// </summary>
    public static void XLSX()
    {
        m_gameTopic.Clear();
        string filePath = Application.dataPath + "\\..\\data\\" + m_topicFileName + ".xlsx";
        Debug.Log("Read xlsx:" + filePath);
        FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        do
        {
            //Debug.Log(excelReader.Name);
            //List<questionObject> tableList = new List<questionObject>();
            //dataList.Add(tableList);
            bool isTitle = true;
            Debug.Log("excelReader.Read() : " + excelReader.Read());
            while (excelReader.Read())
            {
                //if (isTitle)
                //{
                //    isTitle = false;
                //    continue;
                //}
                List<string> temp = new List<string>();
                
                bool isErrorData = false;
                for (int i = 0; i < excelReader.FieldCount; i++)
                {
                    string value = excelReader.IsDBNull(i) ? "" : excelReader.GetString(i);

                    if (!value.Trim().Equals(""))
                    {
                        //Debug.Log(value);
                        temp.Add(value);
                    }
                    else
                    {
                        isErrorData = true;
                        Debug.Log(i + " is error data!");
                        break;
                    }
                }
                if (!isErrorData)
                {
                    int index = 0;
                    string content = " ";
                    if (temp[0]!="")
                    {
                        index = int.Parse(temp[0]);
                    }
                    if (temp[1] != "")
                    {
                        content = temp[1];
                    }

                    m_gameTopic.Add(new GameTopic(index, content));
                }
            }
            Debug.Log("m_gameTopic.count:" + m_gameTopic.Count);
        } while (excelReader.NextResult());

    }
}
