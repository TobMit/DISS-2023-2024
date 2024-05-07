using OSPABA;
using simulation;
using managers;
using continualAssistants;
using instantAssistants;
namespace agents
{
	//meta! id="148"
	public class AgentReklamacia : Agent
	{
		public AgentReklamacia(int id, Simulation mySim, Agent parent) :
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
			new ManagerReklamacia(SimId.ManagerReklamacia, MySim, this);
			new ProcessReklamacie(SimId.ProcessReklamacie, MySim, this);
			AddOwnMessage(Mc.NoticeZaciatokReklamacie);
		}
		//meta! tag="end"
	}
}
