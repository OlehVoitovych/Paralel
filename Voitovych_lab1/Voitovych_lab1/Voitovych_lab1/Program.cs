namespace Program
{
    class MainProgram
    {

        public static void Main(string[] args)
        {
            /*console.writeline("enter matrix resolution:");
            r = convert.toint32(console.readline());
            matrix = new int[r, r];
            thread t1 = new thread(matrixgener);
            t1.start();*/

            while (true)
            {
                Console.WriteLine("1. Знайти суму елементів матриці, як суму елементів верхньої трикутної та нижньої трикутною підматриць.  Матриця задається рандомно. Розмірність матриці вводиться з консолі\n" +
                                  "2. Знайти попарний скалярний добуток рядків прямокутної матриці.Кількість рядків матриці є парною.Матриця задається рандомно.Розмірність матриці вводиться з консолі\n" +
                                  "3. Знайти суму цілих елементів проміжку[0, n]  як суму усіх парних і непарних елементів.Значення n вводиться з консолі\n" +
                                  "4. Згенерувати множину із n дійсних чисел.Знати добуток перших n / 2 і суму решти елементів множини.Значення n є парним і вводиться з консолі\n" +
                                  "5. Знайти суму норм векторів, що утворюються із головної та побічної діагоналей квадратної матриці.Матриця задається рандомно.Розмірність матриці вводиться з консолі\n" +
                                  "6. Знайти суму найменших елементів стовпців матриці.Матриця задається рандомно.Розмірність матриці вводиться з консолі\n" +
                                  "7. Знайти середнє найбільших елементів рядків матриці.Матриця задається рандомно.Розмірність матриці вводиться з консолі\n0. Вихід\n Введіть номер:");

                string input = Console.ReadLine();
                if (input == "1") { new TriangleSum(); }
                else if (input == "2") { new Scalar(); }
                else if (input == "3") { new IntSum(); }
                else if (input == "4") { new SetSum(); }
                else if (input == "5") { new NormVector(); }
                else if (input == "6") { new SumColm(); }
                else if (input == "7") { new AvrgRow(); }
                else if (input == "0") { Environment.Exit(0); }
            }
        }

    }

    class TriangleSum
    {

        private static int upSum;
        private static int downSum;
        private static int a;
        private static int[,] matrix;

        public TriangleSum()
        {
            Console.WriteLine("Введіть ромір матриці: ");
            a = Convert.ToInt32(Console.ReadLine());

            matrix = new int[a, a];

            MatrixGener();
            Thread triangleSumUp = new Thread(TriangleSumUp);
            triangleSumUp.Start();

            Thread triangleSumDown = new Thread(TriangleSumDown);
            triangleSumDown.Start();

            Thread sum = new Thread(SumTriangles);
            sum.Priority = ThreadPriority.Lowest;
            sum.Start();

        }

        static void MatrixGener()
        {
            Random rnd = new();

            for (int i = 0; i < a; i++)
            {

                for (int j = 0; j < a; j++)
                {
                    matrix[i, j] = Convert.ToInt32(rnd.Next(1, 100));
                }
            }

            for (int i = 0; i < a; i++)
            {
                for (int j = 0; j < a; j++)
                {
                    Console.Write(matrix[i, j] + " ");
                }
                Console.WriteLine();
            }

        }


        static void TriangleSumUp()
        {
            int count = 0;
            while (count != a)
            {

                for (int i = count; i < a; i++)
                {
                    upSum += matrix[count, i];

                }
                count++;
            }

            Console.WriteLine("сума елементів верхньої трикутної підматриці = " + upSum);
        }

        static void TriangleSumDown()
        {
            int count = 1;
            int count_ = 0;
            while (count != a)
            {
                for (int i = 0; i <= count_; i++)
                {
                    downSum += matrix[count, i];

                }
                count++;
                count_++;
            }

            Console.WriteLine("сума елементів нижньої трикутної підматриці = " + downSum);
        }

        static void SumTriangles()
        {
            int sumt = downSum + upSum;
            Console.WriteLine("сума елементів нижньої та верхної трикутної підматриці = " + sumt);
        }
    }

    class Scalar
    {
        private static int m;
        private static int n;
        private static int[,] matrix;
        private static List<List<int>> rowScalar;

        public Scalar()
        {
            Console.WriteLine("Введіть ромір m(парне, у випадку непарного розмір + 1) матриці: ");
            m = ResPare(Convert.ToInt32(Console.ReadLine()));
            Console.WriteLine("Введіть ромір n матриці: ");
            n = Convert.ToInt32(Console.ReadLine());


            matrix = new int[m, n];
            rowScalar = new List<List<int>>();
            MatrixGener();


            Thread Scal_ = new Thread(Scal);
            Scal_.Start();


            Task.Run(() =>
            {
                Scal_.Join();
                printRes();
            });



        }

        static int ResPare(int n)
        {
            switch (n % 2 == 0)
            {
                case true: return n;
                case false: return n + 1;
            }
        }

        private static void printRes()
        {
            Console.WriteLine("===============================");
            foreach (List<int> l in rowScalar)
            {
                foreach (int i in l)
                {
                    Console.Write(i + " ");
                }
                Console.WriteLine();
            }
        }
        static void MatrixGener()
        {
            Random rnd = new();

            for (int i = 0; i < m; i++)
            {

                for (int j = 0; j < n; j++)
                {
                    matrix[i, j] = Convert.ToInt32(rnd.Next(1, 100));
                    Console.Write(matrix[i, j] + " ");
                }
                Console.WriteLine();
            }

        }

        static void Scal()
        {
            for (int i = 0; i < m; i++)
            {

                for (int j = i + 1; j < m; j++)
                {
                    List<int> temp = new List<int>();
                    for (int k = 0; k < n; k++)
                    {
                        Console.WriteLine(matrix[i, k] + matrix[j, k] + " rows:" + j + " " + i);
                        temp.Add(matrix[i, k] + matrix[j, k]);
                    }
                    rowScalar.Add(temp);

                }
            }

        }
    }

    class IntSum
    {
        private static int n;
        private static int pareSum;
        private static int nonPareSum;


        public IntSum()
        {
            Console.WriteLine("Введіть n: ");
            n = Convert.ToInt32(Console.ReadLine());

            Thread p = new Thread(PareSum);
            p.Start();
            Thread np = new Thread(NonPareSum);
            np.Start();

            Task.Run(() =>
            {
                p.Join();
                np.Join();

                Console.WriteLine($"Сума парних: {pareSum}\nСума непарних: {nonPareSum}\n Сума: {pareSum + nonPareSum}");
            });
        }

        static void PareSum()
        {
            pareSum = 0;
            for (int i = 0; i <= n; i++)
            {
                if (i % 2 == 0) { pareSum += i; }
            }
        }

        static void NonPareSum()
        {
            nonPareSum = 0;
            for (int J = 0; J <= n; J++)
            {
                if (J % 2 != 0) { nonPareSum += J; }
            }
        }
    }

    class SetSum
    {
        private static List<double> doubles;
        private static double mult;
        private static double sum;
        private static int n;
        public SetSum()
        {
            Console.WriteLine("Введіть n(парне, якщо непарне тоді n + 1): ");
            n = isPare(Convert.ToInt32(Console.ReadLine()));
            SortedSet<double> temp = new SortedSet<double>();

            setGener(n, temp);
            doubles = temp.ToList();

            Thread m = new Thread(halfmult);
            m.Start();
            Thread s = new Thread(halfsum);
            s.Start();

            Task.Run(() =>
            {
                m.Join();
                s.Join();
                printSet();
                Console.WriteLine($"Добуток до n/2: {mult}\nСума після n/2: {sum}");


            });

        }

        static void printSet()
        {
            foreach (double d in doubles)
            {
                Console.Write(d + " ");
            }
            Console.WriteLine();
        }

        static int isPare(int n)
        {
            switch (n % 2 == 0)
            {
                case true: return n;
                case false: return n + 1;
            }
        }

        static void setGener(int n, SortedSet<double> temp)
        {
            Random rnd = new();

            for (int i = 0; i < n; i++)
            {
                temp.Add(Convert.ToDouble(rnd.NextDouble()));
            }
        }

        static void halfmult()
        {
            mult = doubles[0];
            for (int i = 1; i < n / 2; i++)
            {
                mult *= doubles[i];
            }

        }

        static void halfsum()
        {
            for (int i = n / 2; i < n; i++)
            {
                sum += doubles[i];
            }
        }

    }

    class NormVector
    {
        private static int a;
        private static int[,] matrix;
        private static int vecNorm = 1;
        private static List<int> generalVector;
        private static List<int> subVector;
        private static double gvn;
        private static double svn;


        public NormVector()
        {

            Console.WriteLine("Введіть ромір матриці: ");
            a = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Обрахувати:\n1:Евклідова норма\n2:Манхеттенська норма\nВведіть номер: ");
            if(Console.ReadLine() == "2") { vecNorm = 2; }
            else if (Console.ReadLine() == "1") { vecNorm = 1; }

            matrix = new int[a, a];
            generalVector = new List<int>();
            subVector = new List<int>();
            MatrixGener();

            Thread gv = new Thread(GeneralVector);
            gv.Start();
            Thread sv = new Thread(SubVector);
            sv.Start();

            Task.Run(() =>
            {
                gv.Join();
                sv.Join();

                Console.Write("Вектор головної діагоналі: ");
                printVector(generalVector);
                Console.WriteLine($"Норма вектора головної діагоналі: {gvn}");
                Console.Write("Вектор побічної діагоналі: ");
                printVector(subVector);
                Console.WriteLine($"Норма вектора побічної діагоналі: {svn}");
                Console.WriteLine($"Сума норм векторів {gvn + svn}");
            });
        }

        static void MatrixGener()
        {
            Random rnd = new();

            for (int i = 0; i < a; i++)
            {

                for (int j = 0; j < a; j++)
                {
                    matrix[i, j] = Convert.ToInt32(rnd.Next(-100, 100));
                }
            }

            for (int i = 0; i < a; i++)
            {
                for (int j = 0; j < a; j++)
                {
                    Console.Write(matrix[i, j] + " ");
                }
                Console.WriteLine();
            }

        }

        static void GeneralVector()
        {
            for (int i = 0; i < a; i++) {
                generalVector.Add(matrix[i,i]);
            }

            gvn = findVectorNorm(generalVector);
        }

        static void SubVector()
        {
            for (int i = 0; i < a; i++)
            {
                subVector.Add(matrix[a-1-i, i]);
            }

            svn = findVectorNorm(subVector);
        }

        static double findVectorNorm(List<int> vector) {
            double res = 0;
            if (vecNorm == 1)
            {
                double sum = 0;
                foreach (int i in vector) { sum += Math.Pow(i, 2); }
                res = Math.Sqrt(sum);
            }
            else if (vecNorm == 2)
            {
                foreach (int i in vector) { res += Math.Abs(i); }
            }

            return res;
        }

        static void printVector(List<int> vector)
        {
            foreach (int i in vector) { Console.Write(i + " "); }
            Console.WriteLine();
        }
    }

    class SumColm
    {
        private static int m;
        private static int n;
        private static int[,] matrix;
        private static int[] matrixMin;
        public SumColm()
        {
            Console.WriteLine("Введіть ромір m матриці: ");
            m = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Введіть ромір n матриці: ");
            n = Convert.ToInt32(Console.ReadLine());

            matrix = new int[m, n];
            matrixMin = new int[n];
            MatrixGener();

            Thread p = new Thread(findMinPareCol);
            p.Start();
            Thread np = new Thread(findMinNonPareCol);
            np.Start();

            Task.Run(() =>
            {
                p.Join();
                np.Join();

                printres();
            });

        }


        static void printres()
        {
            int sum = 0;



            foreach (int i in matrixMin)
            {
                Console.Write(i + " ");
                sum += i;
            }
            Console.WriteLine();
            Console.WriteLine($"Сума мінімальних: {sum}");
        }

        static void MatrixGener()
        {
            Random rnd = new();

            for (int i = 0; i < m; i++)
            {

                for (int j = 0; j < n; j++)
                {
                    matrix[i, j] = Convert.ToInt32(rnd.Next(1, 100));
                    Console.Write(matrix[i, j] + " ");
                }
                Console.WriteLine();
            }

        }

        static void findMinPareCol()
        {
            for (int i = 0; i < n; i++)
            {
                if (i % 2 == 0)
                {
                    int minNum = matrix[0, i];
                    for (int j = 1; j < m; j++)
                    {
                        if (minNum > matrix[j, i]) { minNum = matrix[j, i]; }
                    }
                    matrixMin[i] = minNum;
                }
            }
        }
        static void findMinNonPareCol()
        {
            for (int i = 0; i < n; i++)
            {
                if (i % 2 != 0)
                {
                    int minNum = matrix[0, i];
                    for (int j = 1; j < m; j++)
                    {
                        if (minNum > matrix[j, i]) { minNum = matrix[j, i]; }
                    }
                    matrixMin[i] = minNum;
                }
            }
        }
    }

    class AvrgRow
    {
        private static int m;
        private static int n;
        private static List<List<int>> matrix;
        private static int[] matrixAvrg;
        public AvrgRow()
        {
            Console.WriteLine("Введіть ромір m матриці: ");
            m = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Введіть ромір n матриці: ");
            n = Convert.ToInt32(Console.ReadLine());

            matrix = new List<List<int>>();
            matrixAvrg = new int[m];
            MatrixGener();

            Thread p = new Thread(findMaxPareRow);
            p.Start();
            Thread np = new Thread(findMaxNonPareRow);
            np.Start();

            Task.Run(() => { 
                p.Join();
                np.Join();
                printres();
            });

        }


        static void printres()
        {
            int sum = 0;



            foreach (int i in matrixAvrg)
            {
                Console.Write(i + " ");
                sum += i;
            }
            Console.WriteLine();
            Console.WriteLine($"Сума максимальних: {sum}");
        }

        static void MatrixGener()
        {
            Random rnd = new();

            for (int i = 0; i < m; i++)
            {
                List<int> temp = new List<int>();
                for (int j = 0; j < n; j++)
                {
                    int tmp = Convert.ToInt32(rnd.Next(0, 100));
                    Console.Write(tmp + " ");
                    temp.Add(tmp);
                }
                matrix.Add(temp);
                Console.WriteLine();
            }

        }

        static void findMaxPareRow()
        {
            for (int i = 0; i < m; i++) {
                if (i % 2 == 0) { matrixAvrg[i] = matrix[i].Max(); }
            }
        }
        static void findMaxNonPareRow()
        {
            for (int i = 0; i < m; i++)
            {
                if (i % 2 != 0) { matrixAvrg[i] = matrix[i].Max(); }
            }
        }

    }
}