using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    static Queue<int> buffer = new Queue<int>();
    static object bufferLock = new object();

    static void Main(string[] args)
    {
        Thread producerThread = new Thread(Producer);
        Thread consumerThread = new Thread(Consumer);

        producerThread.Start();
        consumerThread.Start();

        producerThread.Join();
        consumerThread.Join();
    }

    static void Producer()
    {
        Random random = new Random();

        while (true)
        {
            int number = random.Next(100);
            lock (bufferLock)
            {
                buffer.Enqueue(number);
                Console.WriteLine($"Producer produced: {number}");
            }

            Thread.Sleep(random.Next(1000));
        }
    }

    static void Consumer()
    {
        while (true)
        {
            int number;
            lock (bufferLock)
            {
                if (buffer.Count == 0)
                {
                    continue;
                }

                number = buffer.Dequeue();
            }

            Console.WriteLine($"Consumer consumed: {number}");
            Thread.Sleep(500);
        }
    }
}

