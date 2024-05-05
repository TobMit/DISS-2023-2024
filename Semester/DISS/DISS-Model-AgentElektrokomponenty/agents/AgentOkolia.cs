using OSPABA;
using simulation;
using managers;
using continualAssistants;
using instantAssistants;
namespace agents
{
	//meta! id="2"
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
			new PlanovacPrichodovOnline(SimId.PlanovacPrichodovOnline, MySim, this);
			new PlanovacPrichodovBasic(SimId.PlanovacPrichodovBasic, MySim, this);
			new PlanovacPrichodovZmluvny(SimId.PlanovacPrichodovZmluvny, MySim, this);
			AddOwnMessage(Mc.Init);
			AddOwnMessage(Mc.NoticeNovyBasic);
			AddOwnMessage(Mc.NoticeOdchodZakaznika);
			AddOwnMessage(Mc.NoticeNovyOnline);
			AddOwnMessage(Mc.NoticeNovyZmluvny);
		}
		//meta! tag="end"
	}
}