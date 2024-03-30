using DISS_HelperClasses.Statistic;

namespace DISS_RNG_Tests;

public class WeightedAverageTests
{
    private WeightedAverage _average;
    
    [Test]
    public void AverageTest()
    {
        _average = new();
        var n = 10;
        for (int i = 0; i < n; i++)
        {
            _average.AddValue(i, i);
        }

        Assert.That(Double.Round(_average.Calucate(),4), Is.EqualTo(Double.Round(6.33333333, 4))); 
        Assert.That(_average.Count, Is.EqualTo(n));
        _average.Clear();
        //Assert.That(_average.Count, Is.EqualTo(0));
    }

    [Test]
    public void AverageExceptionTest()
    {
        _average = new();
        var e = Assert.Throws<InvalidOperationException>(() => _average.Calucate());

        Assert.That(e.Message, Is.EqualTo("Nemôžem počítať priemer s nulovou váhou"));

        e = Assert.Throws<InvalidOperationException>(() => _average.AddValue(4));
        Assert.That(e.Message, Is.EqualTo("Táto funkcia nieje podporovaná pre vážený priemer"));
    }
}