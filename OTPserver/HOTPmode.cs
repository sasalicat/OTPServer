using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTPserver
{
    class HOTPmode : LoginMode
    {
        public HOTPmode(Server.HandleClient server, string account) : base(server, account)
        {
        }

        public override int no
        {
            get
            {
                return 1;
            }
        }

        public override int getKey()
        {
            int key = server.dbmodel.getKey(server.account);
            Console.Write("getkey 結束 key is:" + key);
            if (key < 0)//如果還沒有可以的話
            {
                TimeSpan ts = DateTime.Now - Server.origen;
                int second = (int)ts.TotalMilliseconds;
                key = new Random().Next(100000, 999999);
                server.dbmodel.addData(server.account, key,0);
            }
            return key;
        }

        public override bool HandlePassword(object pwd)
        {
            int time = server.dbmodel.getSecondFromOri(server.account);
            Console.WriteLine("in handlePassword time is:" + time);
            Console.WriteLine("ans is " + hashReduceNum.getNumber(getKey(), time) + " pwd is" + pwd);
            if ((int)pwd == hashReduceNum.getNumber(getKey(), time))
            {
                server.dbmodel.changeCounter(account,time + 1);
                return true;
            }
            return false;
        }
        public int getCount()
        {
            return server.dbmodel.getSecondFromOri(account);
        }
    }
}
