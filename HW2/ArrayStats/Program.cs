using System;

namespace ArrayStats
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] input;
            Console.WriteLine("请输入一个整数数组，空格间隔：");
            input = Console.ReadLine().Split(" ");
            int len = input.Length,max=0,min=0,sum=0;
            float avg=0;
            if (len == 0)
            {
                Console.WriteLine("数组长度需大于0");
                return;
            }
            for(int i = 0;i< len; i++)
            {
                int cur = 0;
                try { cur = int.Parse(input[i]); }
                catch { }
                if (i == 0)
                {
                    max = min = cur;
                }
                else
                {
                    if (max < cur) max = cur;
                    if (min > cur) min = cur;
                }
                sum += cur;
            }
            avg = sum / len;
            Console.WriteLine("最大值："+max+"，最小值：" + min + "，平均值：" + avg + "，和："+sum);
        }
    }
}
