using OSPABA;
using simulation;
using managers;
using continualAssistants;
using instantAssistants;
namespace agents
{
	//meta! id="9"
	public class AgentPredajne : Agent
	{
		public AgentPredajne(int id, Simulation mySim, Agent parent) :
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
			new ManagerPredajne(SimId.ManagerPredajne, MySim, this);
			AddOwnMessage(Mc.PocetMiestVRade);
			AddOwnMessage(Mc.VstupDoPredajne);
			AddOwnMessage(Mc.Init);
			AddOwnMessage(Mc.NoticeUvolnenieZakaznika);
			AddOwnMessage(Mc.NoticePrestavkaKoniec);
			AddOwnMessage(Mc.NoticeKoniecObsluhy);
			AddOwnMessage(Mc.NoticeUvolneniePredajni);
			AddOwnMessage(Mc.PridelenieZakaznikaOM);
			AddOwnMessage(Mc.PridelenieZakaznikaPredajni);
		}
		//meta! tag="end"
	}
}