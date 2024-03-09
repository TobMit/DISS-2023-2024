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
    private ModelFirstVariant modelFirstVariant;
    private ModelSecondVariant modelSecondVariant;
    private ModelThirdVariant modelThirdVariant;


    private SeriesCollection seriesCollectionVariantA;

    public SeriesCollection SeriesCollectionVariantA
    {
        get { return seriesCollectionVariantA; }
        set
        {
            seriesCollectionVariantA = value;
            OnPropertyChanged();
        }
    }

    private List<string> labelsVariantA;

    public List<string> LabelsVariantA
    {
        get { return labelsVariantA; }
        set
        {
            labelsVariantA = value;
            OnPropertyChanged();
        }
    }

    private SeriesCollection seriesCollectionVariantB;

    public SeriesCollection SeriesCollectionVariantB
    {
        get { return seriesCollectionVariantB; }
        set
        {
            seriesCollectionVariantB = value;
            OnPropertyChanged();
        }
    }

    private List<string> labelsVariantB;

    public List<string> LabelsVariantB
    {
        get { return labelsVariantB; }
        set
        {
            labelsVariantB = value;
            OnPropertyChanged();
        }
    }

    private SeriesCollection seriesCollectionVariantC;

    public SeriesCollection SeriesCollectionVariantC
    {
        get { return seriesCollectionVariantC; }
        set
        {
            seriesCollectionVariantC = value;
            OnPropertyChanged();
        }
    }

    private List<string> labelsVariantC;

    public List<string> LabelsVariantC
    {
        get { return labelsVariantC; }
        set
        {
            labelsVariantC = value;
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

    }

    private void InicialiseButtons()
    {
        StartCommand = new RelayCommand(o => StartMonteCarlo());
        StopCommand = new RelayCommand(o => StopMonteCarlo());
    }

    private void StartMonteCarlo()
    {
        modelFirstVariant = new(numberOfRepplication, cutFirst);
        modelFirstVariant.Vysledky.CollectionChanged += VariantA_CollectionChanged;
        modelSecondVariant = new(numberOfRepplication, cutFirst);
        modelSecondVariant.Vysledky.CollectionChanged += VariantB_CollectionChanged;
        modelThirdVariant = new(numberOfRepplication, cutFirst);
        modelThirdVariant.Vysledky.CollectionChanged += VariantC_CollectionChanged;

        modelFirstVariant.Run();
        modelSecondVariant.Run();
        modelThirdVariant.Run();

    }

    private void StopMonteCarlo()
    {
        //testMonteCarlo.Stop();
        modelFirstVariant.Stop();
        modelSecondVariant.Stop();
        modelThirdVariant.Stop();
    }


    private void VariantA_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            List<double> displayData = new();
            List<string> tmpLabels = new();

            for (int i = 0; i < modelFirstVariant.Vysledky.Count; i++)
            {
                displayData.Add(modelFirstVariant.Vysledky[i].First);
                tmpLabels.Add(modelFirstVariant.Vysledky[i].Second.ToString());
            }

            LabelsVariantA = tmpLabels;

            if (SeriesCollectionVariantA is null)
            {
                SeriesCollectionVariantA = new SeriesCollection
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
                SeriesCollectionVariantA[0].Values.Add(displayData.Last());
                VysledokPokusuJeden = $"1. Pokus: {displayData.Last()}";
            }

        });
    }

    private void VariantB_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            List<double> displayData = new();
            List<string> tmpLabels = new();

            for (int i = 0; i < modelSecondVariant.Vysledky.Count; i++)
            {
                displayData.Add(modelSecondVariant.Vysledky[i].First);
                tmpLabels.Add(modelSecondVariant.Vysledky[i].Second.ToString());
            }

            LabelsVariantB = tmpLabels;

            if (SeriesCollectionVariantB is null)
            {
                SeriesCollectionVariantB = new SeriesCollection
                {
                    new LineSeries
                    {
                        Title = "Seria 2",
                        Values = new ChartValues<double>(displayData),
                        Fill = Brushes.Transparent,
                        //PointGeometry = null
                    }
                };
                if (displayData.Count >= 1)
                {
                    VysledokPokusuDva = $"2. Pokus: {displayData.Last()}";
                }
            }
            else
            {
                SeriesCollectionVariantB[0].Values.Add(displayData.Last());
                VysledokPokusuDva = $"2. Pokus: {displayData.Last()}";
            }

        });
    }

    private void VariantC_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            List<double> displayData = new();
            List<string> tmpLabels = new();

            for (int i = 0; i < modelThirdVariant.Vysledky.Count; i++)
            {
                displayData.Add(modelThirdVariant.Vysledky[i].First);
                tmpLabels.Add(modelThirdVariant.Vysledky[i].Second.ToString());
            }

            LabelsVariantC = tmpLabels;

            if (SeriesCollectionVariantC is null)
            {
                SeriesCollectionVariantC = new SeriesCollection
                {
                    new LineSeries
                    {
                        Title = "Seria 3",
                        Values = new ChartValues<double>(displayData),
                        Fill = Brushes.Transparent,
                        //PointGeometry = null
                    }
                };
                if (displayData.Count >= 1)
                {
                    VysledokPokusuTri = $"3. Pokus: {displayData.Last()}";
                }
            }
            else
            {
                SeriesCollectionVariantC[0].Values.Add(displayData.Last());
                VysledokPokusuTri = $"3. Pokus: {displayData.Last()}";
            }

        });
    }
}
