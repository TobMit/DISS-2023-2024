using DISS.Random;
using EmpiricC = DISS.Random.Continous.Empiric;
using EmpiricD = DISS.Random.Discrete.Empiric;

namespace DISS_RNG_Tests;

public class EmpiricTests
{
    [Test]
    public void TestContinousEmpiricThrow()
    {
        List<EmpiricBase<double>.EmpiricData<double>> datas = new();
        datas.Add(new EmpiricBase<double>.EmpiricData<double>(1.3, 2.3, 0.5));
        datas.Add(new EmpiricBase<double>.EmpiricData<double>(2.3, 3.3, 0.2));
        
        var ex = Assert.Throws<ArgumentException>(() => new EmpiricC(datas));
        Assert.That(ex.Message, Is.EqualTo("Sum of probabilities is not equal to 1"));
        
        datas.Add(new EmpiricBase<double>.EmpiricData<double>(1.3, 2.3, 0.2));
        datas.Add(new EmpiricBase<double>.EmpiricData<double>(2.3, 3.3, 0.2));
        
        ex = Assert.Throws<ArgumentException>(() => new EmpiricC(datas));
        Assert.That(ex.Message, Is.EqualTo("Sum of probabilities is not equal to 1"));
    }
    
    [Test]
    public void TestDiscreteEmpiricThrow()
    {
        List<EmpiricBase<int>.EmpiricData<int>> datas = new();
        datas.Add(new EmpiricBase<int>.EmpiricData<int>(1, 2, 0.5));
        datas.Add(new EmpiricBase<int>.EmpiricData<int>(2, 3, 0.2));
        
        var ex = Assert.Throws<ArgumentException>(() => new EmpiricD(datas));
        Assert.That(ex.Message, Is.EqualTo("Sum of probabilities is not equal to 1"));
        
        datas.Add(new EmpiricBase<int>.EmpiricData<int>(1, 2, 0.2));
        datas.Add(new EmpiricBase<int>.EmpiricData<int>(2, 3, 0.2));
        
        ex = Assert.Throws<ArgumentException>(() => new EmpiricD(datas));
        Assert.That(ex.Message, Is.EqualTo("Sum of probabilities is not equal to 1"));
    }
    
    [Test]
    public void TestContinousEmpiricThrowSeed()
    {
        List<EmpiricBase<double>.EmpiricDataWithSeed<double>> datas = new();
        datas.Add(new EmpiricBase<double>.EmpiricDataWithSeed<double>(1.3, 2.3, 0.5, 1));
        datas.Add(new EmpiricBase<double>.EmpiricDataWithSeed<double>(2.3, 3.3, 0.2, 2));
        
        var ex = Assert.Throws<ArgumentException>(() => new EmpiricC(datas, 1));
        Assert.That(ex.Message, Is.EqualTo("Sum of probabilities is not equal to 1"));
        
        datas.Add(new EmpiricBase<double>.EmpiricDataWithSeed<double>(1.3, 2.3, 0.2, 3));
        datas.Add(new EmpiricBase<double>.EmpiricDataWithSeed<double>(2.3, 3.3, 0.2, 4));
        
        ex = Assert.Throws<ArgumentException>(() => new EmpiricC(datas, 1));
        Assert.That(ex.Message, Is.EqualTo("Sum of probabilities is not equal to 1"));
    }
    
    [Test]
    public void TestDiscreteEmpiricThrowSeed()
    {
        List<EmpiricBase<int>.EmpiricDataWithSeed<int>> datas = new();
        datas.Add(new EmpiricBase<int>.EmpiricDataWithSeed<int>(1, 2, 0.5, 1));
        datas.Add(new EmpiricBase<int>.EmpiricDataWithSeed<int>(2, 3, 0.2, 2));
        
        var ex = Assert.Throws<ArgumentException>(() => new EmpiricD(datas, 1));
        Assert.That(ex.Message, Is.EqualTo("Sum of probabilities is not equal to 1"));
        
        datas.Add(new EmpiricBase<int>.EmpiricDataWithSeed<int>(1, 2, 0.2, 3));
        datas.Add(new EmpiricBase<int>.EmpiricDataWithSeed<int>(2, 3, 0.2, 4));
        
        ex = Assert.Throws<ArgumentException>(() => new EmpiricD(datas, 1));
        Assert.That(ex.Message, Is.EqualTo("Sum of probabilities is not equal to 1"));
    }
    
    [Test]
    public void TestContinousEmpiric()
    {
        List<EmpiricBase<double>.EmpiricDataWithSeed<double>> datas = new();
        datas.Add(new EmpiricBase<double>.EmpiricDataWithSeed<double>(1.3, 2.3, 0.5, 0));
        datas.Add(new EmpiricBase<double>.EmpiricDataWithSeed<double>(2.3, 3.3, 0.2, 0));
        datas.Add(new EmpiricBase<double>.EmpiricDataWithSeed<double>(3.3, 4.3, 0.3, 0));
        
        var empiric1 = new EmpiricC(datas,0);
        var empiric2 = new EmpiricC(datas,0);

        for (int i = 0; i < 1000000; i++)
        {
            Assert.That(empiric1.Next(), Is.EqualTo(empiric2.Next()));
        }
    }
    
    [Test]
    public void TestDiscreteEmpiric()
    {
        List<EmpiricBase<int>.EmpiricDataWithSeed<int>> datas = new();
        datas.Add(new EmpiricBase<int>.EmpiricDataWithSeed<int>(1, 2, 0.5, 0));
        datas.Add(new EmpiricBase<int>.EmpiricDataWithSeed<int>(2, 3, 0.2, 0));
        datas.Add(new EmpiricBase<int>.EmpiricDataWithSeed<int>(3, 4, 0.3, 0));
        
        var empiric1 = new EmpiricD(datas,0);
        var empiric2 = new EmpiricD(datas,0);

        for (int i = 0; i < 1000000; i++)
        {
            Assert.That(empiric1.Next(), Is.EqualTo(empiric2.Next()));
        }
    }
    
    [Test]
    public void TestContinousEmpiricSeed()
    {
        List<EmpiricBase<double>.EmpiricDataWithSeed<double>> datas = new();
        datas.Add(new EmpiricBase<double>.EmpiricDataWithSeed<double>(1.3, 2.3, 0.5, 1));
        datas.Add(new EmpiricBase<double>.EmpiricDataWithSeed<double>(2.3, 3.3, 0.2, 2));
        datas.Add(new EmpiricBase<double>.EmpiricDataWithSeed<double>(3.3, 4.3, 0.3, 3));
        
        var empiric1 = new EmpiricC(datas,1);

        for (int i = 0; i < 1000000; i++)
        {
            var tmp = empiric1.Next();
            Assert.That(tmp, Is.GreaterThanOrEqualTo(1.3));
            Assert.That(tmp, Is.LessThan(4.3));
        }
    }
    
    [Test]
    public void TestDiscreteEmpiricSeed()
    {
        List<EmpiricBase<int>.EmpiricDataWithSeed<int>> datas = new();
        datas.Add(new EmpiricBase<int>.EmpiricDataWithSeed<int>(1, 2, 0.5, 1));
        datas.Add(new EmpiricBase<int>.EmpiricDataWithSeed<int>(3, 4, 0.2, 2));
        datas.Add(new EmpiricBase<int>.EmpiricDataWithSeed<int>(4, 5-1, 0.3, 3));
        
        var empiric1 = new EmpiricD(datas,1);

        for (int i = 0; i < 1000000; i++)
        {
            var tmp = empiric1.Next();
            Assert.That(tmp, Is.GreaterThanOrEqualTo(1));
            Assert.That(tmp, Is.LessThan(5));
        }
    }
}