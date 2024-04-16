
using AbbaNovynovyStanok.simulation;

class Program
{
    static void Main(string[] args)
    {
        MySimulation simulation = new();
        simulation.Simulate(1000,10);
    }
}