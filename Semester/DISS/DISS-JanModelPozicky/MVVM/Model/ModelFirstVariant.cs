using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using DISS_HelperClasses;
using DISS_MonteCarloCore.Core;
using DISS.Random;
using DISS.Random.Continous;
using DISS.Random.Other;
using UniformC = DISS.Random.Continous.Uniform;
using UniformD = DISS.Random.Discrete.Uniform;

public class ModelFirstVariant : MonteCarloCore
{
    private UniformD _random24_25;
    private UniformC _random26_27;
    private Empiric _randomEmpiric28_29;
    private Deterministic<double> _randomDeterministic30_31;
    private UniformC _random32_33;

    private double _HU;
    private int _nRokovSplacania;
    private int _mSplatenychRokov;
    private double _Ipa;

    private double _zaplateneBanke;
    private double _totalZaplateneBanke;

    private double Ipm
    {
        get
        {
            return _Ipa / 12.0;
        }
    }

    private double M
    {
        get
        {
            var vrch = _HU * Ipm * Math.Pow(1.0 + Ipm, 12.0 * _nRokovSplacania);
            var spodok = Math.Pow(1.0 + Ipm, 12.0 * _nRokovSplacania) - 1.0;
            return vrch / spodok;
        }
    }

    private double S
    {
        get
        {
            var vrch = Math.Pow(1.0 + Ipm, 12.0 * _nRokovSplacania) - Math.Pow(1.0 + Ipm, 12.0 * _mSplatenychRokov);
            var spodok = Math.Pow(1.0 + Ipm, 12.0 * _nRokovSplacania) - 1.0;
            var zlomok = vrch / spodok;
            return _HU * zlomok;
            //return _HU * ((Math.Pow(1 + Ipm, 12 * _nRokovSplacania) - Math.Pow(1 + Ipm, 12 * _mSplatenychRokov)) /
            //              (Math.Pow(1 + Ipm, 12 * _nRokovSplacania) - 1));
        }
    }

    public ObservableCollection<Pair<Double, int>> Vysledky
    {
        get;
        set;
    }
    public ModelFirstVariant(int numberOfReplications, int cutFirst) : base(numberOfReplications, cutFirst)
    {
        Vysledky = new();
    }

    public override void BeforeAllReplications()
    {
        _random24_25 = new(1, 4, ExtendedRandom<double>.NextSeed());
        _random26_27 = new(0.3, 5.0, 1);
        List<EmpiricBase<double>.EmpiricDataWithSeed<double>> list = new();
        // U = <0.1, 0.3); p = 0.1
        // U = <0.3, 0.8); p = 0.35
        // U = <0.8, 1.2); p = 0.2
        // U = <1.2, 2.5); p = 0.15
        // U = <2.5, 3.8); p = 0.15
        // U = <3.8, 4.8); p = 0.05
        list.Add(new(0.1, 0.3, 0.1, ExtendedRandom<double>.NextSeed()));
        list.Add(new(0.3, 0.8, 0.35, ExtendedRandom<double>.NextSeed()));
        list.Add(new(0.8, 1.2, 0.2, ExtendedRandom<double>.NextSeed()));
        list.Add(new(1.2, 2.5, 0.15, ExtendedRandom<double>.NextSeed()));
        list.Add(new(2.5, 3.8, 0.15, ExtendedRandom<double>.NextSeed()));
        list.Add(new(3.8, 4.8, 0.05, ExtendedRandom<double>.NextSeed()));
        _randomEmpiric28_29 = new(list, ExtendedRandom<double>.NextSeed());
        _randomDeterministic30_31 = new(1.3);
        _random32_33 = new(0.9, 2.2, ExtendedRandom<double>.NextSeed());

        _totalZaplateneBanke = 0.0;

        Vysledky.Clear();
    }

    public override void BeforeReplication()
    {
        _HU = 100000.0;
        _nRokovSplacania = 10;
        _Ipa = 0.0;
        _mSplatenychRokov = 0;
        _zaplateneBanke = 0.0;
    }

    public override void Replication()
    {
        // Stratégia A
        // - fixkácia na 5 rokov 24-28
        // - fixkácia na 3 rokov 29-31
        // - fixkácia na 1 rokov 32
        // - fixkácia na 1 rokov 33

        // -- začiatok
        //2024 - 2028
        _Ipa = (double)(_random24_25.Next()) /100.0;
        _zaplateneBanke += (M*12.0*5.0);
        // koniec obdobia
        _mSplatenychRokov = 5;
        _HU = S;
        _nRokovSplacania -= 5;
        //_nRokovSplacania -= 5;
        
        //2029-2031
        _Ipa = _randomEmpiric28_29.Next() /100.0;
        _zaplateneBanke += (M*12.0*3.0);
        // koniec obdobia
        _mSplatenychRokov = 3;
        _HU = S;
        _nRokovSplacania -= 3;

        //2032
        _Ipa = _random32_33.Next() /100.0;
        _zaplateneBanke += (M*12.0);
        // koniec obdobia
        _mSplatenychRokov = 1;
        _HU = S;
        _nRokovSplacania -= 1;

        //2033
        _Ipa = _random32_33.Next() /100.0;
        _zaplateneBanke += (M*12.0);
        // koniec obdobia
        _mSplatenychRokov = 1;
        _HU = S;
        _nRokovSplacania -= 1;

    }

    public override void AfterReplication()
    {
        _totalZaplateneBanke += _zaplateneBanke;
        if (_currentReplication <= _cutFirst)
        {
            return;
        }

        var tmp = _totalZaplateneBanke / (_currentReplication -1);

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
        var tmp = _totalZaplateneBanke / _currentReplication;
        lock (Vysledky)
        {
            Vysledky.Add(new(tmp, _currentReplication));
        }
    }
}
