namespace AbbaNovynovyStanok;

public class ExponentialGenerator
{
    private readonly double _mean;
    private readonly Random _random;

    public ExponentialGenerator(int seed, double mean)
    {
        _mean = mean;
        _random = new Random(seed);
    }

    public double Next()
    {
        return -_mean * Math.Log(1 - _random.NextDouble());
    }
}