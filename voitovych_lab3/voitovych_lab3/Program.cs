namespace Program
{
    class Test
    {
        //(x1 + x2)(x3 + x4) + x5x6
        //x1 = 1, x2 = 2, x3 = 3, x4 = 4, x5 = 5, x6 = 6.

        static EventWaitHandle wh1 = new AutoResetEvent(false),
         wh2 = new AutoResetEvent(false),
        wh3 = new AutoResetEvent(false);

        static private int x1, x2, x3, x4,x5,x6;
        static int A, B, C;

        static void Main(string[] args)
        {
            var T0 = new Thread(t1);
            var T1 = new Thread(t2);
            var T2 = new Thread(t3);
            var T3 = new Thread(t4);
            var T4 = new Thread(t5);
            T0.Start();
            T1.Start();
            T2.Start();
            T3.Start();
            T4.Start();
            T4.Join();
            Console.ReadKey();
        }

        static void t1()
        {
            x1 = 1;
            x2 = 2;
            A = x1 + x2;
            wh1.Set();

        }
        static void t2()
        {
            x3 = 3;
            x4 = 4;
            B = x3 + x4;
            wh2.Set();

        }
        static void t3()
        {
            x5 = 5;
            x6 = 6;
            C = x5 * x6;
            wh3.Set();
        }

        static void t4()
        {
            wh1.WaitOne();
            wh2.WaitOne();
            wh1.Set();
            Console.WriteLine("After wh1 1");
            wh1.WaitOne();
            Console.WriteLine("After wh1 2");
            A *= B;
        }
        static void t5()
        {
            wh3.WaitOne();
            Console.WriteLine($"result: {A + C}");
        }
    }
}