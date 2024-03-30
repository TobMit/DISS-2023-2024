namespace DISS_HelperClasses.Statistic;

/// <summary>
/// Vypočíta klasický priemer
/// </summary>
public class Average
{
    /// <summary>
    /// Počet hodnôt v štatistike
    /// </summary>
    public int Count { get; protected set; }
    protected double SumAll { get; set; }

    public Average()
    {
        Count = 0;
        SumAll = 0.0;
    }

    /// <summary>
    /// Pridá hodnotu to priemeru
    /// </summary>
    /// <param name="pValue">pripočitávaná hodnota</param>
    public virtual void AddValue(double pValue)
    {
        Count++;
        SumAll += pValue;
    }

    /// <summary>
    /// Prepočíta na základe aktuálnych údajov v šturktúre
    /// </summary>
    /// <returns>Vypočítanú hodnotu</returns>
    public virtual double Calucate()
    {
        if (Count <= 0)
        {
            throw new InvalidOperationException("Nemôžem počítať priemer 0 dátami");
        }
        return SumAll / Count;
    }

    /// <summary>
    /// Vyčistí štatistiku
    /// </summary>
    public virtual void Clear()
    {
        SumAll = 0.0;
        Count = 0;
    }
}