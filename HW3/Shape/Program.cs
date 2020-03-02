using System;

namespace Shape
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("输入一个形状数量：");
            int num = int.Parse(Console.ReadLine());
            float areaSum = 0;
            Shape[] shapeArray = ShapeFactory.CreateShapeArray(num,"rectangle");
            for(int i = 0; i < num; i++)
            {
                areaSum += shapeArray[i].Area;
            }
            Console.WriteLine(areaSum);
        }
    }
    abstract class Shape
    {
        public abstract float Area { get; }
        public abstract float Perimeter { get; }
        public abstract bool IsLegal();
    }
    class Rectangle : Shape
    {
        private float _width;
        public float Width
        {
            set
            {
                if (value > 0) _width = value;
                else _width = 1;
            }
            get => _width;
        }
        private float _height;
        public float Height
        {
            set
            {
                if (value > 0) _height = value;
                else _height = 1;
            }
            get=>_height;
        }
        override public float Area
        {
            get =>IsLegal() ? (Width + Height) * 2 : 0;
        }
        override public float Perimeter
        { 
            get=>IsLegal()?(Width + Height) * 2:0;
        }

        public Rectangle(float width, float height)
        {
            Width = width;
            Height = height;
        }

        override public bool IsLegal()
        {
            return Width > 0 && Height > 0;
        }
    }
    class Square : Rectangle
    {
        public Square(float side):base(side,side)
        {
            
        }
    }

    class Triangle: Shape
    {
        private float _side1;
        public float Side1
        {
            set
            {
                if (value > 0) _side1 = value;
                else _side1 = 1;
            }
            get => _side1;
        }
        private float _side2;
        public float Side2
        {
            set
            {
                if (value > 0) _side2 = value;
                else _side2 = 1;
            }
            get => _side2;
        }
        private float _side3;
        public float Side3
        {
            set
            {
                if (value > 0) _side3 = value;
                else _side3 = 1;
            }
            get => _side3;
        }
        override public float Area
        {
            get
            {
                if (IsLegal())
                {
                    float p = Perimeter / 2;
                    return (float)Math.Sqrt(p * (p - Side1) * (p - Side2) * (p - Side3));
                }
                else return 0;
            }
        }
        override public float Perimeter
        {
            get => this.IsLegal() ? (Side1 + Side2 + Side3) : 0;
        }
        public Triangle(float side1,float side2,float side3)
        {
            Side1 = side1;
            Side2 = side2;
            Side3 = side3;
        }
        override public bool IsLegal()
        {
            return (Side1>0&& Side2>0&& Side3 > 0)&&(Side1+ Side2>Side3) && (Side1 + Side3 > Side2) && (Side2 + Side3 > Side1);
        }
    }
    class ShapeFactory
    {
        public static Shape[] CreateShapeArray(int num,string type)
        {
            Shape[] arr = new Shape[num];
            Random rd = new Random();
            if (type == "rectangle") {
                for (int i = 0; i < num; i++)
                    arr[i] = new Rectangle(rd.Next(1,10), rd.Next(1, 10));
            }
            else if(type == "square")
            {
                for (int i = 0; i < num; i++)
                    arr[i] = new Square(rd.Next(1, 10));
            }
            return arr;
        }
    }
}
