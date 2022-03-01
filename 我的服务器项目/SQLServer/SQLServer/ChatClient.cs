using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace SQLServer
{
    public struct Vector3
    {
        public float x;
        public float y;
        public float z;
        public Vector3(float _x, float _y,float _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }
    }

    public struct Color
    {
        public float r;
        public float g;
        public float b;
    }

    public struct RoleInfo
    {
        public string account;
        public string name;
        public Vector3 position;
        public Vector3 rotation;
        public Color color;
        public int sceneID;
    }

    //服务器端存放连接的客户端的对象
    public class ChatClient
    {
        private static int global_id = 0;//静态ID，用来自增长
        public int id;//当前对象的ID
        public string ipAddress;//IP地址
        public int port;//端口
        public Socket socket;//套接字
        private byte[] data; 	//消息数据
        private string messPackage = ""; //消息包
        private List<string> messPackageArray = new List<string>(); //消息包数组

        public RoleInfo roleInfo;

        public bool revAsynInfo = false;
        public string AsynInfoMess = "";
        public bool revSkillInfo = false;
        public string SkillInfoMess = "";

        public ChatClient(Socket client)
        {
            global_id++;
            id = global_id;

            socket = client;
            ipAddress = ((IPEndPoint)socket.RemoteEndPoint).Address.ToString();
            port = ((IPEndPoint)socket.RemoteEndPoint).Port;
            data = new byte[256];//从客户端接收的消息大小

            //把当前客户端实例添加到客户列表当中
            Server.players_obj.Add(id, this);

            //从客户端获取消息
            socket.BeginReceive(data, 0, data.Length, 0, new AsyncCallback(ReceiveCallback), socket);
        }

        //接受消息的回调函数,用户缓冲区获得socket缓冲区中的数据后调用,接收客户端消息并非由此函数决定,而是由tcp协义决定
        void ReceiveCallback(IAsyncResult ar)
        {
            Socket socket = (Socket)ar.AsyncState;
            // Read data from the client socket.     
            int bytesRead = socket.EndReceive(ar);
            if (bytesRead <= 0)
            {
                //客户端断开
                Server.destroyUser = this;

                //向所有客户端广播，有玩家退出
                string dsyMsg = ((int)MessageType.DestroyInfo).ToString() + "_" + id.ToString();
                Broadcast(dsyMsg);
                return;
            }
            else
            {
                //获取消息 
                string message = Encoding.ASCII.GetString(data, 0, bytesRead);
                Console.WriteLine("消息:" + message);

                messPackageArray.Clear();
                //解析包
                AnalyzePackage(message);

                foreach (string mess in messPackageArray)
                {
                    string[] tmpArray = mess.Split('_');//1_3_2

                    if ((MessageType)(int.Parse(tmpArray[0])) == MessageType.Login)
                    {
                        string account = tmpArray[1];
                        string password = tmpArray[2];

                        string msg = ((int)MessageType.Login).ToString();
                        if (DBManager.Instance.CheckAccountAndPsw(account,password))
                        {
                            msg += "_" + 1;

                            //记录当前玩家的账号
                            roleInfo.account = account;
                            //记录玩家位置和旋转
                            DBManager.Instance.GetPosAndRot(account, out roleInfo.position, out roleInfo.rotation);
                            //记录玩家的颜色
                            DBManager.Instance.GetColor(account, out roleInfo.color);

                            string roleName=DBManager.Instance.GetRoleNameByAccount(account);
                            if (roleName == "")//没有角色
                            {
                                msg += "_" + 0;
                            }
                            else//有角色
                            {
                                roleInfo.name = roleName;//记录玩家角色名
                                msg += "_" + 1 +"_"+ roleName;
                            }
                        }
                        else
                        {
                            msg += "_" + 0; 
                        }
                        Send(msg);
                    }
                    else if ((MessageType)(int.Parse(tmpArray[0])) == MessageType.Register)
                    {
                        string account = tmpArray[1];
                        string password = tmpArray[2];
                        
                        string msg = ((int)MessageType.Register).ToString();

                        if (DBManager.Instance.CheckAccountExist(account))//如果账号已经存在
                        {
                            msg += "_" + 0;                        
                        }
                        else
                        {
                            DBManager.Instance.InsertAccountAndPsw(account, password);//账号不存在则插入到数据库
                            msg += "_" + 1;   
                        }

                        Send(msg);
                    }
                    else if ((MessageType)(int.Parse(tmpArray[0])) == MessageType.CreateRole)
                    {
                        string roleName = tmpArray[1];
                        if (!string.IsNullOrEmpty(roleName))
                        {
                            DBManager.Instance.UpdateRoleName(roleInfo.account,roleName);     
                            roleInfo.name = roleName;//记录玩家角色名
                            
                            roleInfo.color.r = float.Parse(tmpArray[2]);
                            roleInfo.color.g = float.Parse(tmpArray[3]);
                            roleInfo.color.b = float.Parse(tmpArray[4]);
                            DBManager.Instance.UpdateRoleColor(
                                roleInfo.account, roleInfo.color.r, roleInfo.color.g, roleInfo.color.b);

                            string msg = ((int)MessageType.CreateRole).ToString();
                            Send(msg);
                        }
                    }
                    else if ((MessageType)(int.Parse(tmpArray[0])) == MessageType.CreateInfo)
                    {
                        Server.Instance.CreateRoleModel(this);
                    }
                    else if ((MessageType)(int.Parse(tmpArray[0])) == MessageType.ChangeScene)
                    {
                        roleInfo.sceneID = int.Parse(tmpArray[1]);
                    }
                    else if ((MessageType)(int.Parse(tmpArray[0])) == MessageType.SaveRoleInfo)
                    {
                        DBManager.Instance.SaveRoleInfo(roleInfo);
                    }
                    //聊天消息
                    if ((MessageType)(int.Parse(tmpArray[0])) == MessageType.TextInfo)
                    {
                        //string msg = "\n" +  + "[" + tmpArray[1] + "]";

                        //消息类型+用户id+聊天消息
                        string chatMsg = ((int)MessageType.TextInfo).ToString() + "_" + roleInfo.name + "_" + tmpArray[1];
                        //向所有客户端广播，玩家的聊天信息
                        Broadcast(chatMsg);
                    }
                    //移动消息
                    //else if ((MessageType)(int.Parse(tmpArray[0])) == MessageType.MoveInfo)
                    //{
                    //    moveDirArray.Add(tmpArray[1]);

                    //    //消息类型+用户id+移动方向
                    //    string moveMsg = ((int)MessageType.MoveInfo).ToString() + "_" + id + "_" + tmpArray[1];
                    //    //向所有客户端广播，玩家的移动信息
                    //    Broadcast(moveMsg);
                    //}
                    //同步
                    else if ((MessageType)(int.Parse(tmpArray[0])) == MessageType.AsynInfo)
                    {
                        //向所有客户端广播，玩家的同步消息，除了自己
                        Broadcast(mess, false);
                        revAsynInfo = true;
                        AsynInfoMess = mess;
                    }
                    else if ((MessageType)(int.Parse(tmpArray[0])) == MessageType.SkillInfo)
                    {
                        revSkillInfo = true;
                        SkillInfoMess = mess;
                    }
                }
            }

            data = new byte[256];
            //继续从客户端获取消息
            socket.BeginReceive(data, 0, data.Length, 0, new AsyncCallback(ReceiveCallback), socket);
        }


        //向客户端发送消息
        public void Send(string data)//"1_2_3243_dfads#1_234_dsf"
        {
            //加上一个结束符 
            data += "#";
            // Convert the string data to byte data using ASCII encoding.     
            byte[] byteData = Encoding.ASCII.GetBytes(data);
            // Begin sending the data to the remote device.     
            socket.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), socket);
        }

        //发送消息的回调函数,此函数在socket缓冲区中放入数据后调用,发送消息给客户端并不由此函数决定,而是由tcp协义决定
        void SendCallback(IAsyncResult ar)
        {
            Socket socket = (Socket)ar.AsyncState;
            // Complete sending the data to the remote device.     
            int bytesSent = socket.EndSend(ar);
            Console.WriteLine("发送 {" + bytesSent + "} 字节数据 到客户端");
        }

        // 向客户端广播消息，includeSelf是否包含自己
        public void Broadcast(string message, bool includeSelf = true)
        {
            foreach (ChatClient value in Server.players_obj.Values)
            {
                if (!includeSelf && this.id == value.id)
                    continue;

                //不同场景的玩家不接受广播
                if (value.roleInfo.sceneID != this.roleInfo.sceneID)
                    continue;

                ((ChatClient)value).Send(message);
            }
        }


        //解析包的函数
        void AnalyzePackage(string message)
        {
            messPackage += message;
            //#作为一条消息的结尾
            int index = -1;
            while ((index = messPackage.IndexOf("#")) != -1)
            {
                messPackageArray.Add(messPackage.Substring(0, index));
                messPackage = messPackage.Substring(index + 1);

            }
        }
    }
}
