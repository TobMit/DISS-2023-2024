using OSPABA;
using simulation;
using managers;
using continualAssistants;
namespace agents
{
	//meta! id="4"
	public class AgentStanku : Agent
	{
		public AgentStanku(int id, Simulation mySim, Agent parent) :
			base(id, mySim, parent)
		{
			Init();
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new ManagerStanku(SimId.ManagerStanku, MySim, this);
			new ProcesObsluhy(SimId.ProcesObsluhy, MySim, this);
			AddOwnMessage(Mc.Obsluha);
		}
		//meta! tag="end"
	}
}