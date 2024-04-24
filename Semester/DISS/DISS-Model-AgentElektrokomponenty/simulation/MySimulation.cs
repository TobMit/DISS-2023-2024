using OSPABA;
using agents;
namespace simulation
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
			AgentPredajne = new AgentPredajne(SimId.AgentPredajne, this, AgentModelu);
			AgentPokladni = new AgentPokladni(SimId.AgentPokladni, this, AgentPredajne);
			AgentAutomatu = new AgentAutomatu(SimId.AgentAutomatu, this, AgentPredajne);
			AgentObsluzneMiesto = new AgentObsluzneMiesto(SimId.AgentObsluzneMiesto, this, AgentPredajne);
		}
		public AgentModelu AgentModelu
		{ get; set; }
		public AgentOkolia AgentOkolia
		{ get; set; }
		public AgentPredajne AgentPredajne
		{ get; set; }
		public AgentPokladni AgentPokladni
		{ get; set; }
		public AgentAutomatu AgentAutomatu
		{ get; set; }
		public AgentObsluzneMiesto AgentObsluzneMiesto
		{ get; set; }
		//meta! tag="end"
	}
}
