using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace OTPserver
{
    abstract class  LoginMode
    {
        Server.HandleClient server=null;
        public LoginMode(Server.HandleClient server)
        {
            this.server = server;
        }
        public abstract bool HandlePassword(object pwd);
    }
}
