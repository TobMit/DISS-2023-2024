using OSPABA;
using simulation;
using managers;
using continualAssistants;
namespace agents
{
	//meta! id="3"
	public class AgentOkolia : Agent
	{
		public AgentOkolia(int id, Simulation mySim, Agent parent) :
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
			new ManagerOkolia(SimId.ManagerOkolia, MySim, this);
			new PlanovacPrichodov(SimId.PlanovacPrichodov, MySim, this);
			AddOwnMessage(Mc.OdchodZakaznika);
			AddOwnMessage(Mc.Inicializacia);
			AddOwnMessage(Mc.NoticeNovyZakaznik);
		}
		//meta! tag="end"
	}
}