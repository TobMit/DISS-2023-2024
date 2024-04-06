using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using DISS_Model_Elektrokomponenty;
using DISS_Model_Elektrokomponenty.DataStructures;
using DISS_Model_Elektrokomponenty.Entity;
using DISS_S2_Elektroomponenty.Core;
using DISS_S2_Elektroomponenty.MVVM.Model;
using LiveCharts;
using LiveCharts.Wpf;

namespace DISS_S2_Elektroomponenty.MVVM.ViewModel;

public class MainViewModel : ObservableObjects
{
    private string _pocetRplikacii;
    private string _pocetObsluznychMiest;
    private string _pocetPokladni;
    private string _cutFirst;
    private string _pauseButtonText;

    private DISS_Model_Elektrokomponenty.Core? _behZavislotiCore1;
    private DISS_Model_Elektrokomponenty.Core? _behZavislotiCore2;
    private DISS_Model_Elektrokomponenty.Core? _behZavislotiCore3;
    private DISS_Model_Elektrokomponenty.Core? _behZavislotiCore4;
    private DISS_Model_Elektrokomponenty.Core? _behZavislotiCore5;
    
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
    private string _priemernyPocetObsluzenychZakaznikov;
    private string _priemerneVytazenieAutomatu;
    private string _premerneDlzkyRadovPredObsluhov;
    private ObservableCollection<PersonModel> _peoples;
    private bool _slowDown;
    private Visibility _replicationDetailVisibility;
    private double _sliderValue;
    private bool _behZavisloti;
    private Visibility _behZavislotiVisibility;
    private Visibility _behZavislotiVisibilityOstatne;
    private SeriesCollection? _seriesCollection;
    private ObservableCollection<string> _lables;

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
    
    public string CutFirst
    {
        get => _cutFirst;
        set
        {
            _cutFirst = value;
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
    
    public string PriemernyPocetObsluzenychZakaznikov
    {
        get => _priemernyPocetObsluzenychZakaznikov;
        set
        {
            if (String.Compare(value, _priemernyPocetObsluzenychZakaznikov, StringComparison.Ordinal) != 0)
            {
                _priemernyPocetObsluzenychZakaznikov = value;
                OnPropertyChanged();
            }
        }
    }
    
    public string PriemerneVytazenieAutomatu
    {
        get => _priemerneVytazenieAutomatu;
        set
        {
            if (String.Compare(value, _priemerneVytazenieAutomatu, StringComparison.Ordinal) != 0)
            {
                _priemerneVytazenieAutomatu = value;
                OnPropertyChanged();
            }
        }
    }
    
    public string PriemerneDlzkyRadovPredObsluhov
    {
        get => _premerneDlzkyRadovPredObsluhov;
        set
        {
            if (String.Compare(value, _premerneDlzkyRadovPredObsluhov, StringComparison.Ordinal) != 0)
            {
                _premerneDlzkyRadovPredObsluhov = value;
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
                SledovanieSimulacie();
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

    public double SliderValue
    {
        get => _sliderValue;
        set
        {
            _sliderValue = value;
            SetSimulaciaSpeed();
            OnPropertyChanged();
        }
    }
    
    public bool BehZavisloti
    {
        get => _behZavisloti;
        set
        {
            _behZavisloti = value;
            SetVidetelnostBehuZavislosti();
            OnPropertyChanged();
        }
    }

    public Visibility BehZavislotiVisibility
    {
        get => _behZavislotiVisibility;
        set
        {
            _behZavislotiVisibility = value;
            OnPropertyChanged();
        }
    }
    
    public Visibility BehZavislotiVisibilityOstatne
    {
        get => _behZavislotiVisibilityOstatne;
        set
        {
            _behZavislotiVisibilityOstatne = value;
            OnPropertyChanged();
        }
    }
    
    public SeriesCollection? SeriesCollection
    {
        get { return _seriesCollection; }
        set
        {
            _seriesCollection = value;
            OnPropertyChanged();
        }
    }
    
    public ObservableCollection<string> Labels
    {
        get => _lables;
        set
        {
            _lables = value;
            OnPropertyChanged();
        }
    }
    
    public MainViewModel()
    {
        InicialiseButtons();
        PocetReplikacii = "25_000";
        PocetObsluznychMiest = "13";
        PocetPokladni = "4";
        PauseButtonText = "Pause";
        CutFirst = "4000";
        SliderValue = 130;

        AktualnaReplikacia = "-/-";
        PriemernaDlzkaRaduPredAutomatom = "-/-";
        PriemernyCasPredAutomatom = "-/-";
        PriemernyCasVObchode = "-/-";
        PriemernyPocetZakaznikov = "-/-";
        PriemernyOdchodPoslednehoZakaznika = "-/-";
        PriemernyPocetObsluzenychZakaznikov = "-/-";
        PriemerneVytazenieAutomatu = "-/-";
        PriemerneDlzkyRadovPredObsluhov = "[-/-], [-/-], [-/-]";

        RadaPredAutomatom = "-/-";
        RadaPredObsluznimiMiestamiOnline = "-/-";
        RadaPredObsluznimiMiestamiBasic = "-/-";
        RadaPredObsluznimiMiestamiZmluvny = "-/-";
        Automat = new();
        SimulationTime = "-/-";
        SlowDown = false;
        ReplicationDetial = Visibility.Collapsed;
        BehZavisloti = false;
        BehZavislotiVisibility = Visibility.Collapsed;
        BehZavislotiVisibilityOstatne = Visibility.Visible;
        
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
        int cutFirst = 0;
        try
        {
            pocetReplikacii = Int32.Parse(PocetReplikacii.Replace("_", ""));
            pocetObsluznychMiest = int.Parse(PocetObsluznychMiest);
            pocetPokladni = int.Parse(PocetPokladni);
            cutFirst = int.Parse(CutFirst);

            if (pocetObsluznychMiest < 3)
            {
                throw new Exception("Nepravyn pocet obsluznych miest");
            }

            if (pocetPokladni < 1)
            {
                throw new Exception("Nespravny pocet pokladni");
            }

            if (cutFirst < 0)
            {
                throw new Exception("Nespravny cutFirst");
            }
        }
        catch (Exception e)
        {
            MessageBox.Show("Zle zadaný vstup" + e.Message, "Chyba vstupu", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        if (BehZavisloti)
        {
            // vyresetovanie štatistiky
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "2 Pokladne",
                    Values = new ChartValues<double>{},
                    Fill = Brushes.Transparent,
                    //PointGeometry = null
                },
                new LineSeries
                {
                    Title = "3 Pokladne",
                    Values = new ChartValues<double>{},
                    Fill = Brushes.Transparent,
                    //PointGeometry = null
                },
                new LineSeries
                {
                    Title = "4 Pokladne",
                    Values = new ChartValues<double>{},
                    Fill = Brushes.Transparent,
                    //PointGeometry = null
                },
                new LineSeries
                {
                    Title = "5 Pokladne",
                    Values = new ChartValues<double>{},
                    Fill = Brushes.Transparent,
                    //PointGeometry = null
                },
                new LineSeries
                {
                    Title = "6 Pokladni",
                    Values = new ChartValues<double>{},
                    Fill = Brushes.Transparent,
                    //PointGeometry = null
                }
            };
            
            _behZavislotiCore1 = new DISS_Model_Elektrokomponenty.Core(pocetReplikacii, cutFirst, pocetObsluznychMiest, 2)
            {
                SlowDown = false,
                BehZavislosti = true
            };
            _behZavislotiCore1.DataAvailable += BehZavislotiUpdateUi1;
            _behZavislotiCore2 = new DISS_Model_Elektrokomponenty.Core(pocetReplikacii, cutFirst, pocetObsluznychMiest, 3)
            {
                SlowDown = false,
                BehZavislosti = true
            };
            _behZavislotiCore2.DataAvailable += BehZavislotiUpdateUi2;
            _behZavislotiCore3 = new DISS_Model_Elektrokomponenty.Core(pocetReplikacii, cutFirst, pocetObsluznychMiest, 4)
            {
                SlowDown = false,
                BehZavislosti = true
            };
            _behZavislotiCore3.DataAvailable += BehZavislotiUpdateUi3;
            _behZavislotiCore4 = new DISS_Model_Elektrokomponenty.Core(pocetReplikacii, cutFirst, pocetObsluznychMiest, 5)
            {
                SlowDown = false,
                BehZavislosti = true
            };
            _behZavislotiCore4.DataAvailable += BehZavislotiUpdateUi4;
            _behZavislotiCore5 = new DISS_Model_Elektrokomponenty.Core(pocetReplikacii, cutFirst, pocetObsluznychMiest, 6)
            {
                SlowDown = false,
                BehZavislosti = true
            };
            _behZavislotiCore5.DataAvailable += BehZavislotiUpdateUI5;
            
            _behZavislotiCore1.Run();
            _behZavislotiCore2.Run();
            _behZavislotiCore3.Run();
            _behZavislotiCore4.Run();
            _behZavislotiCore5.Run();
            return;
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
        PauseButtonText = "Pause";

        _core = new DISS_Model_Elektrokomponenty.Core(pocetReplikacii, 0, pocetObsluznychMiest, pocetPokladni)
        {
            SlowDown = _slowDown,
            SlowDownSpeed = SliderValue
        };
        _core.DataAvailable += UpdateUI;
        _core.Run();
    }

    private void StopModel()
    {
        if (_core is not null)
        {
            _core.Stop();
            PauseButtonText = "Pause";
        }

        if (_behZavislotiCore1 is not null)
        {
            _behZavislotiCore1.Stop();
        }
        if (_behZavislotiCore2 is not null)
        {
            _behZavislotiCore2.Stop();
        }
        if (_behZavislotiCore3 is not null)
        {
            _behZavislotiCore3.Stop();
        }
        if (_behZavislotiCore4 is not null)
        {
            _behZavislotiCore4.Stop();
        }
        if (_behZavislotiCore5 is not null)
        {
            _behZavislotiCore5.Stop();
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

        if (_behZavislotiCore1 is not null)
        {
            if (_behZavislotiCore1.IsRunning)
            {
                if (_behZavislotiCore1.Pause)
                {
                    _behZavislotiCore1.Pause = false;
                    PauseButtonText = "Pause";
                }
                else
                {
                    _behZavislotiCore1.Pause = true;
                    PauseButtonText = "Continue";
                }
            }
        }
        if (_behZavislotiCore2 is not null)
        {
            if (_behZavislotiCore2.IsRunning)
            {
                if (_behZavislotiCore2.Pause)
                {
                    _behZavislotiCore2.Pause = false;
                }
                else
                {
                    _behZavislotiCore2.Pause = true;
                }
            }
        }
        if (_behZavislotiCore3 is not null)
        {
            if (_behZavislotiCore3.IsRunning)
            {
                if (_behZavislotiCore3.Pause)
                {
                    _behZavislotiCore3.Pause = false;
                }
                else
                {
                    _behZavislotiCore3.Pause = true;
                }
            }
        }
        if (_behZavislotiCore4 is not null)
        {
            if (_behZavislotiCore4.IsRunning)
            {
                if (_behZavislotiCore4.Pause)
                {
                    _behZavislotiCore4.Pause = false;
                }
                else
                {
                    _behZavislotiCore4.Pause = true;
                }
            }
        }
        if (_behZavislotiCore5 is not null)
        {
            if (_behZavislotiCore5.IsRunning)
            {
                if (_behZavislotiCore5.Pause)
                {
                    _behZavislotiCore5.Pause = false;
                }
                else
                {
                    _behZavislotiCore5.Pause = true;
                }
            }
        }
    }

    private void SledovanieSimulacie()
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

    private void SetSimulaciaSpeed()
    {
        if (_core is not null)
        {
            _core.SlowDownSpeed = _sliderValue;
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
    
    private void SetVidetelnostBehuZavislosti()
    {
        if (_behZavisloti)
        {
            BehZavislotiVisibility = Visibility.Visible;
            BehZavislotiVisibilityOstatne = Visibility.Collapsed;
            ReplicationDetial = Visibility.Collapsed;
            if (_core is not null)
            {
                _core.Stop();
            }
        }
        else
        {
            BehZavislotiVisibility = Visibility.Collapsed;
            BehZavislotiVisibilityOstatne = Visibility.Visible;
            SetReplicationDetailVisibilit(SlowDown);
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
                PriemernyPocetObsluzenychZakaznikov = e.PriemernyPocetObsluzenychZakaznikov;
                PriemerneVytazenieAutomatu = e.PriemerneVytazenieAutomatu;
                PriemerneDlzkyRadovPredObsluhov = e.PriemerneDlzkyRadovPredObsluhov;
            }
        });
    }
    
    private void BehZavislotiUpdateUi1(object? sender, DataStructure e)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            AktualnaReplikacia = e.AktuaReplikacia;
            SeriesCollection?[0].Values.Add(e.BehZavislostiPriemernyPocetZakaznikovPredAutomatom);
        });
    }
    private void BehZavislotiUpdateUi2(object? sender, DataStructure e)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            SeriesCollection?[1].Values.Add(e.BehZavislostiPriemernyPocetZakaznikovPredAutomatom);
        });
    }
    private void BehZavislotiUpdateUi3(object? sender, DataStructure e)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            SeriesCollection?[2].Values.Add(e.BehZavislostiPriemernyPocetZakaznikovPredAutomatom);
        });
    }
    private void BehZavislotiUpdateUi4(object? sender, DataStructure e)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            SeriesCollection?[3].Values.Add(e.BehZavislostiPriemernyPocetZakaznikovPredAutomatom);
        });
    }
    private void BehZavislotiUpdateUI5(object? sender, DataStructure e)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            SeriesCollection?[4].Values.Add(e.BehZavislostiPriemernyPocetZakaznikovPredAutomatom);
        });
    }
}