using OSPABA;
using simulation;
using managers;
using continualAssistants;
using instantAssistants;
namespace agents
{
	//meta! id="25"
	public class AgentAutomatu : Agent
	{
		public AgentAutomatu(int id, Simulation mySim, Agent parent) :
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
			new ManagerAutomatu(SimId.ManagerAutomatu, MySim, this);
			new ProcessObsluhaAutomatu(SimId.ProcessObsluhaAutomatu, MySim, this);
			new SchedulerZatvorenieAutomatu(SimId.SchedulerZatvorenieAutomatu, MySim, this);
			AddOwnMessage(Mc.NoticeZaciatokObsluhy);
		}
		//meta! tag="end"
	}
}
