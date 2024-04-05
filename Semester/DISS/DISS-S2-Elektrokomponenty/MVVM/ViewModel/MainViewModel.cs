using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using DISS_Model_Elektrokomponenty;
using DISS_Model_Elektrokomponenty.DataStructures;
using DISS_Model_Elektrokomponenty.Entity;
using DISS_S2_Elektroomponenty.Core;
using DISS_S2_Elektroomponenty.MVVM.Model;

namespace DISS_S2_Elektroomponenty.MVVM.ViewModel;

public class MainViewModel : ObservableObjects
{
    private string _pocetRplikacii;
    private string _pocetObsluznychMiest;
    private string _pocetPokladni;
    private string _pauseButtonText;

    private DISS_Model_Elektrokomponenty.Core? _core;
    private string _simulationTime;
    private string _radaPredAutomatom;
    private AutomatModel _automat;
    private string _radaPredObsluznimiMiestamiOnline;
    private string _radaPredObsluznimiMiestamiBasic;
    private string _radaPredObsluznimiMiestamiZmluvny;
    private ObservableCollection<ObsluzneMiestoModel> _obsluzneMiestos;
    private ObservableCollection<PokladnaModel> _pokladne;
    private string _aktulnaReplikacia;
    private string _priemernyCasVObchode;
    private string _priemernyCasPredAutomatom;
    private string _priemernaDlzkaRaduPredAutomatom;
    private string _priemernyOdchodPoslednehoZakaznika;
    private string _priemernyPocetZakaznikov;
    private ObservableCollection<PersonModel> _peoples;
    private bool _slowDown;
    private Visibility _replicationDetailVisibility;

    public RelayCommand StartCommand { get; set; }
    public RelayCommand StopCommand { get; set; }
    public RelayCommand PauseCommand { get; set; }


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
            if (String.Compare(value, _radaPredAutomatom, StringComparison.Ordinal) != 0)
            {
                _radaPredAutomatom = value;
                OnPropertyChanged();
            }
        }
    }

    public AutomatModel Automat
    {
        get => _automat;
        set
        {
            _automat = value;
        }
    }

    public string RadaPredObsluznimiMiestamiOnline
    {
        get => _radaPredObsluznimiMiestamiOnline;
        set
        {
            // this should be faster (on binary level)
            if (String.Compare(value, _radaPredObsluznimiMiestamiOnline, StringComparison.Ordinal) != 0)
            {
                _radaPredObsluznimiMiestamiOnline = value;
                OnPropertyChanged();
            }
        }
    }
    public string RadaPredObsluznimiMiestamiBasic
    {
        get => _radaPredObsluznimiMiestamiBasic;
        set
        {
            if (String.Compare(value, _radaPredObsluznimiMiestamiBasic, StringComparison.Ordinal) != 0)
            {
                _radaPredObsluznimiMiestamiBasic = value;
                OnPropertyChanged();
            }
        }
    }
    public string RadaPredObsluznimiMiestamiZmluvny
    {
        get => _radaPredObsluznimiMiestamiZmluvny;
        set
        {
            if (String.Compare(value, _radaPredObsluznimiMiestamiZmluvny, StringComparison.Ordinal) != 0)
            {
                _radaPredObsluznimiMiestamiZmluvny = value;
                OnPropertyChanged();
            }
        }
    }

    public ObservableCollection<ObsluzneMiestoModel> ObsluzneMiestos
    {
        get => _obsluzneMiestos;
        set
        {
            _obsluzneMiestos = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<PokladnaModel> Pokladne
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
            if (String.Compare(value, _priemernyCasVObchode, StringComparison.Ordinal) != 0)
            {
                _priemernyCasVObchode = value;
                OnPropertyChanged();
            }
        }
    }

    public string PriemernyCasPredAutomatom
    {
        get => _priemernyCasPredAutomatom;
        set
        {
            if (String.Compare(value, _priemernyCasPredAutomatom, StringComparison.Ordinal) != 0)
            {
                _priemernyCasPredAutomatom = value;
                OnPropertyChanged();
            }
        }
    }

    public string PriemernaDlzkaRaduPredAutomatom
    {
        get => _priemernaDlzkaRaduPredAutomatom;
        set
        {
            if (String.Compare(value, _priemernaDlzkaRaduPredAutomatom, StringComparison.Ordinal) != 0)
            {
                _priemernaDlzkaRaduPredAutomatom = value;
                OnPropertyChanged();
            }
        }
    }

    public string PriemernyOdchodPoslednehoZakaznika
    {
        get => _priemernyOdchodPoslednehoZakaznika;
        set
        {
            if (String.Compare(value, _priemernyOdchodPoslednehoZakaznika, StringComparison.Ordinal) != 0)
            {
                _priemernyOdchodPoslednehoZakaznika = value;
                OnPropertyChanged();
            }
        }
    }

    public string PriemernyPocetZakaznikov
    {
        get => _priemernyPocetZakaznikov;
        set
        {
            if (String.Compare(value, _priemernyPocetZakaznikov, StringComparison.Ordinal) != 0)
            {
                _priemernyPocetZakaznikov = value;
                OnPropertyChanged();
            }
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
            if (String.Compare(value, _aktulnaReplikacia, StringComparison.Ordinal) != 0)
            {
                _aktulnaReplikacia = value;
                OnPropertyChanged();
            }
        }
    }

    public bool SlowDown
    {
        get => _slowDown;
        set
        {
            if (value != _slowDown)
            {
                _slowDown = value;
                SetReplicationDetailVisibilit(_slowDown);
                RychlostModel();
                OnPropertyChanged();
            }
        }
    }

    public Visibility ReplicationDetial
    {
        get => _replicationDetailVisibility;
        set
        {
            _replicationDetailVisibility = value;
            OnPropertyChanged();
        }
    }

    public string PauseButtonText
    {
        get => _pauseButtonText;
        set
        {
            _pauseButtonText = value;
            OnPropertyChanged();
        }

    }

    public MainViewModel()
    {
        InicialiseButtons();
        PocetReplikacii = "50_000";
        PocetObsluznychMiest = "15";
        PocetPokladni = "6";
        PauseButtonText = "Pause";

        AktualnaReplikacia = "-/-";
        PriemernaDlzkaRaduPredAutomatom = "-/-";
        PriemernyCasPredAutomatom = "-/-";
        PriemernyCasVObchode = "-/-";
        PriemernyPocetZakaznikov = "-/-";
        PriemernyOdchodPoslednehoZakaznika = "-/-";

        RadaPredAutomatom = "-/-";
        RadaPredObsluznimiMiestamiOnline = "-/-";
        RadaPredObsluznimiMiestamiBasic = "-/-";
        RadaPredObsluznimiMiestamiZmluvny = "-/-";
        Automat = new();
        SimulationTime = "-/-";
        SlowDown = false;
        ReplicationDetial = Visibility.Collapsed;
    }

    private void InicialiseButtons()
    {
        StartCommand = new RelayCommand(o => StartModel());
        StopCommand = new RelayCommand(o => StopModel());
        PauseCommand = new RelayCommand(o => PauseModel());
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

        List<ObsluzneMiestoModel> tmpList = new(pocetObsluznychMiest);
        for (int i = 0; i < pocetObsluznychMiest; i++)
        {
            tmpList.Add(new());
        }
        ObsluzneMiestos = new(tmpList);

        List<PokladnaModel> tmpPokladne = new(pocetPokladni);
        for (int i = 0; i < pocetPokladni; i++)
        {
            tmpPokladne.Add(new());
        }
        Pokladne = new(tmpPokladne);

        _core = new DISS_Model_Elektrokomponenty.Core(pocetReplikacii, 0, pocetObsluznychMiest, pocetPokladni) {SlowDown = _slowDown};
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

    private void PauseModel()
    {
        if (_core is not null)
        {
            if (_core.IsRunning)
            {
                if (_core.Pause)
                {
                    _core.Pause = false;
                    PauseButtonText = "Pause";
                }
                else
                {
                    _core.Pause = true;
                    PauseButtonText = "Continue";
                }
            }
        }
    }

    private void RychlostModel()
    {
        if (_core is not null)
        {
            if (_slowDown)
            {
                _core.SlowDown = true;
            }
            else
            {
                _core.SlowDown = false;
            }
        }
    }

    private void SetReplicationDetailVisibilit(bool visibilit)
    {
        if (visibilit)
        {
            ReplicationDetial = Visibility.Visible;
        }
        else
        {
            ReplicationDetial = Visibility.Collapsed;
        }
    }
    
    private void UpdateUI(object? sender, DataStructure e)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            // pri pomalom behu update cas aj keď nie je zadna aktiviata -> plynulejsi beh
            if (e.ShallowUpdate)
            {
                SimulationTime = e.SimulationTime;
            }

            if (e.NewData)
            {
                e.NewData = false;
                if (e.ShallowUpdate)
                {
                    if (Peoples is null)
                    {
                        Peoples = new();
                    }

                    // nahradím novým listom
                    if (Peoples.Count > e.People.Count)
                    {
                        Peoples = new();
                    }
                    for (int i = 0; i < e.People.Count; i++)
                    {
                        if (i < Peoples.Count )
                        {
                            Peoples[i].Update(e.People[i]);
                        }
                        else
                        {
                            Peoples.Add(new PersonModel(e.People[i]));
                        }
                    }
                    RadaPredAutomatom = e.RadaPredAutomatom;
                    Automat.Update(e.Automat);
                    RadaPredObsluznimiMiestamiOnline = e.RadaPredObsluznimiMiestamiOnline;
                    RadaPredObsluznimiMiestamiBasic = e.RadaPredObsluznimiMiestamiBasic;
                    RadaPredObsluznimiMiestamiZmluvny = e.RadaPredObsluznimiMiestamiZmluvny;
                    for (int i = 0; i < e.ObsluzneMiestos.Count; i++)
                    {
                        ObsluzneMiestos[i].Update(e.ObsluzneMiestos[i]);
                    }
                    for (int i = 0; i < e.Pokladne.Count; i++)
                    {
                        Pokladne[i].Update(e.Pokladne[i]);
                    };   
                }

                AktualnaReplikacia = e.AktuaReplikacia;
                PriemernyCasVObchode = e.PriemernyCasVObhchode;
                PriemernyCasPredAutomatom = e.PriemernyCasPredAutomatom;
                PriemernaDlzkaRaduPredAutomatom = e.PriemernaDlzkaraduPredAutomatom;
                PriemernyOdchodPoslednehoZakaznika = e.PriemernyOdchodPoslednehoZakaznika;
                PriemernyPocetZakaznikov = e.PriemernyPocetZakaznikov;
            }
        });
    }
}