using agents;
using OSPABA;
using simulation;

namespace AbbaNovynovyStanok.simulation
{
	public class MySimulation : Simulation
	{
		
		public ExponentialGenerator GeneratorCasovPrichodov { get; set; }
		public ExponentialGenerator GeneratorCasovObsluhy { get; set; }

		public double PriemernyCasCakania { get; set; }
		public double KumulativnyCasCakania { get; set; }
		public int KumulativnyPocetCakajucich { get; set; }

		private Random generatorNasad;
		
		public MySimulation()
		{
			Init();
		}

		protected override void PrepareSimulation()
		{
			base.PrepareSimulation();
			// Create global statistcis
			PriemernyCasCakania = 0.0;
			generatorNasad = new Random();

			GeneratorCasovPrichodov = new ExponentialGenerator(generatorNasad.Next(), 100);
			GeneratorCasovObsluhy = new ExponentialGenerator(generatorNasad.Next(), 45);
			Console.Clear();
			Console.WriteLine("Simulating...");
		}

		protected override void PrepareReplication()
		{
			base.PrepareReplication();
			// Reset entities, queues, local statistics, etc...
			KumulativnyCasCakania = 0;
			KumulativnyPocetCakajucich = 0;
		}

		protected override void ReplicationFinished()
		{
			PriemernyCasCakania += KumulativnyCasCakania / KumulativnyPocetCakajucich;
			
			Console.WriteLine("R" + CurrentReplication + ": Celkový priemerný čas čakania: {0:0.00000}", PriemernyCasCakania / (1 + CurrentReplication));
			base.ReplicationFinished();
		}

		protected override void SimulationFinished()
		{
			base.SimulationFinished();
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			AgentModelu = new AgentModelu(SimId.AgentModelu, this, null);
			AgentOkolia = new AgentOkolia(SimId.AgentOkolia, this, AgentModelu);
			AgentStanku = new AgentStanku(SimId.AgentStanku, this, AgentModelu);
		}
		public AgentModelu AgentModelu
		{ get; set; }
		public AgentOkolia AgentOkolia
		{ get; set; }
		public AgentStanku AgentStanku
		{ get; set; }
		//meta! tag="end"
	}
}