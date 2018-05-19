using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace OTPserver
{

    public class Server
    {
        public static Dictionary<string, LoginMode> modeDict=new Dictionary<string, LoginMode>();
        public static Timer mainTimer = new Timer();
        public static readonly DateTime origen = new DateTime(2000, 1, 1);
        public class HandleClient
        {
            public const byte SET_MODE= 0;
            public const byte LOG_ACCOUNT = 1;
            public const byte GET_KEY = 2;
            public const byte LOGIN = 3;
            public const byte C_ERROR = 4;
            public const byte C_GET_KEY = 5;
            public const byte C_LOGIN_RESPONSE = 6;
            public const byte PROOFREAD = 7;
            public const byte C_PRO_RES = 8;
            public const byte REQ_COUNT = 9;
            public const byte C_COUNT_RES = 10;
            //错误编号------------------------
            public const int E_CANT_GET_KEY=0;
            public const int E_CANT_FIND_ACCOUNT_HAS_LOG=1;
            /// <summary>
            /// private attribute of HandleClient class
            /// </summary>
            private TcpClient mTcpClient;
            public DataBaseUnit dbmodel;
            private PacketSlice pslicer;
            //private LoginMode mode;
            public string account;
            public bool shutdown = false;
            public Timer timer;
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="_tmpTcpClient">傳入TcpClient參數</param>
            public HandleClient(PacketSlice slice, TcpClient _tmpTcpClient,Timer timer)
            {
                this.dbmodel = new DataBaseUnit();
                this.mTcpClient = _tmpTcpClient;
                this.pslicer = slice;
                this.timer = timer;
            }

            /// <summary>
            /// Communicate
            /// </summary>
            public void Communicate()
            {
                /*try
                {
                    CommunicationBase cb = new CommunicationBase();
                    string msg = cb.ReceiveMsg(this.mTcpClient);
                    Console.WriteLine(msg + "\n");
                    cb.SendMsg("主機回傳測試", this.mTcpClient);
                }
                catch
                {
                    Console.WriteLine("客戶端強制關閉連線!");
                    this.mTcpClient.Close();
                    Console.Read();
                }*/
                try
                {
                    //CommunicationBase cb = new CommunicationBase();
                    //string msg = cb.ReceiveMsg(this.mTcpClient);
                    //Console.WriteLine(dbmodel.getSecondFromOri("asswecan"));
                    Console.WriteLine("等待命令中");
                    TransModule postman = new TransModule(mTcpClient, pslicer);
                    while (!shutdown) { 
                        List<PacketSlice.Order> orders = postman.waitForOrder();
                        if (orders == null)//终止线程
                        {
                            return;
                        }
                        Console.WriteLine("来自客户端 "+mTcpClient+"的命令:"+orders.Count);
                        while(orders.Count>0)
                        {
                            PacketSlice.Order o = orders[0];
                            Console.WriteLine(o.order + ":" + o.data + "\n");
                            switch (orders[0].order) {
                                case LOG_ACCOUNT:
                                    {
                                        account = (string)orders[0].data;
                                        break;
                                    }
                                case SET_MODE:
                                    {
                                        if (modeDict.ContainsKey(account)&& modeDict[account].no == (int)orders[0].data)
                                        {

                                            modeDict[account].onDuplication();

                                        }
                                        else
                                        {
                                            switch ((int)orders[0].data)
                                            {
                                                case 0:
                                                    {//对应帐号创建login接收者
                                                        Server.modeDict[account] = new OCRAmode(this,account);
                                                        break;
                                                    }
                                                case 1:
                                                    {
                                                        Server.modeDict[account] = new HOTPmode(this, account);
                                                        break;
                                                    }
                                                case 2:
                                                    {
                                                        Server.modeDict[account] = new TOTPmode(this,account);
                                                        break;
                                                    }
                                            }
                                        }
                                        break;
                                    }
                                case GET_KEY:
                                    {
                                        int key = modeDict[account].getKey();
                                        if (key < 0)
                                        {//0为client端的error
                                            postman.sendOrder(C_ERROR, E_CANT_GET_KEY);
                                        }
                                        else
                                        {//1为client端的getKey
                                            postman.sendOrder(C_GET_KEY, key);
                                        }
                                        break;
                                    }
                                case LOGIN:
                                    {
                                        if (!modeDict.ContainsKey(account))
                                        {
                                            postman.sendOrder(C_ERROR,E_CANT_FIND_ACCOUNT_HAS_LOG);
                                        }
                                        if (modeDict[account].HandlePassword(orders[0].data))
                                        {//2为client端的loginResponse
                                            postman.sendOrder(C_LOGIN_RESPONSE, 1);
                                        }
                                        else
                                        {
                                            postman.sendOrder(C_LOGIN_RESPONSE, -1);
                                        }
                                        break;
                                    }
                                case PROOFREAD:
                                    {
                                        postman.sendOrder(C_PRO_RES, ((TOTPmode)modeDict[account]).getSecondBetween());

                                        break;
                                    }
                                case REQ_COUNT:
                                    {
                                        postman.sendOrder(C_COUNT_RES, ((HOTPmode)modeDict[account]).getCount());
                                        break;
                                    }

                            }
                            orders.RemoveAt(0);

                        }
                    }
                    //cb.SendMsg("主機回傳測試", this.mTcpClient);
                }
                catch(Exception e)
                {
                    Console.WriteLine("客戶端強制關閉連線!:"+e);
                    this.mTcpClient.Close();
                    Console.Read();
                }
            } // end HandleClient()
        } // end Class
          /// <summary>
          /// 等待客戶端連線
          /// </summary>
        public void ListenToConnection()
        {
            DataBaseUnit dataBaseModel = new DataBaseUnit();
            PacketSlice packetSlicer = new PacketSlice();
            packetSlicer.useDefaultDict();
            
            //取得本機名稱
            string hostName = Dns.GetHostName();
            Console.WriteLine("本機名稱=" + hostName);

            //取得本機IP
            //IPAddress[] ipa = Dns.GetHostAddresses(hostName);
            //Console.WriteLine("本機IP=" + ipa[0].ToString());
            IPAddress ip = new IPAddress(new byte[] {127,0,0,1});

            //建立本機端的IPEndPoint物件
            IPEndPoint ipe = new IPEndPoint(ip, 1234);

            //建立TcpListener物件
            TcpListener tcpListener = new TcpListener(ipe);

            //開始監聽port
            tcpListener.Start();
            Console.WriteLine("等待客戶端連線中... \n");

            TcpClient tmpTcpClient;
            int numberOfClients = 0;
            while (true)
            {
                try
                {
                    //建立與客戶端的連線
                    tmpTcpClient = tcpListener.AcceptTcpClient();

                    if (tmpTcpClient.Connected)
                    {
                        Console.WriteLine("連線成功!");
                        HandleClient handleClient = new HandleClient(packetSlicer,tmpTcpClient, mainTimer);
                        Thread myThread = new Thread(new ThreadStart(handleClient.Communicate));
                        numberOfClients += 1;
                        myThread.IsBackground = true;
                        myThread.Start();
                        myThread.Name = tmpTcpClient.Client.RemoteEndPoint.ToString();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.Read();
                }
            } // end while
        } // end ListenToConnect()
    } // end class

} // end namespace
