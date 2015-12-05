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
        public void hashing()
        {
            SHA1 sha = SHA1.Create();
            Console.WriteLine("Geef de SHA1 hash van een getal van 0 t/m 10, \nwaarbij het getal als string is gehashed.");
            string hash = Console.ReadLine();
            for (int i = 0; i < 11; i++)
            {
                byte[] hashArray = sha.ComputeHash(Encoding.ASCII.GetBytes(i.ToString()));
                string newHash = "";
                for (int hashPos = 0; hashPos < hashArray.Length; hashPos++)
                    newHash += hashArray[hashPos].ToString("x2");
                Console.WriteLine("De hash van {0} is {1}.", i, newHash);
                if (newHash == hash)
                {
                    Console.WriteLine("Je zocht het getal " + i);
                    Console.ReadLine();
                    return;
                }
            }
            Console.WriteLine("Getal niet gevonden.");
            Console.ReadLine();
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
