using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace SQLServer
{
    //消息号枚举
    enum MessageType
    {
        IdInfo = 0,
        Login,
        Register,
        CreateRole,
        CreateInfo,
        ChangeScene,
        TextInfo,
        MoveInfo,
        DestroyInfo,
        AsynInfo,
        SkillInfo,
        SkillEffect,
        SaveRoleInfo,
    }

    //服务器单例
    class Server
    {
        private int serverPort = 1000; //端口号
        private string serverIP = "192.168.124.19"; //服务器IP
        private Socket serverSocket = null; //服务器端socket
        public static Hashtable players_obj = new Hashtable(); //所有客户端列表

        public static ChatClient acceptUser = null;		//正在连接的客户端
        public static ChatClient destroyUser = null;   //要销毁的客户端
        //保持socket消息与主线程同步
        public ManualResetEvent allDone = new ManualResetEvent(false);//人工重置事件内核对象

        private static Server m_Instance;
        public static Server Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new Server();
                }
                return m_Instance;
            }
        }
        
        private Server()
        {

        }

        //启动服务器
        public void StartServer()
        {
            IPAddress ipAddress = IPAddress.Parse(serverIP);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, serverPort);

            // Create a TCP/IP socket.     创建TCP/IP面向连接的数据流的套接字，（Dgram，Udp用户数据报连接）
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(localEndPoint);//将套接字与IP和端口进行绑定
            serverSocket.Listen(100);//监听

            //异步函数，绑定一个回调函数，当有客户端请求连接时，就触发回调
            serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), serverSocket);

            Console.WriteLine("创建服务器成功,等待客户端连接。。。");
        }

        //接受客户端连接,每当一个新玩家成功连接时，在服务器上调用这个函数
        private void AcceptCallback(IAsyncResult ar)
        {
            //ar:异步结果，包含有服务器和客户端的套接字
            Socket server = (Socket)ar.AsyncState;
            Socket client = server.EndAccept(ar);

            client.NoDelay = true;//不延迟

            allDone.Reset();
            //创建一个新客户
            acceptUser = new ChatClient(client);
            //等待主线程执行
            allDone.WaitOne();

            //继续异步Accept
            server.BeginAccept(new AsyncCallback(AcceptCallback), server);
        }


        public void MainLoop()
        {
            while (true)
            {
                Update();
                Thread.Sleep(30);
            }
        }

        private void Update()
        {
            //创建预制对象只能在主线程中
            if (acceptUser != null)
            {
                PlayerConnected();
                acceptUser = null;
                allDone.Set();//重置了事件对象，其他线程才可以等到该对象
            }

            if (destroyUser != null)
            {
                PlayerDisconnected();
                destroyUser = null;
                allDone.Set();
            }
   
            //处理位置同步
            foreach (ChatClient player in players_obj.Values)
            {
                if (player.revAsynInfo && player.AsynInfoMess != null)
                {
                    string[] tmpArray = player.AsynInfoMess.Split('_');
                    if (player.id == int.Parse(tmpArray[1]))
                    {
                        Vector3 pos = new Vector3(float.Parse(tmpArray[2]), float.Parse(tmpArray[3]), float.Parse(tmpArray[4]));                        
                        Vector3 rot = new Vector3(float.Parse(tmpArray[5]), float.Parse(tmpArray[6]), float.Parse(tmpArray[7]));

                        player.roleInfo.position = pos;
                        player.roleInfo.rotation = rot;
                        player.revAsynInfo = false;
                        break;
                    }
                }
            }      
        }

        //每当一个新玩家成功连接时，在服务器上调用这个函数
        void PlayerConnected()
        {
            if (acceptUser == null)
                return;

            Console.WriteLine("连接的玩家id:" + acceptUser.id + "[Ip:" + acceptUser.ipAddress + "]");
            //Color color = GetRandColor(acceptUser.id);//根据玩家ID获取一个颜色

            //发送id号给新连接的客户端    0_1
            acceptUser.Send(((int)MessageType.IdInfo).ToString() + "_" + acceptUser.id.ToString());
            Thread.Sleep(100);           
        }

        //当一个玩家从服务器上断开时在服务器端调用
        void PlayerDisconnected()
        {
            Console.WriteLine("断开的玩家id:" + destroyUser.id + "[Ip:" + destroyUser.ipAddress + "]");

            //从容器中移除断开的玩家
            players_obj.Remove(destroyUser.id);
        }

        //创建角色模型
        public void CreateRoleModel(ChatClient user)
        {
            //消息类型+用户id+消息
            string message = ((int)MessageType.CreateInfo).ToString() + "_" + user.id.ToString() + "_" +
                user.roleInfo.position.x + "_" + user.roleInfo.position.y + "_" + user.roleInfo.position.z+ "_" +
                    "0_0_0" + "_" + user.roleInfo.color.r + "_" + user.roleInfo.color.g + "_" + user.roleInfo.color.b + 
                    "_" + user.roleInfo.name;
           
            //为所有玩家添加新加入的玩家模型，包含新加入的玩家
            user.Broadcast(message);
          
            
            //为新加入的玩家添加已经加入的其它玩家的模型
            foreach (DictionaryEntry elem in players_obj)
            {
                int otherPlayerID = (int)elem.Key;
                ChatClient otherPlayer = (ChatClient)elem.Value;

                if (otherPlayerID != user.id && otherPlayer.roleInfo.sceneID == user.roleInfo.sceneID)
                {
                    Vector3 pos = otherPlayer.roleInfo.position;
                    Vector3 rot = otherPlayer.roleInfo.rotation;
                    Color col = otherPlayer.roleInfo.color;
                    //消息类型+用户id+消息
                    string message1 = ((int)MessageType.CreateInfo).ToString() + "_" + otherPlayer.id.ToString() + "_" +
                            pos.x + "_" + pos.y + "_" + pos.z + "_" +
                            rot.x + "_" + rot.y + "_" + rot.z + "_" +
                            col.r + "_" + col.g + "_" + col.b + "_" + otherPlayer.roleInfo.name;
                    user.Send(message1);
                }
            }
        }


        //关闭服务器
        public void CloseServer()
        {
            foreach (ChatClient value in players_obj.Values)
            {
                //关闭客户端的连接
                ((ChatClient)value).socket.Close();
            }
            players_obj.Clear();

            //关闭服务器端
            if (serverSocket != null)
            {
                serverSocket.Close();
                serverSocket = null;
            }
        }
    }
}
