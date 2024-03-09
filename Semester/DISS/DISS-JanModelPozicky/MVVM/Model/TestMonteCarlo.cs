using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DISS_HelperClasses;
using DISS_MonteCarloCore.Core;
using DISS.Random.Continous;

public class TestMonteCarlo : MonteCarloCore
{
    private int _d = 100;
    private int _dlzkaIhly = 10;

    private Uniform _randomD;
    private Uniform _randomAlfa;

    private int _pocetPetatych;
    public ObservableCollection<Pair<Double, int>> Vysledky
    {
        get;
        set;
    }
    public ObservableCollection<Pair<Double, int>> VysledkyDisplay
    {
        get;
        set;
    }
    public TestMonteCarlo(int numberOfReplications, int cutFirst) : base(numberOfReplications, cutFirst)
    {
        Vysledky = new();
        Vysledky = new();
    }

    public override void BeforeAllReplications()
    {
        _randomD = new Uniform(0, _d);
        _randomAlfa = new Uniform(0, Math.PI / 2);
        _pocetPetatych = 0;

        Vysledky.Clear();
    }

    public override void BeforeReplication()
    {
        // zatial nič
        // Console.WriteLine("Thread: before replication ");
    }

    public override void Replication()
    {
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
        if (_currentReplication <= _cutFirst)
        {
            return;
        }
        double tmp = (2 * _dlzkaIhly) / (_d * (_pocetPetatych / (double)_currentReplication));

        int stepSize = _numberOfReplications / 100;
        if (stepSize >= 2)
        {
            if (_currentReplication % stepSize == 0)
            {
                lock (Vysledky)
                {
                    Vysledky.Add(new(tmp, _currentReplication));
                }
            }    
        }
        else
        {
            lock (Vysledky)
            {
                Vysledky.Add(new(tmp, _currentReplication));
            }
        }
        

    }

    public override void AfterAllReplications()
    {
        double tmp = (2 * _dlzkaIhly) / (_d * (_pocetPetatych / (double)_currentReplication));
        lock (Vysledky)
        {
            Vysledky.Add(new(tmp, _currentReplication));
        }
    }
}
