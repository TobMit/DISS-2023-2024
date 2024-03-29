// See https://aka.ms/new-console-template for more information

using DISS_HelperClasses;
using DISS_MonteCarloCore.Core;
using DISS.Random;
using DISS.Random.Continous;

class TestMonteCarlo : MonteCarloCore
{
    private int _d = 1000;
    private int _dlzkaIhly = 10;

    private Uniform _randomD;
    private Uniform _randomAlfa;

    private int _pocetPetatych;
    public List<Pair<Double, int>> Vysledky {
        get;
        set;
    }
    public TestMonteCarlo(int numberOfReplications, int cutFirst) : base(numberOfReplications, cutFirst)
    {
    }

    public override void BeforeAllReplications()
    {
        _randomD = new Uniform(0, _d);
        _randomAlfa = new Uniform(0, Math.PI / 2);
        _pocetPetatych = 0;

        Vysledky = new();
    }

    public override void BeforeReplication()
    {
        // zatial nič
        // Console.WriteLine("Thread: before replication ");
    }

    public override void Replication()
    {
        // Console.WriteLine("Thread: replication ");
        double x = _randomD.Next();
        double alfa = _randomAlfa.Next();
        double y = x + _dlzkaIhly * Math.Sin(alfa);
        if (y > _d)
        {
            _pocetPetatych++;
        }
    }

    public override void AfterReplication()
    {
        // Console.WriteLine("Thread: after replication start");
        double tmp = (2 * _dlzkaIhly) / (_d * (_pocetPetatych / (double)_currentReplication));
        lock (Vysledky)
        {
            Vysledky.Add(new(tmp, _currentReplication));
        }
        // Console.WriteLine("Thread: after replication end");
    }

    public override void AfterAllReplications()
    {
        // Console.WriteLine("Thread: after all replication start");
        double tmp = (2 * _dlzkaIhly) / (_d * (_pocetPetatych / (double)_numberOfReplications));
        lock (Vysledky)
        {
            Vysledky.Add(new(tmp, _currentReplication));
        }
        // Console.WriteLine("Thread: after replication end");
    }
}

class Program
{
    static void Main(string[] args)
    {
        List<EmpiricBase<double>.EmpiricData<double>> list = new();
        // U = <0.1, 0.3); p = 0.1
        // U = <0.3, 0.8); p = 0.35
        // U = <0.8, 1.2); p = 0.2
        // U = <1.2, 2.5); p = 0.15
        // U = <2.5, 3.8); p = 0.15
        // U = <3.8, 4.8); p = 0.05
        list.Add(new(0.1, 0.3, 0.1));
        list.Add(new(0.3, 0.8, 0.35));
        list.Add(new(0.8, 1.2, 0.2));
        list.Add(new(1.2, 2.5, 0.15));
        list.Add(new(2.5, 3.8, 0.15));
        list.Add(new(3.8, 4.8, 0.05));
        Empiric test = new(list);

        // save 1000 000 values to file
        // using (StreamWriter sw = new("empiric.txt"))
        // {
        //     for (int i = 0; i < 1000000; i++)
        //     {
        //         sw.WriteLine(test.Next());
        //     }
        // } 

        PriorityQueue<string, int> testQueue = new();

        testQueue.Enqueue("4", 4);
        testQueue.Enqueue("3.1", 3);
        testQueue.Enqueue("3.3", 3);
        testQueue.Enqueue("2", 2);
        testQueue.Enqueue("3.2", 3);
        testQueue.Enqueue("1", 1);

        while (testQueue.Count > 0)
        {
            Console.WriteLine(testQueue.Dequeue());
        }

        // TestMonteCarlo test = new TestMonteCarlo(1000000000, 0);
        // test.Run();
        // bool continueRunning = true;
        //
        // while (test.IsRunning)
        // {
        //     // Console.WriteLine("Program: before read Event Wait");
        //     test.ReadEvent.Wait();
        //
        //     if (!test.IsRunning)
        //     {
        //         test.ReadEvent.Reset();
        //         break;
        //     }
        //     
        //
        //     lock (test.Vysledky)
        //     {
        //         Console.WriteLine($"Aktualny vysledok je: {test.Vysledky[^1].First} cislo rep: {test.Vysledky[^1].Second}");
        //         Console.WriteLine($"Aktualnz pocet zaznamenannych replikacii je: {test.Vysledky.Count}");
        //
        //         // if (test.Vysledky.Last().Second > 1000000)
        //         // {
        //         //     Console.WriteLine("---------------- Stopping simulation -----------------");
        //         //     test.Stop();
        //         // }
        //     }
        //     continueRunning = test.IsRunning;
        //     test.ReadEvent.Reset();
        //     // Console.WriteLine("Program: after read Event Reset");
        // }
        // test.End();
        // Console.WriteLine($"Fynaliny vysledok je: {test.Vysledky[^1].First} cislo rep: {test.Vysledky[^1].Second}");
    }
}