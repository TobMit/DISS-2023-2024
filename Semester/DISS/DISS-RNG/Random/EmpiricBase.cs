using DISS_HelperClasses;

namespace DISS.Random;

public abstract class EmpiricBase<T> : ExtendedRandom<T> where T : struct
{
    
    /// <summary>
    /// Trieda slúži na naplneneie Empirického rozdelenia
    /// </summary>
    /// <typeparam name="T">Typ numerickej hodnoty</typeparam>
    public class EmpiricData<T> where T : struct
    {
        public Pair<T, T> Range {
            get;
            set;
        }

        public double Probability { get; set; }

        public EmpiricData(T pMin, T pMax, double pProbability)
        {
            Range = new Pair<T, T>(pMin, pMax);
            Probability = pProbability;
        }
        
        public EmpiricData(Pair<T, T> pRange, double pProbability)
        {
            Range = pRange;
            Probability = pProbability;
        }
    }
    
    /// <summary>
    /// Trieda slúži na naplneneie Empirického rozdelenia spolu so seedom
    /// </summary>
    /// <typeparam name="T">Typ numerickej hodnoty</typeparam>
    /// <exception cref="ArgumentException">Keď je súčet != 1 </exception>
    public class EmpiricDataWithSeed<T>
    {
        public Pair<T, T> Range {
            get;
            set;
        }
        
        public Pair<double, int> ProbabilitySeed {
            get;
            set;
        }
        
        /// <summary>
        /// Parametrický konštruktor
        /// </summary>
        /// <param name="pMin">min value</param>
        /// <param name="pMax">max value</param>
        /// <param name="pProbability">probability of the RNG</param>
        /// <param name="pSeed">seed</param>
        public EmpiricDataWithSeed(T pMin, T pMax, double pProbability, int pSeed)
        {
            Range = new Pair<T, T>(pMin, pMax);
            ProbabilitySeed = new Pair<double, int>(pProbability, pSeed);
        }
        
        /// <summary>
        /// Parametrický konštruktor
        /// </summary>
        /// <param name="pMin">min value</param>
        /// <param name="pMax">max value</param>
        /// <param name="pProbability">probability of the RNG</param>
        /// <param name="pSeed">seed</param>
        public EmpiricDataWithSeed(Pair<T, T> pRange, double pProbability, int pSeed)
        {
            Range = pRange;
            ProbabilitySeed = new Pair<double, int>(pProbability, pSeed);
        }
        
        /// <summary>
        /// Parametrický konštruktor
        /// </summary>
        /// <param name="pMin">min value</param>
        /// <param name="pMax">max value</param>
        /// <param name="pProbability">probability of the RNG</param>
        /// <param name="pSeed">seed</param>
        public EmpiricDataWithSeed(Pair<T, T> pRange, Pair<double, int> pProbabilitySeed)
        {
            Range = pRange;
            ProbabilitySeed = pProbabilitySeed;
        }
    }

    protected List<EmpiricData<T>> _listOfValuse;

    /// <summary>
    /// Empiric RNG
    /// </summary>
    /// <param name="pListOfValues">list values of probability</param>
    public EmpiricBase(List<EmpiricData<T>> pListOfValues)
    {
        double sum = 0.0;
        
        pListOfValues.ForEach(x => sum += x.Probability);

        if (Math.Abs(sum - 1.0) > Double.Epsilon)
        {
            throw new ArgumentException("Sum of probabilities is not equal to 1");
        }
    }
    
    /// <summary>
    /// Empiric RNG
    /// </summary>
    /// <param name="pListOfValues">list values of probability</param>
    public EmpiricBase(List<EmpiricDataWithSeed<T>> pListOfValues, int pSeed) : base(pSeed)
    {
        double sum = 0.0;
        
        pListOfValues.ForEach(x => sum += x.ProbabilitySeed.First);

        if (Math.Abs(sum - 1.0) > Double.Epsilon)
        {
            throw new ArgumentException("Sum of probabilities is not equal to 1");
        }
    }
}