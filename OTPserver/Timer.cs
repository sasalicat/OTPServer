using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTPserver
{
    public delegate bool NoArgB();
    
    public class Timer
    {
        public class delayExe
        {
            public double timeLeft;
            public NoArgB function;
            public delayExe(double second, NoArgB function)
            {
                timeLeft = second * 1000;//因為timeleft是
                this.function = function;
            }
        }
        public bool quit = false;
        private DateTime lastTime;

        public List<delayExe> handles;
        public Timer()
        {
            lastTime = (DateTime.Now);
        }
        public void clock()
        {
            while (!quit)
            {
                double time = (DateTime.Now - lastTime).TotalMilliseconds;
                foreach(delayExe e in handles)
                {
                    e.timeLeft -= time;
                    if (e.timeLeft <= 0)
                    {
                        if (e.function == null)
                        {
                            handles.Remove(e);
                        }
                        else if (e.function())
                        {
                            handles.Remove(e);
                        }
                    }
                }
            }
        }
    }
}
