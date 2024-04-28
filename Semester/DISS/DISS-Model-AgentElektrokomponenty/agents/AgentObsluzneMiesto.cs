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
			new ProcessOMDiktovanie(SimId.ProcessOMDiktovanie, MySim, this);
			new SchedulerPrestavkaOM(SimId.SchedulerPrestavkaOM, MySim, this);
			new ActionPrideleniePokladne(SimId.ActionPrideleniePokladne, MySim, this);
			new ProcessOMPripravaTovaru(SimId.ProcessOMPripravaTovaru, MySim, this);
			new ProcessOMOnlinePripravaTovaru(SimId.ProcessOMOnlinePripravaTovaru, MySim, this);
			AddOwnMessage(Mc.PocetMiestVRade);
			AddOwnMessage(Mc.Init);
			AddOwnMessage(Mc.NoticeZaciatokObsluhyOM);
			AddOwnMessage(Mc.NoticeUvolnenieOM);
			AddOwnMessage(Mc.NoticePrestavkaZaciatok);
		}
		//meta! tag="end"
	}
}