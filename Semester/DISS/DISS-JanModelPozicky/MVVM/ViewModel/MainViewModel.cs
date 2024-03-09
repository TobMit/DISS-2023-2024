using System;
using System.Collections.Generic;
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

	public List<double> ListLiveCharts
	{
		get { return listLiveCharts; }
        set
        {
            listLiveCharts = value;
            OnPropertyChanged("ListLiveCharts");
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



    public RelayCommand Start { get; set; }


    public MainViewModel()
    {
        InicialiseButtons();
        listLiveCharts = new();
        List<double> temp = new(1000);
        Uniform generator = new(0.0, 100.0);
        for (int i = 0; i < 1000; i++)
        {
            temp.Add(generator.Next());
        }
        ListLiveCharts = new(temp);

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



        //VisibilityCharts = Visibility.Visible;

    }

    private void InicialiseButtons()
    {
        //Start = new RelayCommand(o => VisibilityCharts = Visibility.Visible);
    }
}
