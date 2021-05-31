using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Dining_Philosophers_Problem
{
    public class Philosoph
    {
        public static int[] ForkFlags = new int[5];// 1 - вилка занята
        private static object[] fork = new object[5];


        public int PhNumber;
        public int NumberLeftFork;
        public int NumberRightFork;

        static Philosoph()
        {
            for (int i = 0; i < 5; i++)
                fork[i] = new();
        }

        public Philosoph(int number)
        {
            this.PhNumber = number;
            NumberLeftFork = PhNumber >= 5 - 1 ? 0 : PhNumber;
            NumberRightFork = PhNumber <= 0 ? 5 - 1 : PhNumber - 1;
        }

        public void Thinking()
        {
            Random rnd = new Random();
            Thread.Sleep(rnd.Next(5000));
        }

        public  void TakeLeftFork()
        {
            lock (fork[NumberLeftFork])
            {
                Interlocked.CompareExchange(ref ForkFlags[NumberLeftFork], 1, 0);
                Thread.Sleep(500);
                Console.WriteLine($"Философ {PhNumber} взял левую вилку {NumberLeftFork}");
            }
        }

        public void TakeRightFork()
        {
            lock (fork[NumberRightFork])
            {
                Interlocked.CompareExchange(ref ForkFlags[NumberRightFork], 1, 0);
                Thread.Sleep(500);
                Console.WriteLine($"Философ {PhNumber} взял правую вилку {NumberRightFork}");
            }
        }

        public void PutLeftFork()
        {
            lock (fork[NumberLeftFork])
            {
                Interlocked.CompareExchange(ref ForkFlags[NumberLeftFork], 0, 1);
                Thread.Sleep(500);
                Console.WriteLine($"Философ {PhNumber} положил левую вилку {NumberLeftFork}");
            }
        }

        public void PutRightFork()
        {
            lock (fork[NumberRightFork])
            {
                Interlocked.CompareExchange(ref ForkFlags[NumberRightFork], 0, 1);
                Console.WriteLine($"Философ {PhNumber} положил правую вилку {NumberRightFork}");
                Thread.Sleep(500);
            }
        }

        public void Eat()
        {
            lock (fork[NumberLeftFork])
            {
                lock (fork[NumberRightFork])
                {
                        Console.WriteLine($"Философ {PhNumber} обедает");
                        Thread.Sleep(500);
                        PutLeftFork();
                        PutRightFork();
                }
            }
            Console.WriteLine($"Философ {PhNumber} закончил обед");
        }


        public void Algorythm()
        {
            if (ForkFlags[NumberLeftFork] == 0)
            {
                TakeLeftFork();
            }

            if (ForkFlags[NumberRightFork] == 0)
            {
                TakeRightFork();
                Eat();
                Thinking();
            }
            else
            {
                PutLeftFork();
            }

        }

    }


    class Program
    {

        static void Main(string[] args)
        {

            List<Philosoph> Philosophers = new List<Philosoph>();

            for(int i=0 ; i < 5; i++)
            {
                Philosophers.Add(new Philosoph(i));
            }

            Task[] philosophs = new Task[5];
            while (true)
            {
                for (int i = 0; i < 5; i++)
                {
                    Thread.Sleep(500);
                        philosophs[i] = Task.Run(() => Philosophers[i].Algorythm());
                }
            }
        }
    }

 
}