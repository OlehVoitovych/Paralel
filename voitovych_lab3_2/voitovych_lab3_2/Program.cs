using System.Diagnostics;
using ScottPlot;
//dotnet add package ScottPlot --version 4.1.67

class Task
{
    public List<int> row1;
    public List<int> row2;
    public List<List<int>> toMult;
    public char oper;
    public int rowIndex;
    public char toSave;
    public bool Mult = false;
    public Task(List<int> row1, List<int> row2, char oper, int rowIndex, char toSave)
    {
        this.row1 = row1;
        this.row2 = row2;
        this.oper = oper;
        this.rowIndex = rowIndex;
        this.toSave = toSave;
    }

    public Task(List<int> row1, List<List<int>> toMult, char oper, int rowIndex, char toSave)
    {
        this.Mult = true;
        this.row1 = row1;
        this.toMult = toMult;
        this.oper = oper;
        this.rowIndex = rowIndex;
        this.toSave = toSave;
    }
}

class ProducerConsumerQueue : IDisposable
{
    EventWaitHandle wh = new AutoResetEvent(false);
    List<Thread> workers = new List<Thread>();
    object locker = new object();
    static Queue<Task> tasks = new Queue<Task>();
    public bool done;

    public ProducerConsumerQueue(int P)
    {
        done = false;
        for (int i = 0; i < P; i++)
        {
            workers.Add(new Thread(Work));
        }
        foreach (Thread t in workers)
        {
            t.Start();
        }
        //worker = new Thread(Work);
        //worker.Start();
    }

    public void EnqueueTask(Task task)
    {
        lock (locker)
            tasks.Enqueue(task);
        wh.Set();
    }

    public void Dispose()
    {
        EnqueueTask(null); // Сигнал Споживачу про завершення
        while (!workers.All(x => !x.IsAlive))
        {
            wh.Set();
        } // Очікування завершення
        wh.Close(); // Звільнення ресурсів
    }

    void Work()
    {
        while (true)
        {
            Task task = null;
            lock (locker)
            {
                if (tasks.Count > 0)
                {
                    task = tasks.Dequeue();
                    if (task == null)
                        return;
                }
            }
            if (task != null)
            {
                if (task.oper == '+')
                {
                    if (task.toSave == 'A') { lock (locker) { Test.A[task.rowIndex] = rowadd(task.row1, task.row2); } }
                    if (task.toSave == 'B') { lock (locker) { Test.B[task.rowIndex] = rowadd(task.row1, task.row2); } }
                    if (task.toSave == 'C') { lock (locker) { Test.C[task.rowIndex] = rowadd(task.row1, task.row2); } }
                }

                else if (task.oper == '*' && task.Mult)
                {

                    if (task.toSave == 'A') { lock (locker) { Test.A[task.rowIndex] = mult(task.row1, task.toMult); } }
                    if (task.toSave == 'B') { lock (locker) { Test.B[task.rowIndex] = mult(task.row1, task.toMult); } }
                    if (task.toSave == 'C') { lock (locker) { Test.C[task.rowIndex] = mult(task.row1, task.toMult); } }
                }
            }
            else
                lock (locker)
                {
                    if (done)
                        return;
                }
            wh.WaitOne(); // Більше задач немає, очікуємо сигнал...
        }
    }

    List<int> rowadd(List<int> a, List<int> b)
    {
        List<int> res = new List<int>();
        for (int i = 0; i < a.Count; i++) { res.Add(a[i] + b[i]); }
        return res;
    }

    List<int> mult(List<int> a, List<List<int>> b)
    {
        List<int> res = new List<int>();
        for (int i = 0; i < a.Count; i++)
        {
            int sum = 0;
            List<int> temp = new List<int>();
            for (int j = 0; j < a.Count; j++)
            {
                sum += a[j] * b[j][i];

            }
            res.Add(sum);
        }
        return res;
    }

    void prt(List<int> x)
    {
        foreach (int i in x) { Console.Write($"{i}; "); }
        Console.WriteLine();
    }
}

class Test
{
    private static List<List<int>> m1, m2, m3, m4, m5, m6;
    private static List<int> resolutions = new List<int>();
    public static List<List<int>> A, B, C;

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("Enter P: ");
            int P = cpu(Convert.ToInt32(Console.ReadLine()));
            if (P == 0)
            {
                Environment.Exit(0);
            }

            for (int i = 0; i < 10; i++)
            {
                resolutions.Add(100 + i);
            }

            List<long> elapsed = new List<long>();

            int counter = 0;
            foreach (int n in resolutions)
            {
                counter++;
                m1 = matrixFill(n);
                m2 = matrixFill(n);
                m3 = matrixFill(n);
                m4 = matrixFill(n);
                m5 = matrixFill(n);
                m6 = matrixFill(n);
                A = matrixFillempty(n);
                B = matrixFillempty(n);
                C = matrixFillempty(n);


                Stopwatch sw = new Stopwatch();

                sw.Start();
                using (ProducerConsumerQueue q = new ProducerConsumerQueue(P))
                {
                    for (int i = 0; i < n; i++)
                    {
                        q.EnqueueTask(new Task(m1[i], m2[i], '+', i, 'A'));
                    }

                    for (int i = 0; i < n; i++)
                    {
                        q.EnqueueTask(new Task(m3[i], m4[i], '+', i, 'B'));
                    }

                    for (int i = 0; i < n; i++)
                    {
                        q.EnqueueTask(new Task(m5[i], m6[i], '+', i, 'C'));
                    }

                    for (int i = 0; i < n; i++)
                    {
                        q.EnqueueTask(new Task(A[i], B, '*', i, 'A'));
                    }

                    for (int i = 0; i < n; i++)
                    {
                        q.EnqueueTask(new Task(A[i], C, '*', i, 'A'));
                    }

                    q.done = true;


                }

                sw.Stop();
                elapsed.Add(sw.ElapsedMilliseconds);
                Console.Write($"\rDone {counter} out of {resolutions.Count}");


            }
            Console.WriteLine();

            for (int i = 0; i < elapsed.Count; i++)
            {
                Console.WriteLine($"Elapsed time for N={resolutions[i]} is {elapsed[i]}ms.");
            }
            /*double[] x = new double[resolutions.Count];
            double[] y = new double[resolutions.Count];
            for (int i = 0; i < resolutions.Count; i++)
            {
                x[i] = (Convert.ToDouble(resolutions[i]));
                y[i] = (Convert.ToDouble(elapsed[i]));
            }
            
            var plt = new Plot(400,400);
            plt.AddScatter(x, y);
            string filepath = "~/statistics/graph100.png";
            plt.SaveFig(filepath);
            Console.WriteLine($"Saved to {filepath}");
            Console.ReadKey();*/
            Poly p = new Poly(resolutions, elapsed);
        }


    }

    static int cpu(int p)
    {
        if (false)
        {
            Console.WriteLine($"CPU is {Environment.ProcessorCount} core");
            return Environment.ProcessorCount;
        }
        else
        {
            return p;
        }
    }

    static List<List<int>> matrixFill(int size)
    {
        List<List<int>> matrix = new List<List<int>>();
        Random random = new Random();
        for (int i = 0; i < size; i++)
        {
            List<int> temp = new List<int>();
            for (int j = 0; j < size; j++)
            {
                temp.Add(Convert.ToInt32(random.Next(-10, 11)));
            }

            matrix.Add(temp);
        }

        return matrix;
    }

    static List<List<int>> matrixFillempty(int size)
    {
        List<List<int>> matrix = new List<List<int>>();
        for (int i = 0; i < size; i++)
        {
            List<int> temp = new List<int>();
            for (int j = 0; j < size; j++)
            {
                temp.Add(0);
            }

            matrix.Add(temp);
        }

        return matrix;
    }

}

class Poly
{
        public Poly( List<int> a, List<long> b)
        {
            double[] xs = new double[a.Count];
            double[] ys = new double[a.Count];
            for (int i = 0; i < a.Count; i++)
            {
                xs[i] = (Convert.ToDouble(a[i]));
                ys[i] = (Convert.ToDouble(b[i]));
            }
            var plt = new Plot();
            plt.AddScatter(xs, ys);
            plt.SaveFig(@"C:/Users/Oleh/Documents/GitHub/2023/voitovych_lab3_2/voitovych_lab3_2/quickstart.png");
        //Console.WriteLine($"Saved to {filepath}");
        //Console.ReadKey();
    }

}

