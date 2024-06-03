using DISS.Random.Other;

namespace DISS_RNG_Tests;

public class TriangularTests
{
    [Test]
    public void TestContinuousUniform()
    {
        var rng = new Triangular(0, 5, 10, 0);
        var rng2 = new Triangular(0,5, 10, 0);
        for (int i = 0; i < 100; i++)
        {
            Assert.That(rng2.Next(), Is.EqualTo(rng.Next()));
        }
    }
    
    [Test]
    public void TestRange()
    {
        const double min = -5.3;
        const double max = 15.6;
        var rng = new Triangular(min, 0, max);
        for (int i = 0; i < 1000000; i++)
        {
            var tmp = rng.Next();
            Assert.That(tmp, Is.GreaterThanOrEqualTo(min));
            Assert.That(tmp, Is.LessThan(max));
        }
    }
}