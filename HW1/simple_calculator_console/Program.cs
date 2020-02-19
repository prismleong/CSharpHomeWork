using System;

namespace cal
{
    class int_calculator
    {
        public int[] input_arr;
        public string op;
        private int input_num;

        public int_calculator(int input_num)
        {
            this.input_num = input_num;
            input_arr = new int[input_num];
            op = "";
        }
        public bool input(String intput, int index)
        {
            bool error_flag = true;
            if (index <= input_num)
            {
                try
                {
                    input_arr[index - 1] = int.Parse(intput);
                    error_flag = false;
                }
                catch
                {
                    System.Console.WriteLine("请输入一个正整数");
                    error_flag = true;
                }
            }
            else System.Console.WriteLine("index 超出范围");
            return error_flag;
        }
        public bool input(String intput)
        {
            op = intput;
            bool error_flag = true;
            if (op == "+" || op == "-" || op == "*" || op == "/")
            {
                error_flag = false;
            }
            else
            {
                System.Console.WriteLine("请输入+-*/");
                error_flag = true;
            }
            return error_flag;
        }
        public float show_result()
        {
            int a = input_arr[0];
            int b = input_arr[1];
            float result = 0;
            if (op != "/" && b != 0)
            {
                System.Console.WriteLine("result:");
                if (op == "+")
                    result = a + b;
                else if (op == "-")
                    result = a - b;
                else if (op == "*")
                    result = a * b;
                else if (op == "/")
                    result = a / b;
                System.Console.WriteLine(result);
            }
            else System.Console.WriteLine("分母不能为0");
            return result;
        }
    }
        class Program
    {
        static void Main()
        {
            int_calculator cal = new int_calculator(2);
            System.Console.WriteLine("Input1:");
            while (cal.input(System.Console.ReadLine(), 1)) ;
            System.Console.WriteLine("Input2:");
            while (cal.input(System.Console.ReadLine(), 2)) ;
            System.Console.WriteLine("Op:");
            while (cal.input(System.Console.ReadLine())) ;
            cal.show_result();
        }
    }
}
