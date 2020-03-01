using System; 


namespace PrimeFactors
{
    class Program
    {
        static void Main(string[] args)
        {
            int input;
            string result="";
            Console.WriteLine("请输入一个正整数：");
            try { 
                input=int.Parse(Console.ReadLine());
                while (input % 2 == 0)
                {
                    result += "2 ";
                    input /= 2;
                }
                int up = (int)Math.Sqrt(input * 1.0);
                for (int i = 3; i < up; i+=2)
                {
                    while (input % i == 0)
                    {
                        result += i + " ";
                        input /= i;
                    }
                }
                if (input > 2) result += input + "";
                Console.WriteLine(result);
            }
            catch
            {
                Console.WriteLine("输入有误，请输入一个正整数");
            }
            
        }
    }
}
