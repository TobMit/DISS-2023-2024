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

		//meta! sender="AgentPredajne", id="28", type="Response"
		public void ProcessVstupDoPredajne(MessageForm message)
		{
		}

		//meta! sender="AgentOkolia", id="18", type="Notice"
		public void ProcessNoticePrichodZakaznika(MessageForm message)
		{
			Constants.Log($"ManagerModelu ({TimeSpan.FromSeconds(MySim.CurrentTime + Constants.START_DAY).ToString(@"hh\:mm\:ss")}): ProcessNoticePrichodZakaznika", Constants.LogType.ManagerLog);
			((MySimulation)MySim).testPocetLudi++;
			switch (((MyMessage)message).TypZakaznika)
			{
				case Constants.TypZakaznika.Basic:
					((MySimulation)MySim).testPocetLudiBasic++;
					break;
				case Constants.TypZakaznika.Zmluvný:
					((MySimulation)MySim).testPocetLudiZmluvny++;
					break;
			}
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
			}
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		public void Init()
		{
		}

		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
			case Mc.NoticePrichodZakaznika:
				ProcessNoticePrichodZakaznika(message);
			break;

			case Mc.VstupDoPredajne:
				ProcessVstupDoPredajne(message);
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