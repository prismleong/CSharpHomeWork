using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CayleyTree2
{
    public partial class Form1 : Form
    {
        Graphics graphics;
        double per1, per2, th1, th2;
        Pen color;
        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            int n = int.Parse(textBox1.Text);
            double len = double.Parse(textBox2.Text);
            per1 = double.Parse(textBox3.Text);
            per2 = double.Parse(textBox4.Text);
            th1 = double.Parse(textBox5.Text) * Math.PI / 180;
            th2 = double.Parse(textBox6.Text) * Math.PI / 180;
            color = color2pen(comboBox1.Text);
            if (graphics == null)
                graphics = this.CreateGraphics();
            graphics.Clear(Color.White);
            DrawCayleyTree(n, 250, 450, len, -Math.PI / 2);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int i = int.Parse(textBox1.Text);
                if (i < 0)
                    textBox1.Text = "10";
            }
            catch { textBox1.Text = "10"; }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            CheckDoubleInput(textBox2, "100");
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            CheckDoubleInput(textBox3, "0.8");
        }

        

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            CheckDoubleInput(textBox4, "0.66");
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            CheckDoubleInput(textBox5, "20");
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            CheckDoubleInput(textBox6, "30");
        }
        void CheckDoubleInput(TextBox tbox, string dft)
        {
            try
            {
                double i = double.Parse(tbox.Text);
                if (i < 0)
                    tbox.Text = dft;
            }
            catch { tbox.Text = dft; }
        }

        Pen color2pen(string str)
        {
            Pen result;
            if (str == "Blue")
                result = Pens.Blue;
            else if (str == "Yellow")
                result = Pens.Yellow;
            else if (str == "Red")
                result = Pens.Red;
            else if (str == "Green")
                result = Pens.Green;
            else
                result = Pens.Blue;
            return result;
        }

        public Form1()
        {
            InitializeComponent();
            String[] arr = new String[] { "Blue", "Yellow", "Red","Green" };
            for (int i = 0; i < arr.Length; i++)
            {
                comboBox1.Items.Add(arr[i]);
            }
        }
        void DrawCayleyTree(int n, double x0, double y0, double len, double theta)
        {
            if (n <= 0)
                return;
            double x1 = x0 + len * Math.Cos(theta);
            double y1 = y0 + len * Math.Sin(theta);
            graphics.DrawLine(color, (int)x0, (int)y0, (int)x1, (int)y1);
            DrawCayleyTree(n - 1, x1, y1, per1 * len, theta + th1);
            DrawCayleyTree(n - 1, x1, y1, per2 * len, theta - th2);
        }
    }
}

