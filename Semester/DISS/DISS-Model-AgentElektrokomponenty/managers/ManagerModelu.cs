using OSPABA;
using simulation;
using agents;
using continualAssistants;
using instantAssistants;
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

		//meta! sender="AgentOkolia", id="18", type="Notice"
		public void ProcessNoticePrichodZakaznika(MessageForm message)
		{
			var sprava = (MyMessage)message.CreateCopy();
			Constants.Log("ManagerModelu", MySim.CurrentTime, null,$"ProcessNoticePrichodZakaznika typu: {sprava.Zakaznik.TypZakaznika}", Constants.LogType.ManagerLog);
			((MySimulation)MySim).CelkovyPocetZakaznikov++;
			sprava.Addressee = MySim.FindAgent(SimId.AgentPredajne);
			sprava.Code = Mc.VstupDoPredajne;
			Notice(sprava);
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
			}
		}

		//meta! sender="AgentPredajne", id="125", type="Notice"
		public void ProcessNoticeOdchodZakaznika(MessageForm message)
		{
			var sprava = (MyMessage)message.CreateCopy();
			Constants.Log("ManagerModelu", MySim.CurrentTime, sprava.Zakaznik,"ProcessNoticeOdchodZakaznika", Constants.LogType.ManagerLog);
			sprava.Addressee = MySim.FindAgent(SimId.AgentOkolia);
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
			case Mc.NoticeOdchodZakaznika:
				ProcessNoticeOdchodZakaznika(message);
			break;

			case Mc.NoticePrichodZakaznika:
				ProcessNoticePrichodZakaznika(message);
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