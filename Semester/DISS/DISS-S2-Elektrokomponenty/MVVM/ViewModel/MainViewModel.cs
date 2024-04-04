using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
using DISS_Model_Elektrokomponenty;
using DISS_Model_Elektrokomponenty.DataStructures;
using DISS_S2_Elektroomponenty.Core;
using DISS_S2_Elektroomponenty.MVVM.Model;

namespace DISS_S2_Elektroomponenty.MVVM.ViewModel;

public class MainViewModel : ObservableObjects
{
    private string _pocetRplikacii;
    private string _pocetObsluznychMiest;
    private string _pocetPokladni;

    private DISS_Model_Elektrokomponenty.Core? _core;
    private string _simulationTime;
    private string _radaPredAutomatom;
    private string _automat;
    private string _radaPredObsluznimiMiestami;
    private ObservableCollection<string> _obsluzneMiestos;
    private ObservableCollection<string> _pokladne;
    private string _aktulnaReplikacia;
    private string _priemernyCasVObchode;
    private string _priemernyCasPredAutomatom;
    private string _priemernaDlzkaRaduPredAutomatom;
    private string _priemernyOdchodPoslednehoZakaznika;
    private string _priemernyPocetZakaznikov;
    private ObservableCollection<PersonModel> _peoples;

    public RelayCommand StartCommand { get; set; }
    public RelayCommand StopCommand { get; set; }
    public RelayCommand RychlostCommand { get; set; }


    public string PocetReplikacii
    {
        get => _pocetRplikacii;
        set
        {
            _pocetRplikacii = value;
            OnPropertyChanged();
        }
    }
    public string PocetObsluznychMiest
    {
        get => _pocetObsluznychMiest;
        set
        {
            _pocetObsluznychMiest = value;
            OnPropertyChanged();
        }
    }
    public string PocetPokladni
    {
        get => _pocetPokladni;
        set
        {
            _pocetPokladni = value;
            OnPropertyChanged();
        }
    }

    public string SimulationTime
    {
        get => _simulationTime;
        set
        {
            _simulationTime = value;
            OnPropertyChanged();
        }
    }

    public string RadaPredAutomatom
    {
        get => _radaPredAutomatom;
        set
        {
            _radaPredAutomatom = value;
            OnPropertyChanged();
        }
    }

    public string Automat
    {
        get => _automat;
        set
        {
            _automat = value;
            OnPropertyChanged();
        }
    }

    public string RadaPredObsluznimiMiestami
    {
        get => _radaPredObsluznimiMiestami;
        set
        {
            _radaPredObsluznimiMiestami = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<string> ObsluzneMiestos
    {
        get => _obsluzneMiestos;
        set
        {
            _obsluzneMiestos = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<string> Pokladne
    {
        get => _pokladne;
        set
        {
            _pokladne = value;
            OnPropertyChanged();
        }
    }

    public string PriemernyCasVObchode
    {
        get => _priemernyCasVObchode;
        set
        {
            _priemernyCasVObchode = value;
            OnPropertyChanged();
        }
    }

    public string PriemernyCasPredAutomatom
    {
        get => _priemernyCasPredAutomatom;
        set
        {
            _priemernyCasPredAutomatom = value;
            OnPropertyChanged();
        }
    }

    public string PriemernaDlzkaRaduPredAutomatom
    {
        get => _priemernaDlzkaRaduPredAutomatom;
        set
        {
            _priemernaDlzkaRaduPredAutomatom = value;
            OnPropertyChanged();
        }
    }

    public string PriemernyOdchodPoslednehoZakaznika
    {
        get => _priemernyOdchodPoslednehoZakaznika;
        set
        {
            _priemernyOdchodPoslednehoZakaznika = value;
            OnPropertyChanged();
        }
    }

    public string PriemernyPocetZakaznikov
    {
        get => _priemernyPocetZakaznikov;
        set
        {
            _priemernyPocetZakaznikov = value;
            OnPropertyChanged();
        }
    }
    
    public ObservableCollection<PersonModel> Peoples
    {
        get => _peoples;
        set
        {
            _peoples = value;
            OnPropertyChanged();
        }
    }
    public string AktualnaReplikacia
    {
        get => _aktulnaReplikacia;
        set
        {
            _aktulnaReplikacia = value;
            OnPropertyChanged();
        }
    }

    public MainViewModel()
    {
        InicialiseButtons();
        PocetReplikacii = "50_000";
        PocetObsluznychMiest = "15";
        PocetPokladni = "6";

        AktualnaReplikacia = "-/-";
        PriemernaDlzkaRaduPredAutomatom = "-/-";
        PriemernyCasPredAutomatom = "-/-";
        PriemernyCasVObchode = "-/-";
        PriemernyPocetZakaznikov = "-/-";
        PriemernyOdchodPoslednehoZakaznika = "-/-";

        RadaPredAutomatom = "-/-";
        Automat = "-/-";
    }

    private void InicialiseButtons()
    {
        StartCommand = new RelayCommand(o => StartModel());
        StopCommand = new RelayCommand(o => StopModel());
        RychlostCommand = new RelayCommand(o => RychlostModel());
    }

    private void StartModel()
    {
        int pocetReplikacii = 3;
        int pocetObsluznychMiest = 3;
        int pocetPokladni = 1;
        try
        {
            pocetReplikacii = Int32.Parse(PocetReplikacii.Replace("_", ""));
            pocetObsluznychMiest = int.Parse(PocetObsluznychMiest);
            pocetPokladni = int.Parse(PocetPokladni);

            if (pocetObsluznychMiest < 3)
            {
                throw new Exception("Nepravyn pocet obsluznych miest");
            }

            if (pocetPokladni < 1)
            {
                throw new Exception("Nespravny pocet pokladni");
            }
        }
        catch (Exception e)
        {
            MessageBox.Show("Zle zadaný vstup" + e.Message, "Chyba vstupu", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        _core = new DISS_Model_Elektrokomponenty.Core(pocetReplikacii, 0, pocetObsluznychMiest, pocetPokladni);
        _core.DataAvailable += UpdateUI;
        _core.Run();
    }

    private void StopModel()
    {
        if (_core is not null)
        {
            _core.Stop();
        }
    }

    private void RychlostModel()
    {
        if (_core is not null)
        {
            _core.SlowDown = !_core.SlowDown;
        }
    }
    
    private void UpdateUI(object? sender, DataStructure e)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            if (e.ShallowUpdate)
            {
                SimulationTime = e.SimulationTime;
                ObservableCollection<PersonModel> tmpCollection = new();
                foreach (var person in e.People)
                {
                    tmpCollection.Add(new PersonModel(person));
                }
                Peoples = tmpCollection;
                RadaPredAutomatom = e.RadaPredAutomatom;
                Automat = e.Automat;
                RadaPredObsluznimiMiestami = e.RadaPredObsluznimiMiestami;
                ObsluzneMiestos = new(e.ObsluzneMiestos);
                Pokladne = new(e.Pokladne);   
            }

            AktualnaReplikacia = e.AktuaReplikacia;
            PriemernyCasVObchode = e.PriemernyCasVObhchode;
            PriemernyCasPredAutomatom = e.PriemernyCasPredAutomatom;
            PriemernaDlzkaRaduPredAutomatom = e.PriemernaDlzkaraduPredAutomatom;
            PriemernyOdchodPoslednehoZakaznika = e.PriemernyOdchodPoslednehoZakaznika;
            PriemernyPocetZakaznikov = e.PriemernyPocetZakaznikov;
        });
    }
}