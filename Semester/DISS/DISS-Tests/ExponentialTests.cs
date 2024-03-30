using DISS.Random.Other;

namespace DISS_RNG_Tests;

public class ExponentialTests
{
    [Test]
    public void TestContinuousUniform()
    {
        var rng = new Exponential(0.2, 0);
        var rng2 = new Exponential(0.2, 0);
        for (int i = 0; i < 100; i++)
        {
            Assert.That(rng2.Next(), Is.EqualTo(rng.Next()));
        }
    }
}