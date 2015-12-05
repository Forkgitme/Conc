using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Security.Cryptography;

namespace IBANCalc
{
    class Threader
    {
        protected TaS TaSlock;
        protected Thread[] ts;

        public Threader()
        {
            ts = new Thread[Program.P];
            TaSlock = new TaS();
        }

        public virtual void makeThreads() { }

        public virtual void Start()
        {
            for (int t = 0; t < Program.P; t++)
                ts[t].Start(t);

            for (int t = 0; t < Program.P; t++)
                ts[t].Join();
        }
    }

    class hashThreader : Threader
    { 
        int result = -1;

        public hashThreader()
        {
        }

        public override void makeThreads()
        {
            for (int t = 0; t < Program.P; t++)
                ts[t] = new Thread(findHash);
        }

        public override void Start()
        {
            base.Start();

            Console.WriteLine(result);
        }

        public void findHash(object mt)
        {
            SHA1 sha = SHA1.Create();
            int from = Program.B + (int)mt * (Program.E - Program.B) / Program.P;
            int to = Program.B + ((int)mt + 1) * (Program.E - Program.B) / Program.P;
            //Console.WriteLine("Thread " + mt + " evaluates from " + from + " to " + to + " (total domain: " + (to - from) + ").");

            for (int bt = from; bt < to; bt++)
            {
                if (result != -1)
                    return;
                int b = bt;
                if (Test.mTest(b))
                {
                    byte[] hashArray = sha.ComputeHash(Encoding.ASCII.GetBytes(b.ToString()));
                    string newHash = "";
                    for (int hashPos = 0; hashPos < hashArray.Length; hashPos++)
                        newHash += hashArray[hashPos].ToString("x2");
                    if (newHash == Program.H)
                    {
                        result = b;
                    }
                }
            }

        }
    }

    class ListThreader : Threader
    {
        int count;

        public ListThreader()
        {
            count = 0;
        }

        public override void makeThreads()
        {
            for (int t = 0; t < Program.P; t++)
                ts[t] = new Thread(listNumbers);
        }

        public void listNumbers(object mt)
        {
            int from = Program.B + (int)mt * (Program.E - Program.B) / Program.P;
            int to = Program.B + ((int)mt + 1) * (Program.E - Program.B) / Program.P;
            //Console.WriteLine("Thread " + mt + " evaluates from " + from + " to " + to + " (total domain: " + (to - from) + ").");

            for (int bt = from; bt < to; bt++)
            {
                int b = bt;
                if (Test.mTest(b))
                {
                    if (Program.L == 1)
                    {
                        lock (this)
                        {
                            ++count;
                            Console.WriteLine(count + " " + b);
                        }
                    }
                    else
                    {
                        TaSlock.Lock();
                        ++count;
                        Console.WriteLine(count + " " + b);
                        TaSlock.Unlock();
                    }
                }
            }
        }
    }

    class CountThreader : Threader
    {
        int sum;
        public CountThreader()
        {
            sum = 0;
        }

        public override void makeThreads()
        {
            for (int t = 0; t < Program.P; t++)
                ts[t] = new Thread(countNumbers);
        }

        public override void Start()
        {
            base.Start();

            Console.WriteLine(sum);
        }

        public void countNumbers(object mt)
        {
            int from = Program.B + (int)mt * (Program.E - Program.B) / Program.P;
            int to = Program.B + ((int)mt + 1) * (Program.E - Program.B) / Program.P;
            //Console.WriteLine("Thread " + mt + " evaluates from " + from + " to " + to + " (total domain: " + (to - from) + ").");

            for(int bt = from; bt < to; bt++)
            {
                int b = bt;
                if (Test.mTest(b))
                {
                    if(Program.L == 1)
                    {
                        lock (this)
                        {
                            sum++;
                        }
                    }
                    else
                    {
                        TaSlock.Lock();
                        sum++;
                        TaSlock.Unlock();
                    }
                }
            }
        }
    }

}
