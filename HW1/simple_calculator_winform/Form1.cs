using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox1.Items.Add("+");
            comboBox1.Items.Add("-");
            comboBox1.Items.Add("*");
            comboBox1.Items.Add("/");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int_calculator cal = new int_calculator(2);
            cal.input(textBox1.Text, 1);
            cal.input(textBox2.Text, 2);
            cal.input(comboBox1.Text);
            label1.Text = cal.show_result();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
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
        public string show_result()
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
            else { 
                System.Console.WriteLine("分母不能为0");
                return "inf";
            } 
            return ""+result;
        }
    }
}
