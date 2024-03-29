namespace DISS_HelperClasses.Statistic;

public class WeightedAverage : Average
{
    public double SumOfWeightedValues { get; private set; }
    public override double Calucate()
    {
        if (SumOfWeightedValues == 0)
        {
            throw new InvalidOperationException("Nemôžem počítať priemer s nulovou váhou");
        }

        return SumOfWeightedValues / SumAll;
    }
    
    // new sková metódu pred okolitým svetom
    private new void AddValue(double pValue)
    {
        throw new Exception("Táto funkcia nieje podporovaná pre vážený priemer");
    }

    public void AddValue(double pValue, double pWeight)
    {
        SumOfWeightedValues += pValue * pWeight;
        SumAll += pWeight;
        Count += 0;
    }
    
    public override void Clear()
    {
        base.Clear();
        SumOfWeightedValues = 0.0;
    }
}