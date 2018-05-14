using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTPserver
{
    class OCRAmode : LoginMode//挑战应答模式
    {
        private int corretPwd = 0;

        public OCRAmode(Server.HandleClient server) : base(server)
        {
        }

        public int getPassWord()
        {
            
            int pwd = new Random().Next(100000, 999999);//产生一个随机数作为OTP
            corretPwd = pwd;
            return pwd;
            
        }
        public override bool HandlePassword(object pwd)
        {
            return corretPwd == (int)pwd;
        }
    }
}
