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
        public class HandleClient
        {
            public const byte SET_MODE= 0;

            /// <summary>
            /// private attribute of HandleClient class
            /// </summary>
            private TcpClient mTcpClient;
            private DataBaseUnit dbmodel;
            private PacketSlice pslicer;
            private LoginMode mode;
            public bool shutdown = false;
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="_tmpTcpClient">傳入TcpClient參數</param>
            public HandleClient(DataBaseUnit db,PacketSlice slice, TcpClient _tmpTcpClient)
            {
                this.dbmodel = db;
                this.mTcpClient = _tmpTcpClient;
                this.pslicer = slice;
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
                    Console.WriteLine("等待命令中");
                    TransModule postman = new TransModule(mTcpClient, pslicer);
                    while (!shutdown) { 
                    List<PacketSlice.Order> orders = postman.waitForOrder();
                    Console.WriteLine("来自客户端 "+mTcpClient+"的命令:");
                        foreach (PacketSlice.Order o in orders)
                        {
                            Console.WriteLine(o.order + ":" + o.data + "\n");
                            switch (orders[0].order) {
                                case SET_MODE:
                                    {
                                        switch ((int)orders[0].data)
                                        {
                                            case 0:{
                                                    mode = new OCRAmode(this.);
                                                    break;
                                                }


                                        }
                                        break;
                                    }
                            }

                        }
                    }
                    //cb.SendMsg("主機回傳測試", this.mTcpClient);
                }
                catch
                {
                    Console.WriteLine("客戶端強制關閉連線!");
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
                        HandleClient handleClient = new HandleClient(dataBaseModel,packetSlicer,tmpTcpClient);
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
