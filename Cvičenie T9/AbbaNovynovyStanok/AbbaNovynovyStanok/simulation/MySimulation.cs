using agents;
using OSPABA;
using simulation;

namespace AbbaNovynovyStanok.simulation
{
	public class MySimulation : Simulation
	{
		public MySimulation()
		{
			Init();
		}

		protected override void PrepareSimulation()
		{
			base.PrepareSimulation();
			// Create global statistcis
			Console.WriteLine("test");
		}

		protected override void PrepareReplication()
		{
			base.PrepareReplication();
			// Reset entities, queues, local statistics, etc...
		}

		protected override void ReplicationFinished()
		{
			// Collect local statistics into global, update UI, etc...
			base.ReplicationFinished();
			Console.WriteLine("run");
		}

		protected override void SimulationFinished()
		{
			// Dysplay simulation results
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
