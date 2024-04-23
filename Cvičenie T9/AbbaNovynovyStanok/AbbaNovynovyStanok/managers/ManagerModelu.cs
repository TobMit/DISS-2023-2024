using AbbaNovynovyStanok.simulation;
using OSPABA;
using simulation;
using agents;
namespace managers
{
	//meta! id="1"
	public class ManagerModelu : Manager
	{
		public ManagerModelu(int id, Simulation mySim, Agent myAgent) :
			base(id, mySim, myAgent)
		{
			Init();
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication

			if (PetriNet != null)
			{
				PetriNet.Clear();
			}
		}

		//meta! sender="AgentOkolia", id="8", type="Notice"
		public void ProcessPrichodZakaznika(MessageForm message)
		{
			// Console.WriteLine("ManagerModelu: ProcessPrichodZakaznika");
			var sprava = (MyMessage)message.CreateCopy();
			sprava.Addressee = MySim.FindAgent(SimId.AgentStanku);
			sprava.Code = Mc.Obsluha;
			Request(sprava);
		}

		//meta! sender="AgentStanku", id="9", type="Response"
		public void ProcessObsluha(MessageForm message)
		{
			// Console.WriteLine("ManagerModelu: ProcessObsluha");
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
			}
		}

		//meta! sender="AgentStanku", id="35", type="Notice"
		public void ProcessNoticeKoniecObsluhy(MessageForm message)
		{
			// Console.WriteLine("ManagerModelu: ProcessNoticeKoniecObsluhy");
			var sprava = (MyMessage)message.CreateCopy();
			sprava.Addressee = MySim.FindAgent(SimId.AgentOkolia);
			sprava.Code = Mc.OdchodZakaznika;
			Notice(sprava);
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		public void Init()
		{
		}

		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
			case Mc.PrichodZakaznika:
				ProcessPrichodZakaznika(message);
			break;

			case Mc.NoticeKoniecObsluhy:
				ProcessNoticeKoniecObsluhy(message);
			break;

			case Mc.Obsluha:
				ProcessObsluha(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new AgentModelu MyAgent
		{
			get
			{
				return (AgentModelu)base.MyAgent;
			}
		}
	}
}