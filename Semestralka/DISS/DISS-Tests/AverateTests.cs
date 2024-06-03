using DISS_HelperClasses.Statistic;
using NUnit.Framework.Internal;

namespace DISS_RNG_Tests;

public class AverateTests
{
    private Average _average;
    
    
    [Test]
    public void AverageTest()
    {
        _average = new();
        var n = 100;
        var sumAll = 0.0;
        for (int i = 0; i < n; i++)
        {
            _average.AddValue(i);
            sumAll += i;
        }

        Assert.That(sumAll/n, Is.EqualTo(_average.Calucate())); 
        Assert.That(n, Is.EqualTo(_average.Count));
        _average.Clear();
        Assert.That(_average.Count, Is.EqualTo(0));
    }

    [Test]
    public void AverageExceptionTest()
    {
        _average = new();
        var e = Assert.Throws<InvalidOperationException>(() => _average.Calucate());

        Assert.That(e.Message, Is.EqualTo("Nemôžem počítať priemer 0 dátami")); 
    }
}