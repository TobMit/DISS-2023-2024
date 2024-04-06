namespace DISS_HelperClasses.Statistic;

public class WeightedAverage : Average
{
    private List<Pair<double, int>> _listOfData;

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
        double integral = 0;
        Pair<double, int> lastTime = new(0,0);
        foreach (var data in _listOfData)
        {
            integral += (data.First - lastTime.First)*((double)(data.Second + lastTime.Second)/ 2);
            lastTime = data;
        }
        return integral / totalTime;
    }

    // new sková metódu pred okolitým svetom
    public void AddValue(double pValue, int dlzka)
    {
        _listOfData.Add(new Pair<double, int>(pValue, dlzka));
    }
    
    public override void AddValue(double pValue)
    {
        //_listOfData.Add(pValue);
    }

    public override void Clear()
    {
        base.Clear();
        _listOfData.Clear();
    }
}