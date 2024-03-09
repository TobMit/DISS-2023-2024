namespace DISS_MonteCarloCore.Core;

public abstract class MonteCarloCore
{
    protected int _numberOfReplications;
    protected int _cutFirst;
    protected bool _stop;

    public bool IsRunning
    {
        get { return !_stop; }
    }

    protected int _currentReplication;

    public abstract void BeforeAllReplications();
    public abstract void BeforeReplication();
    public abstract void Replication();
    public abstract void AfterReplication();
    public abstract void AfterAllReplications();

    public ManualResetEventSlim ReadEvent { get; set; }
    private Thread simulationThread;

    protected MonteCarloCore(int numberOfReplications, int cutFirst)
    {
        _numberOfReplications = numberOfReplications;
        _cutFirst = cutFirst;
        _stop = false;
        ReadEvent = new(false);
    }

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

    public void Stop()
    {
        _stop = true;
    }

    void SimulationThread()
    {
        //Console.WriteLine("Simulation thread started.");
        BeforeAllReplications();
        for (_currentReplication = 0; _currentReplication < _numberOfReplications && !_stop; _currentReplication++)
        {
            BeforeReplication();
            Replication();
            AfterReplication();
            ReadEvent.Set();
        }

        AfterAllReplications();
        //Console.WriteLine("Simulation thread exited.");
        _stop = true;
        ReadEvent.Set();
    }

}