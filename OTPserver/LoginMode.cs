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
        public LoginMode(Server.HandleClient server)
        {
            this.server = server;
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
    }
}
