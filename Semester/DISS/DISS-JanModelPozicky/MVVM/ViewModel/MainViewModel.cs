using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using DISS.Random.Continous;
using DISS_JanModelPozicky.Core;
using LiveCharts;
using LiveCharts.Wpf;

public class MainViewModel : ObservableObjects
{
	private List<double> listLiveCharts;
    private TestMonteCarlo testMonteCarlo;

	public List<double> ListLiveCharts
	{
		get { return listLiveCharts; }
        set
        {
            listLiveCharts = value;
            OnPropertyChanged();
        }
	}


    private SeriesCollection myVar;

    public SeriesCollection MyProperty
    {
        get { return myVar; }
        set
        {
            myVar = value; 
            OnPropertyChanged();
        }
    }

    private List<string> labels;

    public List<string> Labels
    {
        get { return labels; }
        set
        {
            labels = value;
            OnPropertyChanged();
        }
    }

    private int numberOfRepplication;

    public int NumberOfRelication
    {
        get { return numberOfRepplication; }
        set
        {
            numberOfRepplication = value; 
            OnPropertyChanged();
        }
    }

    private int cutFirst;

    public int CutFirst
    {
        get { return cutFirst; }
        set
        {
            cutFirst = value;
            OnPropertyChanged();
        }
    }


    private string vysledokPokusuJeden;

    public string VysledokPokusuJeden
    {
        get { return vysledokPokusuJeden; }
        set
        {
            vysledokPokusuJeden = value;
            OnPropertyChanged();
        }
    }

    private string vysledokPokusuDva;

    public string VysledokPokusuDva
    {
        get { return vysledokPokusuDva; }
        set
        {
            vysledokPokusuDva = value;
            OnPropertyChanged();
        }
    }

    private string vysledokPokusuTri;

    public string VysledokPokusuTri
    {
        get { return vysledokPokusuTri; }
        set
        {
            vysledokPokusuTri = value;
            OnPropertyChanged();
        }
    }




    public RelayCommand StartCommand { get; set; }
    public RelayCommand StopCommand { get; set; }


    public MainViewModel()
    {
        InicialiseButtons();
        NumberOfRelication = 10000000;
        cutFirst = 200000;
        double toPrintZeros = 0.0;
        VysledokPokusuJeden = $"1. pokus: {toPrintZeros}";
        VysledokPokusuDva = $"2. pokus: {toPrintZeros}";
        VysledokPokusuTri = $"3. pokus: {toPrintZeros}";

        /*
        listLiveCharts = new();
        List<double> temp = new(1000);
        Uniform generator = new(0.0, 100.0);
        for (int i = 0; i < 1000; i++)
        {
            temp.Add(generator.Next());
        }
        ListLiveCharts = new(temp);
        */
        /*
        List<double> displayData = ListLiveCharts
            .Select((value, index) => new { value, index })
            .Where(x => x.index % 10 == 0)
            .Select(x => x.value)
            .ToList();

        var tmp = new List<string>();
        for (int i = 0; i < ListLiveCharts.Count; i++)
        {
            tmp.Add(i.ToString() + " M");
        }
        Labels = new(tmp);
        */
        /*
        List<double> displayData = new();
        List<string> tmpLabels = new();

        int step = (int)(ListLiveCharts.Count * 0.01);

        for (int i = 0; i < ListLiveCharts.Count; i += step)
        {
            displayData.Add(ListLiveCharts[i]);
            tmpLabels.Add(i.ToString() + " M");
        }

        Labels = tmpLabels;

        MyProperty = new SeriesCollection
        {
            new LineSeries
            {
                Title = "Seria 1",
                Values = new ChartValues<double>(displayData),
                Fill = Brushes.Transparent,
                //PointGeometry = null
            }
        };
        */


        //VisibilityCharts = Visibility.Visible;

    }

    private void InicialiseButtons()
    {
        StartCommand = new RelayCommand(o => StartMonteCarlo());
        StopCommand = new RelayCommand(o => StopMonteCarlo());
    }

    private void StartMonteCarlo()
    {
        testMonteCarlo = new(NumberOfRelication, cutFirst);
        testMonteCarlo.Vysledky.CollectionChanged += Vysledky_CollectionChanged;
        testMonteCarlo.Run();
    }

    private void StopMonteCarlo()
    {
        testMonteCarlo.Stop();
    }

    private void Vysledky_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            List<double> displayData = new();
            List<string> tmpLabels = new();

            int step = (int)(testMonteCarlo.Vysledky.Count * 0.1);
            if (step < 1)
            {
                step = 1;
            }

            //for (int i = 0; i < testMonteCarlo.Vysledky.Count; i += step)
            for (int i = 0; i < testMonteCarlo.Vysledky.Count; i++)
            {
                displayData.Add(testMonteCarlo.Vysledky[i].First);
                tmpLabels.Add(testMonteCarlo.Vysledky[i].Second.ToString());
            }

            Labels = tmpLabels;

            if (MyProperty is null)
            {
                MyProperty = new SeriesCollection
                {
                    new LineSeries
                    {
                        Title = "Seria 1",
                        Values = new ChartValues<double>(displayData),
                        Fill = Brushes.Transparent,
                        //PointGeometry = null
                    }
                };
                if (displayData.Count >= 1)
                {
                    VysledokPokusuJeden = $"1. Pokus: {displayData.Last()}";
                }
            }
            else
            {
                MyProperty[0].Values.Add(displayData.Last());
                VysledokPokusuJeden = $"1. Pokus: {displayData.Last()}";
            }

        });
    }
}
