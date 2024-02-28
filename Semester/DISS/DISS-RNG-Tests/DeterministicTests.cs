using DISS.Random.Other;

namespace DISS_RNG_Tests;

public class DeterministicTests
{
    [Test]
    public void TestDeterministic()
    {
        var rng = new Deterministic<int>(5);
        var rng2 = new Deterministic<double>(6.6);
        Assert.That(rng.Next(), Is.EqualTo(5));
        Assert.That(rng2.Next(), Is.EqualTo(6.6));
    }
}