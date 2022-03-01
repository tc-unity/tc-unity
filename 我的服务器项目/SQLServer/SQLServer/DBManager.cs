using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Mono.Data.Sqlite;
using System.Data;

namespace SQLServer
{
    class DBManager
    {
        private static DbAccess m_DB;//数据库对象
        private static DBManager m_Instance;
        public static DBManager Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new DBManager();
                }
                return m_Instance;
            }
        }

        private DBManager()
        {

        }

        //初始化数据库
        public void InitDB()
        {
            string dbPath = Directory.GetCurrentDirectory() + "//..//..//Data//UserInfo.db";
            m_DB = new DbAccess("URI=file:" + dbPath);

            m_DB.CreateTable("UserTable ",
                new string[] { 
                    "Account", 
                    "Password", 
                    "RoleName", 
                    "PosX", "PosY", "PosZ",
                    "RotX", "RotY", "RotZ" ,
                    "ColorR","ColorG","ColorB",
                },
                new string[] {
                    "VARCHAR(20) NOT NULL", 
                    "VARCHAR(20) NOT NULL", 
                    "VARCHAR(20)", 
                    "FLOAT", "FLOAT", "FLOAT", 
                    "FLOAT", "FLOAT", "FLOAT",
                    "FLOAT", "FLOAT", "FLOAT",
                });
        }

        //判断账号是否存在
        public bool CheckAccountExist(string account)
        {
            IDataReader sqReader = m_DB.SelectWhere("UserTable",
                            new string[] { "Account" },
                            new string[] { "Account" },
                            new string[] { "=" },
                           new string[] { string.Format("{0}", account) });

            bool res = false;
            //如果可读说明数据库中有该ID
            if (sqReader.Read())
            {
                res = true;
            }
            if (!sqReader.IsClosed)
            {
                sqReader.Close();
            }
            return res;
        }

        //检测账号和密码是否正确
        public bool CheckAccountAndPsw(string account,string password)
        {
            IDataReader sqReader = m_DB.SelectWhere("UserTable",
                            new string[] {"Password" },
                            new string[] { "Account" },
                            new string[] { "=" },
                           new string[] { string.Format("{0}", account) });
            bool res = false;

            if (sqReader.Read())//如果读到信息，说明账号正确
            {
                if (password == sqReader.GetString(0))//检测密码是否正确
                {
                    res = true;
                }                        
            }

            if (!sqReader.IsClosed)
            {
                sqReader.Close();
            }

            return res;
        }

        //通过账号获取角色名
        public string GetRoleNameByAccount(string account)
        {
            string res = "";

            IDataReader sqReader = m_DB.SelectWhere("UserTable",
                            new string[] { "RoleName" },
                            new string[] { "Account" },
                            new string[] { "=" },
                           new string[] { string.Format("{0}", account) });

            if (sqReader.Read())
            {
                res =  sqReader.GetString(0);
            }
            if (!sqReader.IsClosed)
            {
                sqReader.Close();
            }

            return res;
        }


        //插入新的账号和密码
        public void InsertAccountAndPsw(string account,string password)
        {
            IDataReader sqReader = m_DB.InsertInto("UserTable", new string[]
                                     {string.Format("'{0}'",account),
                                       string.Format("'{0}'",password),
                                      string.Format("'{0}'",""),
                                      string.Format("{0}",3.22f),string.Format("{0}",2.6f),string.Format("{0}",18.7f),
                                      string.Format("{0}",0.0f),string.Format("{0}",0.0f),string.Format("{0}",0.0f),
                                      string.Format("{0}",1.0f),string.Format("{0}",1.0f),string.Format("{0}",1.0f),
                                     });

            if (!sqReader.IsClosed)
            {
                sqReader.Close();
            }
        }


        //更新角色名
        public  void UpdateRoleName(string account,string name)
        {
            IDataReader sqReader = m_DB.UpdateInto("UserTable",
                new string[] { "RoleName" },
                new string[] { string.Format("'{0}'", name), },
                "Account", string.Format("'{0}'", account));

            if (!sqReader.IsClosed)
            {
                sqReader.Close();
            }
        }

        //更新角色颜色
        public void UpdateRoleColor(string account, float r,float g,float b)
        {
            IDataReader sqReader = m_DB.UpdateInto("UserTable",
                new string[] { "ColorR", "ColorG", "ColorB", },
                new string[] { string.Format("{0}", r), string.Format("{0}", g), string.Format("{0}",b),},
                "Account", string.Format("'{0}'", account));

            if (!sqReader.IsClosed)
            {
                sqReader.Close();
            }
        }

        //通过账号获取位置和旋转
        public bool GetPosAndRot(string account,out Vector3 pos,out Vector3 rot)
        {
            IDataReader sqReader = m_DB.SelectWhere("UserTable",
                            new string[] { "PosX", "PosY", "PosZ","RotX", "RotY", "RotZ"},
                            new string[] { "Account" },
                            new string[] { "=" },
                           new string[] { string.Format("{0}", account) });
           
            bool res = false;
            pos = new Vector3();
            rot = new Vector3();

            if (sqReader.Read())
            {
                res = true;
                pos.x = sqReader.GetFloat(0);
                pos.y = sqReader.GetFloat(1);
                pos.z = sqReader.GetFloat(2);

                rot.x = sqReader.GetFloat(3);
                rot.y = sqReader.GetFloat(4);
                rot.z = sqReader.GetFloat(5);
            }

            if (!sqReader.IsClosed)
            {
                sqReader.Close();
            }
            return res;
        }


        //通过账号来获取颜色
        public bool GetColor(string account, out Color color)
        {
            IDataReader sqReader = m_DB.SelectWhere("UserTable",
                            new string[] { "ColorR", "ColorG", "ColorB"},
                            new string[] { "Account" },
                            new string[] { "=" },
                           new string[] { string.Format("{0}", account) });
            bool res = false;
            color = new Color();

            if (sqReader.Read())
            {
                res = true;
                color.r = sqReader.GetFloat(0);
                color.g = sqReader.GetFloat(1);
                color.b = sqReader.GetFloat(2);
            }

            if (!sqReader.IsClosed)
            {
                sqReader.Close();
            }
            return res;
        }

        public void SaveRoleInfo(RoleInfo info)
        {
            IDataReader sqReader = m_DB.UpdateInto("UserTable",
                new string[] { "PosX", "PosY", "PosZ", },
                new string[] { string.Format("{0}", info.position.x), string.Format("{0}", info.position.y), string.Format("{0}", info.position.z), },
                "Account", string.Format("'{0}'", info.account));

            if (!sqReader.IsClosed)
            {
                sqReader.Close();
            }
        }
        
        //关闭数据库
        public void CloseDB()
        {
            if (m_DB != null)
                m_DB.CloseSqlConnection();
        }
    }
}
