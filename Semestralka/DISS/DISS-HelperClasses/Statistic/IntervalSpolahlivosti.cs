namespace DISS_HelperClasses.Statistic;

/// <summary>
/// Trieda pre výpočet intervalu spoľahlivosti
/// </summary>
public class IntervalSpolahlivosti : Average
{
    private double sumAllSqaure = 0;

    public override void AddValue(double pValue)
    {
        Count++;
        SumAll += pValue;
        sumAllSqaure += pValue * pValue;
    }

    private double SmerodajnaOdchylka()
    {
        var vrch = (sumAllSqaure - ((SumAll*SumAll)/Count));
        var spodok = (Count - 1);
        return Math.Sqrt(vrch/spodok);
    }
    

    private new double Calucate()
    {
        throw new InvalidOperationException("Táto funkcia nieje podporovaná pre vážený priemer");
    }

    /// <summary>
    /// Vypočíta interval spoľahlivosti
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">Ak nie je dosť dát</exception>
    public (double dolnaHranica, double hornaHranica) Calculate()
    {
        if (Count < 2)
        {
            throw new InvalidOperationException("Minimálne počet dát pre vypočítanie IS je 2");
        }
        
        var chybovost = 1.96 * SmerodajnaOdchylka() / Math.Sqrt(Count);

        return (base.Calucate() - chybovost, base.Calucate() + chybovost);
    }


    public override void Clear()
    {
        base.Clear();
        sumAllSqaure = 0;
    }
}