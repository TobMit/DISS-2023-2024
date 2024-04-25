using OSPABA;
using simulation;
using managers;
using continualAssistants;
using instantAssistants;
namespace agents
{
	//meta! id="44"
	public class AgentPokladni : Agent
	{
		public AgentPokladni(int id, Simulation mySim, Agent parent) :
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
			new ManagerPokladni(SimId.ManagerPokladni, MySim, this);
			new SchedulerPrestavkaPokladne(SimId.SchedulerPrestavkaPokladne, MySim, this);
			new ProcessObsluhyPokladni(SimId.ProcessObsluhyPokladni, MySim, this);
			new QueryPridelPokladnu(SimId.QueryPridelPokladnu, MySim, this);
			AddOwnMessage(Mc.Init);
			AddOwnMessage(Mc.PridelenieZakaznikaPredajni);
			AddOwnMessage(Mc.NoticePrestavkaZaciatok);
		}
		//meta! tag="end"
	}
}