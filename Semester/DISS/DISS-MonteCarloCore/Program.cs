// See https://aka.ms/new-console-template for more information

using DISS_HelperClasses;
using DISS_MonteCarloCore.Core;
using DISS.Random;
using DISS.Random.Continous;
using DISS.Random.Other;

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
        list.Add(new(11.0, 12.0, 0.1));
        list.Add(new(12.0, 20.0, 0.6));
        list.Add(new(20.0, 25.0, 0.3));
        Empiric empiric = new(list);

        //save 1000 000 values to file
        using (StreamWriter sw = new("empiric.txt"))
        {
            for (int i = 0; i < 1_000_000; i++)
            {
                sw.WriteLine(empiric.Next());
            }
        } 
        
        Exponential exponential = new(((60.0 * 60.0) / 30.0));
        using (StreamWriter sw = new("exponential.txt"))
        {
            for (int i = 0; i < 1_000_000; i++)
            {
                sw.WriteLine(exponential.Next());
            }
        }
        
        Triangular triangular = new(60.0, 120.0, 480.0);
        using (StreamWriter sw = new("triangular.txt"))
        {
            for (int i = 0; i < 1_000_000; i++)
            {
                sw.WriteLine(triangular.Next());
            }
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