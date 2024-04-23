using AbbaNovynovyStanok.simulation;
using simulation;
using managers;
using continualAssistants;
using OSPABA;

namespace agents
{
	//meta! id="1"
	public class AgentModelu : Agent
	{
		public AgentModelu(int id, Simulation mySim, Agent parent) :
			base(id, mySim, parent)
		{
			Init();
		}

		public override void PrepareReplication()
		{
			base.PrepareReplication();
			var message = new MyMessage(MySim)
			{
				Addressee = this,
				Code = Mc.Inicializacia
			};
			MyManager.Notice(message);
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new ManagerModelu(SimId.ManagerModelu, MySim, this);
			AddOwnMessage(Mc.PrichodZakaznika);
			AddOwnMessage(Mc.NoticeKoniecObsluhy);
			AddOwnMessage(Mc.Obsluha);
		}
		//meta! tag="end"
	}
}