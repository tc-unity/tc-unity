using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServer
{
    class Program
    {
        static void Main(string[] args)
        {
            DBManager.Instance.InitDB();
            Server.Instance.StartServer();

            Server.Instance.MainLoop();

            DBManager.Instance.CloseDB();
            Server.Instance.CloseServer();
        }
    }
}
