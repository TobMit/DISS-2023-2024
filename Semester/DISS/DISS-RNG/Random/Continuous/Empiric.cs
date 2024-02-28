namespace DISS.Random.Continous;

/// <summary>
/// Empirické spojité rozdlelenie
/// </summary>
public class Empiric : EmpiricBase<double>
{
    private List<Continous.Uniform> _uniformGenerators;

    public Empiric(List<EmpiricData<double>> pListOfValues) : base(pListOfValues)
    {
        _listOfValuse = new(pListOfValues.Count);
        _uniformGenerators = new(pListOfValues.Count);

        //vytvorenie kumulativnej pravdepodobnosti
        double cumulativeProbability = 0.0;
        foreach (var dataValue in pListOfValues)
        {
            EmpiricData<double> tmp = new(dataValue.Range, dataValue.Probability + cumulativeProbability);
            _listOfValuse.Add(tmp);
            _uniformGenerators.Add(new(dataValue.Range.First, dataValue.Range.Second));

            cumulativeProbability += dataValue.Probability;
        }
    }
    
    public Empiric(List<EmpiricDataWithSeed<double>> pListOfValues, int pSeed) : base(pListOfValues, pSeed)
    {
        _listOfValuse = new(pListOfValues.Count);
        _uniformGenerators = new(pListOfValues.Count);

        //vytvorenie kumulativnej pravdepodobnosti
        double cumulativeProbability = 0.0;
        foreach (var dataValue in pListOfValues)
        {
            EmpiricData<double> tmp = new(dataValue.Range, dataValue.ProbabilitySeed.First + cumulativeProbability);
            _listOfValuse.Add(tmp);
            _uniformGenerators.Add(new(dataValue.Range.First, dataValue.Range.Second, dataValue.ProbabilitySeed.Second));

            cumulativeProbability += dataValue.ProbabilitySeed.First;
        }
    }

    public override double Next()
    {
        var classProbability = generator.NextDouble();
        for (int i = 0; i < _listOfValuse.Count; i++)
        {
            if (classProbability <= _listOfValuse[i].Probability)
            {
                return _uniformGenerators[i].Next();
            }
        }

        throw new Exception("Toto nemalo nastať. Nepodarilo sa vygenerovať hodnotu v Continous.Empirical rozdelení");
        return _uniformGenerators[^1].Next(); // posledna hodnota v zozname
    }
}