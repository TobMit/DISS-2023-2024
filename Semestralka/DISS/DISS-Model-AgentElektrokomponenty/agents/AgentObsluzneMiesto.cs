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

		public override void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new ManagerObsluzneMiesto(SimId.ManagerObsluzneMiesto, MySim, this);
			new ProcessOMOnlinePripravaTovaru(SimId.ProcessOMOnlinePripravaTovaru, MySim, this);
			new ProcessOMPripravaTovaru(SimId.ProcessOMPripravaTovaru, MySim, this);
			new ProcessVyzdvihnutieTovaru(SimId.ProcessVyzdvihnutieTovaru, MySim, this);
			new ActionPridelenieOm(SimId.ActionPridelenieOm, MySim, this);
			new SchedulerPrestavkaOM(SimId.SchedulerPrestavkaOM, MySim, this);
			new ProcessOMDiktovanie(SimId.ProcessOMDiktovanie, MySim, this);
			AddOwnMessage(Mc.PocetMiestVRade);
			AddOwnMessage(Mc.Init);
			AddOwnMessage(Mc.NoticeZaciatokObsluhyOm);
			AddOwnMessage(Mc.NoticePrestavkaKoniec);
			AddOwnMessage(Mc.NoticeUvolnenieOm);
		}
		//meta! tag="end"
	}
}