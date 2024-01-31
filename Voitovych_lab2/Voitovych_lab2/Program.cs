using System;
using System.Net.NetworkInformation;

class Program
{
    static double x = 5, y = 5, z = 4;
    private static Semaphore s1 = new Semaphore(0, 1);
    private static Semaphore s2 = new Semaphore(0, 1);
    private static Semaphore s3 = new Semaphore(0, 1);
    private static Semaphore s4 = new Semaphore(0, 1);

    static void Main(string[] args)
    {
        Thread t1 = new Thread(T1);
        Thread t2 = new Thread(T2);
        Thread t3 = new Thread(T3);
        Thread t4 = new Thread(T4);
        Thread t5 = new Thread(T5);

        t1.Start();
        t2.Start();
        t3.Start();
        t4.Start();
        t5.Start();

        t1.Join();
        t2.Join();
        t3.Join();
        t4.Join();
        t5.Join();
    }

    static void T1()
    {
        x *= 5;
        Console.WriteLine($"T1 -> T3 func(x=x*5) x:{x}");
        s1.Release();
    }

    static void T2()
    {
        y += 2;
        Console.WriteLine($"T2 -> T3 func(y=y+2) y:{y}");
        s2.Release();
        
    }

    static void T3()
    {
        s1.WaitOne();
        //s2.WaitOne();
        y -= 3;
        Console.WriteLine($"T3 -> T4 func(y = y - 3) y:{y}");
        s3.Release();
        
    }

    static void T4()
    {
        s3.WaitOne();
        x += 2;
        Console.WriteLine($"T4 -> T5 func(x = x + 2) x:{x}");
        s4.Release();
        
    }
    static void T5()
    {
        s4.WaitOne();
        y = x * y;
        Console.WriteLine($"T5 func(y = x*y) y:{y}\nEnd");
    }
}