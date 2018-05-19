using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTPserver
{
    class TOTPmode : LoginMode
    {
        public const int WINDOW=20;//每window秒改變一次密碼

        public TOTPmode(Server.HandleClient server, string account) : base(server, account)
        {
        }

        public override int getKey()
        {
            int key = server.dbmodel.getKey(server.account);
            Console.Write("getkey 結束 key is:"+key);
            if (key<0)//如果還沒有可以的話
            {
                TimeSpan ts = DateTime.Now - Server.origen;
                int second = (int)ts.TotalMilliseconds;
                key = new Random().Next(100000, 999999);
                server.dbmodel.addData(server.account, key);
            }
            return key;
        }
        public override bool HandlePassword(object pwd)
        {
            int time = (DateTime.Now - Server.origen).Seconds;
            int ans = time / WINDOW;
            Console.Write("time:"+time+" ans:"+ans);
            Console.WriteLine("pwd is" + pwd + "key is" + hashReduceNum.getNumber(getKey(), ans));
            return (int)pwd == hashReduceNum.getNumber(getKey(), ans);
        }
        public int getSecondBetween()
        {
            return (DateTime.Now - Server.origen).Seconds;
        }
        public override int no
        {
            get
            {
                return 2;
            }
        }
    }
}
