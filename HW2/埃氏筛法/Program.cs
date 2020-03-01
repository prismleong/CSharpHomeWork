using System;

namespace 埃氏筛法
{
    class Program
    {
        static void Main(string[] args)
        {
            int up = 100;
            bool[] isprime;
            Console.WriteLine("请输入素数范围：");
            try { up = int.Parse(Console.ReadLine()); }
            catch { Console.WriteLine("请输入一个正整数"); }

            isprime = new bool[up+1];
            for (int i = 0; i < isprime.Length; i++) isprime[i] = true;
            isprime[0] = isprime[1] = false;

            for(int i = 2; i*i<=up; i++)
            {
                if (isprime[i])
                {
                    for (int j = i * 2; j <= up; j += i) isprime[j] = false;
                }
            }
            for (int i = 0; i < isprime.Length; i++)
            {
                if (isprime[i])
                    Console.Write(i+" ");
                if(i%10==0) Console.Write('\n');
            }
        }
    }
}
