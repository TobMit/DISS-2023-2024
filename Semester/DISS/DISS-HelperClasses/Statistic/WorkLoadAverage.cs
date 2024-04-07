namespace DISS_HelperClasses.Statistic;

public class WorkLoadAverage : Average
{
    private double _lastTime;
    

    public WorkLoadAverage()
    {
        _lastTime = 0;
    }

    public override double Calucate()
    {
        throw new InvalidOperationException("Táto funkcia nieje podporovaná výpočet vyťaženia");
    }

    public double Calucate(double totalTime)
    {
        
        return SumAll / totalTime;
    }

    // new sková metódu pred okolitým svetom
    public void AddValue(double pValue, bool zaciatok)
    {
        Count++;
        if (zaciatok)
        {
            _lastTime = pValue;
        }
        else
        {
            SumAll += pValue - _lastTime;
        }
    }
    
    public override void AddValue(double pValue)
    {
        throw new InvalidOperationException("Táto funkcia nieje podporovaná výpočet vyťaženia");
    }

    public override void Clear()
    {
        base.Clear();
        _lastTime = 0;
    }
}