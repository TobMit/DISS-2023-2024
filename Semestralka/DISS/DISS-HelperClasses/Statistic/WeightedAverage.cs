namespace DISS_HelperClasses.Statistic;

public class WeightedAverage : Average
{
    private List<Pair<double, int>> _listOfData;
    private Pair<double, int> lastData;

    public WeightedAverage()
    {
        _listOfData = new();
    }

    public override double Calucate()
    {
        throw new InvalidOperationException("Táto funkcia nieje podporovaná pre vážený priemer");
    }

    public double Calucate(double totalTime)
    {
        return SumAll / totalTime;
    }

    // new sková metódu pred okolitým svetom
    public void AddValue(double pValue, int count)
    {
        if (Count <= 1)
        {
            lastData = new Pair<double, int>(pValue, count);
            Count++;
        }
        else
        {
            SumAll += (pValue - lastData.First) * lastData.Second;
            lastData.First = pValue;
            lastData.Second = count;
        }
    }
    
    public override void AddValue(double pValue)
    {
        throw new InvalidOperationException("Táto funkcia nieje podporovaná pre vážený priemer");
    }

    public override void Clear()
    {
        base.Clear();
        _listOfData.Clear();
    }
}