namespace DISS_HelperClasses.Statistic;

public class WeightedAverage : Average
{
    public double SumOfWeightedValues { get; private set; }

    private double weightSum;
    public override double Calucate()
    {
        if (SumAll == 0)
        {
            return 0;
        }

        if (weightSum == 0)
        {
            throw new InvalidOperationException("Nemôžem počítať priemer s nulovou váhou");
        }

        return SumOfWeightedValues / SumAll;
    }
    
    // new sková metódu pred okolitým svetom
    public override void AddValue(double pValue)
    {
        throw new InvalidOperationException("Táto funkcia nieje podporovaná pre vážený priemer");
    }

    public void AddValue(double pValue, double pWeight)
    {
        SumOfWeightedValues += pValue * pWeight;
        SumAll += pWeight;
        weightSum += pWeight;
        Count++;
    }
    
    public override void Clear()
    {
        base.Clear();
        SumOfWeightedValues = 0.0;
        weightSum = 0;
    }
}