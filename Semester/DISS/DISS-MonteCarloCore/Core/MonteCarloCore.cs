using System.Diagnostics;

namespace DISS_MonteCarloCore.Core;

/// <summary>
/// The core of the Monte Carlo simulation.
/// </summary>
public abstract class MonteCarloCore
{
    protected int _numberOfReplications;
    protected int _cutFirst;
    protected bool _stop;

    /// <summary>
    /// If is the simulation running.
    /// </summary>
    public bool IsRunning
    {
        get { return !_stop; }
    }

    protected int _currentReplication;

    /// <summary>
    /// Set up before all replications.
    /// </summary>
    public abstract void BeforeAllReplications();
    
    /// <summary>
    /// Set up before each replication.
    /// </summary>
    public abstract void BeforeReplication();
    
    /// <summary>
    /// The replication itself.
    /// </summary>
    public abstract void Replication();
    
    /// <summary>
    /// Set up after each replication.
    /// </summary>
    public abstract void AfterReplication();
    
    /// <summary>
    /// Set up after all replications.
    /// </summary>
    public abstract void AfterAllReplications();

    public ManualResetEventSlim ReadEvent { get; set; }
    private Thread simulationThread;

    /// <summary>
    /// Set up the basic parameters of the simulation.
    /// </summary>
    /// <param name="numberOfReplications">Max number of replication</param>
    /// <param name="cutFirst">cunt first params from sending to the FE</param>
    protected MonteCarloCore(int numberOfReplications, int cutFirst)
    {
        _numberOfReplications = numberOfReplications;
        _cutFirst = cutFirst;
        _stop = false;
        ReadEvent = new(false);
    }

    /// <summary>
    /// To start the simulation.
    /// </summary>
    public void Run()
    {
        _stop = false;
        //SimulationThread();
        //simulationThread = new Thread(SimulationThread);
        //simulationThread.Start();
        Task.Run(() => { SimulationThread(); });
        //Console.WriteLine("Main thread started simulation thread.");
         //simulationThread.Join();
    }
    public void RunDebug()
    {
        _stop = false;
        //SimulationThread();
        //simulationThread = new Thread(SimulationThread);
        //simulationThread.Start();
        SimulationThread();
        //Console.WriteLine("Main thread started simulation thread.");
        //simulationThread.Join();
    }

    /// <summary>
    /// To stop the simulation
    /// </summary>
    public void Stop()
    {
        _stop = true;
    }

    /// <summary>
    /// Simulation thread thant runs the simulation, replication by replication.
    /// </summary>
    private void SimulationThread()
    {
        //Console.WriteLine("Simulation thread started.");
        BeforeAllReplications();
        Stopwatch stopwatch = new();
        stopwatch.Start();
        for (_currentReplication = 0; _currentReplication < _numberOfReplications && !_stop; _currentReplication++)
        {
            BeforeReplication();
            Replication();
            AfterReplication();
            ReadEvent.Set();
            if (_currentReplication % 100 == 0)
            {
                stopwatch.Stop();
                Console.WriteLine($"Replication run {_currentReplication} complete in time: {stopwatch.ElapsedMilliseconds}");
                stopwatch.Restart();
            }
        }

        AfterAllReplications();
        //Console.WriteLine("Simulation thread exited.");
        _stop = true;
        ReadEvent.Set();
    }

}