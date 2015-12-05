using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBANCalc
{
    class Program
    {
        static int l, b, e, m, p, u;
        static string h;

        static void Main(string[] args)
        {
            string i = Console.ReadLine();

            string[] input = i.Split(' ');

            Program.L = int.Parse(input[0]);
            Program.B = int.Parse(input[1]);
            Program.E = int.Parse(input[2]);
            Program.M = int.Parse(input[3]);
            Program.P = int.Parse(input[4]);
            Program.U = int.Parse(input[5]);
            Program.H = input[6];

            Threader t = new Threader();

            switch (U)
            {
                default:
                case 0:
                    t = new CountThreader();
                    break;
                case 1:
                    t = new ListThreader();
                    break;
                case 2:
                    t = new hashThreader();
                    break;
            }

            t.makeThreads();
            t.Start();

            Console.ReadLine();
        }

        //Program Parameters
        #region 
        public static int L
        {
            get { return l; }
            set
            {
                if (value == 0 || value == 1)
                    l = value;
            }
        }

        public static int B
        {
            get { return b; }
            set
            {
                if (value > 0 && value < int.MaxValue)
                    b = value;
            }
        }

        public static int E
        {
            get { return e; }
            set
            {
                if (value > B && value <= int.MaxValue)
                    e = value;
            }
        }

        public static int M
        {
            get { return m; }
            set
            {
                if (value >= 1 && value <= 256)
                    m = value;
            }
        }

        public static int P
        {
            get { return p; }
            set
            {
                if (value >= 1 && value <= 256)
                    p = value;
            }
        }

        public static int U
        {
            get { return u; }
            set
            {
                if (value == 1 || value == 2 || value == 3)
                    u = value;
            }
        }

        public static string H
        {
            get { return h; }
            set { h = value; }
        }
        #endregion 
    }
}
