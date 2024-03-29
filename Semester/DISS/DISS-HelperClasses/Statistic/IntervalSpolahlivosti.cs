namespace DISS_HelperClasses.Statistic;

/// <summary>
/// Trieda pre výpočet intrvalu spolahlivosti
/// </summary>
public class IntervalSpolahlivosti : Average
{
    private double _oldMean;
    private double _sumOfSquare;

    public override void AddValue(double pValue)
    {
        Count++;
        _oldMean = base.Calucate();
        base.AddValue(pValue);
        _sumOfSquare += (pValue - _oldMean) * (pValue - base.Calucate());
    }

    // Hodnota musí byť väčšia
    public double Variance => Count > 1 ? _sumOfSquare / (Count - 1) : 0;

    public double StandardDeviation => Math.Sqrt(Variance);

    private new double Calucate()
    {
        return base.Calucate();
    }

    public (double lowerBoud, double upperBound) Calculate(double confidenceLevel)
    {
        if (Count < 2)
        {
            throw new InvalidOperationException("At least two values are required to calculate a confidence interval.");
        }

        var zScore = GetZScore(confidenceLevel);
        var marginOfError = zScore * StandardDeviation / Math.Sqrt(Count);

        return (base.Calucate() - marginOfError, base.Calucate() + marginOfError);
    }

    // Get the Z-score corresponding to the confidence level
    private double GetZScore(double confidenceLevel)
    {
        // Placeholder for the Z-score calculation; you might need an external library or a custom implementation
        // For example, using MathNet.Numerics:
        // return MathNet.Numerics.ExcelFunctions.NORMSINV(1 - (1 - confidenceLevel) / 2);
        throw new NotImplementedException("Z-score calculation needs implementation.");
    }

    public override void Clear()
    {
        base.Clear();
        _oldMean = 0;
    }
}