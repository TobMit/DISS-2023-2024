using OSPABA;
using simulation;
using managers;
using continualAssistants;
using instantAssistants;
namespace agents
{
	//meta! id="36"
	public class AgentObsluzneMiesto : Agent
	{
		public AgentObsluzneMiesto(int id, Simulation mySim, Agent parent) :
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
			new ManagerObsluzneMiesto(SimId.ManagerObsluzneMiesto, MySim, this);
			new SchedulerPrestavkaOM(SimId.SchedulerPrestavkaOM, MySim, this);
			new ProcessOM(SimId.ProcessOM, MySim, this);
			new QueryPridelenieOM(SimId.QueryPridelenieOM, MySim, this);
			AddOwnMessage(Mc.PocetMiestVRade);
			AddOwnMessage(Mc.Init);
			AddOwnMessage(Mc.PridelenieZakaznikaOM);
			AddOwnMessage(Mc.NoticeUvolnenieOM);
			AddOwnMessage(Mc.NoticePrestavkaZaciatok);
		}
		//meta! tag="end"
	}
}