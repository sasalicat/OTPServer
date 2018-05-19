using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace OTPserver
{
    public abstract class  LoginMode
    {
        protected Server.HandleClient server=null;
        public string account;
        public LoginMode(Server.HandleClient server,string account)
        {
            this.server = server;
            this.account = account;
        }


        public abstract bool HandlePassword(object pwd);
        public abstract int getKey();
        public abstract int no
        {
            get;
        }
        public void onDuplication()
        {

        }
        public bool deleself()
        {
            Console.WriteLine("時間到了");
            if (Server.modeDict[account] == this)
            {
                Server.modeDict.Remove(account);
            }
            return true;
        }
    }
}
