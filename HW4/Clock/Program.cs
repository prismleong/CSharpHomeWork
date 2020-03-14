using System;
using System.Threading;

namespace Clock
{
    public delegate void NoticeHadler(int second);
    class Program
    {
        static void Main(string[] args)
        {
            Clock clock = new Clock();
            clock.Tick += tick;
            clock.Alarm += alarm;
            clock.Start();
        }
        static void tick(int second)
        {
            Console.Write($"Tick{second} ");
        }
        static void alarm(int second)
        {
            Console.WriteLine($"Alarm{second}");
        }
    }
    class Clock
    {
        public bool Running;
        public long StartTick;
        public event NoticeHadler Tick;
        public event NoticeHadler Alarm;
        public Clock()
        {
            Running = false;
        }
        public void Start()
        {
            StartTick = DateTime.Now.ToUniversalTime().Ticks;
            int SecondTick = 1;
            Running = true;
            Tick(SecondTick);
            while (Running)
            {
                Thread.Sleep(1000);
                StartTick = DateTime.Now.ToUniversalTime().Ticks;
                SecondTick += 1;
                Tick(SecondTick);
                if (SecondTick % 5 == 0)
                {
                    Alarm(SecondTick);
                }

            }
        }
    }
}
