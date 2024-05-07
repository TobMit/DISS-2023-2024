using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using DISS_S3_Elektrokomponenty.Core;
using LiveCharts;
using LiveCharts.Wpf;
using simulation;

namespace DISS_S3_Elektrokomponenty.MVVM.ViewModel;

public class MainViewModel : ObservableObjects
{
    private string _pocetRplikacii;
    private string _pocetObsluznychMiest;
    private string _pocetPokladni;
    private string _cutFirst;
    private string _pauseButtonText;

    private MySimulation? _behZavislotiCore1;
    private MySimulation? _behZavislotiCore2;
    private MySimulation? _behZavislotiCore3;
    private MySimulation? _behZavislotiCore4;
    private MySimulation? _behZavislotiCore5;

    private MySimulation? _core;
    private string _simulationTime;
    private string _radaPredAutomatom;
    private AutomatModel _automat;
    private string _radaPredObsluznimiMiestamiOnline;
    private string _radaPredObsluznimiMiestamiBasic;
    private string _radaPredObsluznimiMiestamiZmluvny;
    private ObservableCollection<ObsluzneMiestoModel> _obsluzneMiestos;
    private ObservableCollection<PokladnaModel> _pokladne;
    private ObservableCollection<ReklamaciaModel> _reklamacie;
    private string _aktulnaReplikacia;
    private string _priemernyCasVObchode;
    private string _priemernyCasPredAutomatom;
    private string _pocetZakaznikov;
    private string _priemernyOdchodPoslednehoZakaznika;
    private string _priemernyPocetZakaznikov;
    private string _priemernyPocetObsluzenychZakaznikov;
    private string _priemerneVytazenieAutomatu;
    private string _priemerneDlzkyRadovPredObsluhov;
    private string _priemerneDlzkyRadovPredPokladnami;
    private string _priemerneVytazeniePokladni;
    private string _priemerneVytazenieObsluhyOnline;
    private string _priemerneVytazenieObsluhyOstatne;
    private ObservableCollection<String> _peoples;
    private bool _slowDown;
    private bool _break;
    private bool _zvysenyTok;
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
        set { _automat = value; }
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

    public ObservableCollection<ReklamaciaModel> Reklamacie
    {
        get => _reklamacie;
        set
        {
            _reklamacie = value;
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

    public string PocetZakaznikov
    {
        get => _pocetZakaznikov;
        set
        {
            if (String.Compare(value, _pocetZakaznikov, StringComparison.Ordinal) != 0)
            {
                _pocetZakaznikov = value;
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
        get => _priemerneDlzkyRadovPredObsluhov;
        set
        {
            if (String.Compare(value, _priemerneDlzkyRadovPredObsluhov, StringComparison.Ordinal) != 0)
            {
                _priemerneDlzkyRadovPredObsluhov = value;
                OnPropertyChanged();
            }
        }
    }

    public string PriemerneDlzkyRadovPredPokladnami
    {
        get => _priemerneDlzkyRadovPredPokladnami;
        set
        {
            if (String.Compare(value, _priemerneDlzkyRadovPredPokladnami, StringComparison.Ordinal) != 0)
            {
                _priemerneDlzkyRadovPredPokladnami = value;
                OnPropertyChanged();
            }
        }
    }

    public string PriemerneVytazeniePokladni
    {
        get => _priemerneVytazeniePokladni;
        set
        {
            if (String.Compare(value, _priemerneVytazeniePokladni, StringComparison.Ordinal) != 0)
            {
                _priemerneVytazeniePokladni = value;
                OnPropertyChanged();
            }
        }
    }

    public string PriemerneVytazenieObsluhyOnline
    {
        get => _priemerneVytazenieObsluhyOnline;
        set
        {
            if (String.Compare(value, _priemerneVytazenieObsluhyOnline, StringComparison.Ordinal) != 0)
            {
                _priemerneVytazenieObsluhyOnline = value;
                OnPropertyChanged();
            }
        }
    }

    public string PriemerneVytazenieObsluhyOstatne
    {
        get => _priemerneVytazenieObsluhyOstatne;
        set
        {
            if (String.Compare(value, _priemerneVytazenieObsluhyOstatne, StringComparison.Ordinal) != 0)
            {
                _priemerneVytazenieObsluhyOstatne = value;
                OnPropertyChanged();
            }
        }
    }

    public ObservableCollection<string> Peoples
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
    
    public bool Break
    {
        get => _break;
        set
        {
            if (value != _break)
            {
                _break = value;
                OnPropertyChanged();
            }
        }
    }

    public bool ZvysenyTok
    {
        get => _zvysenyTok;
        set
        {
            if (value != _zvysenyTok)
            {
                _zvysenyTok = value;
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
            AktualnaReplikacia = "-/-";
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
        PocetReplikacii = "10_000";
        PocetObsluznychMiest = "14";
        PocetPokladni = "6";
        PauseButtonText = "Pause";
        CutFirst = "200";
        SliderValue = 120;

        AktualnaReplikacia = "-/-";
        PocetZakaznikov = "-/-";
        PriemernyCasPredAutomatom = "-/-";
        PriemernyCasVObchode = "-/-";
        PriemernyPocetZakaznikov = "-/-";
        PriemernyOdchodPoslednehoZakaznika = "-/-";
        PriemernyPocetObsluzenychZakaznikov = "-/-";
        PriemerneVytazenieAutomatu = "-/-";
        PriemerneDlzkyRadovPredObsluhov = "[-/-], [-/-], [-/-]";
        PriemerneDlzkyRadovPredPokladnami = "[-/-]...";
        PriemerneVytazeniePokladni = "[-/-]...";
        PriemerneVytazenieObsluhyOnline = "[-/-]...";
        PriemerneVytazenieObsluhyOstatne = "[-/-]";

        RadaPredAutomatom = "-/-";
        RadaPredObsluznimiMiestamiOnline = "-/-";
        RadaPredObsluznimiMiestamiBasic = "-/-";
        RadaPredObsluznimiMiestamiZmluvny = "-/-";
        Automat = new();
        SimulationTime = "-/-";
        SlowDown = false;
        Break = false;
        ZvysenyTok = false;
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
        StopModel(); // ak beží jadro a ja znovu naštartujem tak stopnem pôvodné.
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
            AktualnaReplikacia = "-/-";
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
                    Title = "5 Pokladní",
                    Values = new ChartValues<double>{},
                    Fill = Brushes.Transparent,
                    //PointGeometry = null
                },
                new LineSeries
                {
                    Title = "6 Pokladní",
                    Values = new ChartValues<double>{},
                    Fill = Brushes.Transparent,
                    //PointGeometry = null
                }
            };

            _behZavislotiCore1 = new (pocetObsluznychMiest, 2)
            {
                SlowDown = false,
                BehZavislosti = true,
                Break = _break,
                ZvysenyTok = _zvysenyTok
            };
            _behZavislotiCore1.OnReplicationDidFinish(simulation =>
            {
                if (simulation.CurrentReplication < cutFirst)
                {
                    return;
                }
            
                int stepSize = pocetReplikacii / Constants.POCET_DAT_V_GRAFE;
                if (stepSize >= 2)
                {
                    if (simulation.CurrentReplication % stepSize == 0)
                    {
                        BehZavislotiUpdateUi1(((MySimulation)simulation).GetUIData((MySimulation)simulation));
                    }    
                }
            });
            _behZavislotiCore1.OnSimulationDidFinish(simulation =>
            {
                BehZavislotiUpdateUi1(((MySimulation)simulation).GetUIData((MySimulation)simulation));
                StopModel();
            });
            _behZavislotiCore2 = new (pocetObsluznychMiest, 3)
            {
                SlowDown = false,
                BehZavislosti = true,
                Break = _break,
                ZvysenyTok = _zvysenyTok
            };
            _behZavislotiCore2.OnReplicationDidFinish(simulation =>
            {
                if (simulation.CurrentReplication < cutFirst)
                {
                    return;
                }
            
                int stepSize = pocetReplikacii / Constants.POCET_DAT_V_GRAFE;
                if (stepSize >= 2)
                {
                    if (simulation.CurrentReplication % stepSize == 0)
                    {
                        BehZavislotiUpdateUi2(((MySimulation)simulation).GetUIData((MySimulation)simulation));
                    }    
                }
            });
            _behZavislotiCore2.OnSimulationDidFinish(simulation =>
            {
                BehZavislotiUpdateUi2(((MySimulation)simulation).GetUIData((MySimulation)simulation));
                StopModel();
            });
            _behZavislotiCore3 = new (pocetObsluznychMiest, 4)
            {
                SlowDown = false,
                BehZavislosti = true,
                Break = _break,
                ZvysenyTok = _zvysenyTok
            };
            _behZavislotiCore3.OnReplicationDidFinish(simulation =>
            {
                if (simulation.CurrentReplication < cutFirst)
                {
                    return;
                }
            
                int stepSize = pocetReplikacii / Constants.POCET_DAT_V_GRAFE;
                if (stepSize >= 2)
                {
                    if (simulation.CurrentReplication % stepSize == 0)
                    {
                        BehZavislotiUpdateUi3(((MySimulation)simulation).GetUIData((MySimulation)simulation));
                    }    
                }
            });
            _behZavislotiCore3.OnSimulationDidFinish(simulation =>
            {
                BehZavislotiUpdateUi3(((MySimulation)simulation).GetUIData((MySimulation)simulation));
                StopModel();
            });
            _behZavislotiCore4 = new (pocetObsluznychMiest, 5)
            {
                SlowDown = false,
                BehZavislosti = true,
                Break = _break,
                ZvysenyTok = _zvysenyTok
            };
            _behZavislotiCore4.OnReplicationDidFinish(simulation =>
            {
                if (simulation.CurrentReplication < cutFirst)
                {
                    return;
                }
            
                int stepSize = pocetReplikacii / Constants.POCET_DAT_V_GRAFE;
                if (stepSize >= 2)
                {
                    if (simulation.CurrentReplication % stepSize == 0)
                    {
                        BehZavislotiUpdateUi4(((MySimulation)simulation).GetUIData((MySimulation)simulation));
                    }    
                }
            });
            _behZavislotiCore4.OnSimulationDidFinish(simulation =>
            {
                BehZavislotiUpdateUi4(((MySimulation)simulation).GetUIData((MySimulation)simulation));
                StopModel();
            });
            _behZavislotiCore5 = new ( pocetObsluznychMiest, 6)
            {
                SlowDown = false,
                BehZavislosti = true,
                Break = _break,
                ZvysenyTok = _zvysenyTok
            };
            _behZavislotiCore5.OnReplicationDidFinish(simulation =>
            {
                if (simulation.CurrentReplication < cutFirst)
                {
                    return;
                }
            
                int stepSize = pocetReplikacii / Constants.POCET_DAT_V_GRAFE;
                if (stepSize >= 2)
                {
                    if (simulation.CurrentReplication % stepSize == 0)
                    {
                        BehZavislotiUpdateUi5(((MySimulation)simulation).GetUIData((MySimulation)simulation));
                    }    
                }
            });
            _behZavislotiCore5.OnSimulationDidFinish(simulation =>
            {
                BehZavislotiUpdateUi5(((MySimulation)simulation).GetUIData((MySimulation)simulation));
                StopModel();
            });

            Task.Run(() => _behZavislotiCore1.Simulate(pocetReplikacii));
            Task.Run(() => _behZavislotiCore2.Simulate(pocetReplikacii));
            Task.Run(() => _behZavislotiCore3.Simulate(pocetReplikacii));
            Task.Run(() => _behZavislotiCore4.Simulate(pocetReplikacii));
            Task.Run(() => _behZavislotiCore5.Simulate(pocetReplikacii));
            return;
        }
        
        // --------------------------------------- Klasicka simulácia ------------------------------------------
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

        List<ReklamaciaModel> tmpReklamacie = new(Constants.POCET_PRACOVNIKOV_REKLAMACIE);
        for (int i = 0; i < Constants.POCET_PRACOVNIKOV_REKLAMACIE; i++)
        {
            tmpReklamacie.Add(new());
        }

        Reklamacie = new(tmpReklamacie);
        PauseButtonText = "Pause";
        
        _core = new(pocetObsluznychMiest, pocetPokladni);
        _core.OnReplicationDidFinish(simulation =>
        {
            if (simulation.CurrentReplication % 100 == 0)
            {
                UpdateUI(((MySimulation)simulation).GetUIData((MySimulation)simulation));
            }
        });
        _core.OnSimulationDidFinish(simulation =>
        {
            UpdateUI(((MySimulation)simulation).GetUIData((MySimulation)simulation));
            StopModel();
        });

        _core.OnReplicationWillStart(simulation =>
        {
            if (SlowDown)
            {
                _core.SetSimSpeed(_sliderValue / Constants.POCET_UPDATOV_ZA_SEKUNDU, 1.0 / Constants.POCET_UPDATOV_ZA_SEKUNDU);
            }
        });
        
        _core.OnRefreshUI(simulation =>
        {
            UpdateUI(((MySimulation)simulation).GetUIData((MySimulation)simulation));
        });

        //aby sa pri spustení spustil pomalý beh
        if (_slowDown)
        {
            _core.SlowDown = true;
            _core.SetSimSpeed(_sliderValue / Constants.POCET_UPDATOV_ZA_SEKUNDU, 1.0 / Constants.POCET_UPDATOV_ZA_SEKUNDU);
        }

        _core.Break = _break;
        _core.ZvysenyTok = _zvysenyTok;
        
        Task.Run(() => _core.Simulate(pocetReplikacii));
    }

    private void StopModel()
    {
        if (_core is not null)
        {
            _core.StopSimulation();
            PauseButtonText = "Pause";
        }

        if (_behZavislotiCore1 is not null)
        {
            _behZavislotiCore1.StopSimulation();
        }

        if (_behZavislotiCore2 is not null)
        {
            _behZavislotiCore2.StopSimulation();
        }

        if (_behZavislotiCore3 is not null)
        {
            _behZavislotiCore3.StopSimulation();
        }

        if (_behZavislotiCore4 is not null)
        {
            _behZavislotiCore4.StopSimulation();
        }

        if (_behZavislotiCore5 is not null)
        {
            _behZavislotiCore5.StopSimulation();
        }
    }

    private void PauseModel()
    {
        if (_core is not null)
        {
            if (_core.IsRunning())
            {
                if (_core.IsPaused())
                {
                    _core.ResumeSimulation();
                    PauseButtonText = "Pause";
                }
                else
                {
                    _core.PauseSimulation();
                    PauseButtonText = "Continue";
                }
            }
        }

        if (_behZavislotiCore1 is not null)
        {
            if (_behZavislotiCore1.IsRunning())
            {
                if (_behZavislotiCore1.IsPaused())
                {
                    _behZavislotiCore1.ResumeSimulation();
                    PauseButtonText = "Pause";
                }
                else
                {
                    _behZavislotiCore1.PauseSimulation();
                    PauseButtonText = "Continue";
                }
            }
        }
        if (_behZavislotiCore2 is not null)
        {
            if (_behZavislotiCore2.IsRunning())
            {
                if (_behZavislotiCore2.IsPaused())
                {
                    _behZavislotiCore2.ResumeSimulation();
                }
                else
                {
                    _behZavislotiCore2.PauseSimulation();
                }
            }
        }
        if (_behZavislotiCore3 is not null)
        {
            if (_behZavislotiCore3.IsRunning())
            {
                if (_behZavislotiCore3.IsPaused())
                {
                    _behZavislotiCore3.ResumeSimulation();
                }
                else
                {
                    _behZavislotiCore3.PauseSimulation();
                }
            }
        }
        if (_behZavislotiCore4 is not null)
        {
            if (_behZavislotiCore4.IsRunning())
            {
                if (_behZavislotiCore4.IsPaused())
                {
                    _behZavislotiCore4.ResumeSimulation();
                }
                else
                {
                    _behZavislotiCore4.PauseSimulation();
                }
            }
        }
        if (_behZavislotiCore5 is not null)
        {
            if (_behZavislotiCore5.IsRunning())
            {
                if (_behZavislotiCore5.IsPaused())
                {
                    _behZavislotiCore5.ResumeSimulation();
                }
                else
                {
                    _behZavislotiCore5.PauseSimulation();
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
                _core.SetSimSpeed(_sliderValue / Constants.POCET_UPDATOV_ZA_SEKUNDU, 1.0 / Constants.POCET_UPDATOV_ZA_SEKUNDU);
            }
            else
            {
                _core.SlowDown = false;
                _core.SetMaxSimSpeed();
            }
        }
    }

    private void SetSimulaciaSpeed()
    {
        if (_core is not null)
        {
            _core.SetSimSpeed(_sliderValue / Constants.POCET_UPDATOV_ZA_SEKUNDU, 1.0 / Constants.POCET_UPDATOV_ZA_SEKUNDU);
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
                _core.StopSimulation();
            }
        }
        else
        {
            BehZavislotiVisibility = Visibility.Collapsed;
            BehZavislotiVisibilityOstatne = Visibility.Visible;
            SetReplicationDetailVisibilit(SlowDown);
        }
    }

    private void UpdateUI(DataStructure e)
    {
        // pri pomalom behu update čas aj keď nie je žiadna aktivita -> plynulejší beh
        if (e.ShallowUpdate)
        {
            SimulationTime = e.SimulationTime;
        }

        if (e.NewData)
        {
            e.NewData = false;
            if (e.ShallowUpdate)
            {
                Peoples = new(e.People);
                RadaPredAutomatom = e.RadaPredAutomatom;
                Automat.Update(e.AutomatObsah, e.AutomatObsadeny);
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
                }

                for (int i = 0; i < e.Reklamacia.Count; i++)
                {
                    Reklamacie[i].Update(e.Reklamacia[i]);
                }
            }

            AktualnaReplikacia = e.AktuaReplikacia;
            PriemernyCasVObchode = e.PriemernyCasVObhchode;
            PriemernyCasPredAutomatom = e.PriemernyCasPredAutomatom;
            PocetZakaznikov = e.PriemernaDlzkaraduPredAutomatom;
            PriemernyOdchodPoslednehoZakaznika = e.PriemernyOdchodPoslednehoZakaznika;
            PriemernyPocetZakaznikov = e.PriemernyPocetZakaznikov;
            PriemernyPocetObsluzenychZakaznikov = e.PriemernyPocetObsluzenychZakaznikov;
            PriemerneVytazenieAutomatu = e.PriemerneVytazenieAutomatu;
            PriemerneDlzkyRadovPredObsluhov = e.PriemerneDlzkyRadovPredObsluhov;
            PriemerneDlzkyRadovPredPokladnami = e.PriemerneDlzkyRadovPredPokladnami;
            PriemerneVytazeniePokladni = e.PriemerneVytazeniePokladni;
            PriemerneVytazenieObsluhyOnline = e.PriemerneVytazenieObsluhyOnline;
            PriemerneVytazenieObsluhyOstatne = e.PriemerneVytazenieObsluhyOstatne;
        }
    }

    /*
    private void UpdateUI(object? sender, DataStructure e)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            // pri pomalom behu update čas aj keď nie je žiadna aktivita -> plynulejší beh
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

                    // nahradím novým listom keď je nový zoznam kratší ako predchádzajúci
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
                PocetZakaznikov = e.PriemernaDlzkaraduPredAutomatom;
                PriemernyOdchodPoslednehoZakaznika = e.PriemernyOdchodPoslednehoZakaznika;
                PriemernyPocetZakaznikov = e.PriemernyPocetZakaznikov;
                PriemernyPocetObsluzenychZakaznikov = e.PriemernyPocetObsluzenychZakaznikov;
                PriemerneVytazenieAutomatu = e.PriemerneVytazenieAutomatu;
                PriemerneDlzkyRadovPredObsluhov = e.PriemerneDlzkyRadovPredObsluhov;
                PriemerneDlzkyRadovPredPokladnami = e.PriemerneDlzkyRadovPredPokladnami;
                PriemerneVytazeniePokladni = e.PriemerneVytazeniePokladni;
                PriemerneVytazenieObsluhyOnline = e.PriemerneVytazenieObsluhyOnline;
                PriemerneVytazenieObsluhyOstatne = e.PriemerneVytazenieObsluhyOstatne;
                IntervalSpolahlivsti = e.IntervalSpolahlivstiCasuVsysteme;
            }
        });
    }
    */

    
    private void BehZavislotiUpdateUi1(DataStructure e)
    {
        AktualnaReplikacia = e.AktuaReplikacia;
        SeriesCollection?[0].Values.Add(e.BehZavislostiPriemernyPocetZakaznikovPredAutomatom);
    }
    private void BehZavislotiUpdateUi2(DataStructure e)
    {
        SeriesCollection?[1].Values.Add(e.BehZavislostiPriemernyPocetZakaznikovPredAutomatom);
    }
    private void BehZavislotiUpdateUi3(DataStructure e)
    {
        SeriesCollection?[2].Values.Add(e.BehZavislostiPriemernyPocetZakaznikovPredAutomatom);
    }
    private void BehZavislotiUpdateUi4(DataStructure e)
    {
        SeriesCollection?[3].Values.Add(e.BehZavislostiPriemernyPocetZakaznikovPredAutomatom);
    }
    private void BehZavislotiUpdateUi5(DataStructure e)
    {
        SeriesCollection?[4].Values.Add(e.BehZavislostiPriemernyPocetZakaznikovPredAutomatom);;
    }
    
    public void OnWindowClosing()
    {
        StopModel();
    }
}