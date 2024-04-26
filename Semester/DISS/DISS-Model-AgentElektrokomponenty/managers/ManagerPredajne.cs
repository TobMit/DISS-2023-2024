using OSPABA;
using simulation;
using agents;
using continualAssistants;
using instantAssistants;
namespace managers
{
	//meta! id="9"
	public class ManagerPredajne : Manager
	{
		public ManagerPredajne(int id, Simulation mySim, Agent myAgent) :
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

		//meta! sender="AgentModelu", id="27", type="Notice"
		public void ProcessInit(MessageForm message)
		{
			Constants.Log("ManagerPredajne: ProcessInit", Constants.LogType.ManagerLog);
			var sprava = (MyMessage)message.CreateCopy();
			sprava.Addressee = MySim.FindAgent(SimId.AgentAutomatu);
			Notice(new MyMessage(sprava));
			//todo add more init to other agents
		}

		//meta! sender="AgentModelu", id="28", type="Notice"
		public void ProcessVstupDoPredajne(MessageForm message)
		{
			var sprava = (MyMessage)message.CreateCopy();
			Constants.Log($"ManagerPredajne: ProcessVstupDoPredajne zakaznik ID {sprava.Zakaznik.ID}", Constants.LogType.ManagerLog);
			sprava.Addressee = MySim.FindAgent(SimId.AgentObsluzneMiesto);
			sprava.Code = Mc.PocetMiestVRade;
			Request(sprava);
		}

		//meta! sender="AgentPokladni", id="59", type="Notice"
		public void ProcessNoticePrestavkaKoniecAgentPokladni(MessageForm message)
		{
		}

		//meta! sender="AgentObsluzneMiesto", id="67", type="Notice"
		public void ProcessNoticePrestavkaKoniecAgentObsluzneMiesto(MessageForm message)
		{
		}

		//meta! sender="AgentObsluzneMiesto", id="43", type="Notice"
		public void ProcessNoticeUvolnenieZakaznika(MessageForm message)
		{
		}

		//meta! sender="AgentPokladni", id="48", type="Notice"
		public void ProcessNoticeUvolneniePredajni(MessageForm message)
		{
		}

		//meta! sender="AgentAutomatu", id="35", type="Notice"
		public void ProcessNoticeKoniecObsluhy(MessageForm message)
		{
			var sprava = (MyMessage)message.CreateCopy();
			Constants.Log($"ManagerPredajne: Zakaznik: {sprava.Zakaznik.ID} ProcessNoticeKoniecObsluhy", Constants.LogType.ManagerLog);
		}

		//meta! sender="AgentObsluzneMiesto", id="41", type="Response"
		public void ProcessPridelenieZakaznikaOM(MessageForm message)
		{
		}

		//meta! sender="AgentPokladni", id="47", type="Response"
		public void ProcessPridelenieZakaznikaPredajni(MessageForm message)
		{
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
			}
		}

		//meta! sender="AgentObsluzneMiesto", id="97", type="Response"
		public void ProcessPocetMiestVRade(MessageForm message)
		{
			var sprava = (MyMessage)message.CreateCopy();
			Constants.Log($"ManagerPredajne: Zakaznik: {sprava.Zakaznik.ID} ProcessPocetMiestVRade", Constants.LogType.ManagerLog);
			sprava.Addressee = MySim.FindAgent(SimId.AgentAutomatu);
			sprava.Code = Mc.NoticeZaciatokObsluhy;
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
			case Mc.PridelenieZakaznikaOM:
				ProcessPridelenieZakaznikaOM(message);
			break;

			case Mc.NoticeKoniecObsluhy:
				ProcessNoticeKoniecObsluhy(message);
			break;

			case Mc.NoticeUvolnenieZakaznika:
				ProcessNoticeUvolnenieZakaznika(message);
			break;

			case Mc.PocetMiestVRade:
				ProcessPocetMiestVRade(message);
			break;

			case Mc.NoticePrestavkaKoniec:
				switch (message.Sender.Id)
				{
				case SimId.AgentPokladni:
					ProcessNoticePrestavkaKoniecAgentPokladni(message);
				break;

				case SimId.AgentObsluzneMiesto:
					ProcessNoticePrestavkaKoniecAgentObsluzneMiesto(message);
				break;
				}
			break;

			case Mc.NoticeUvolneniePredajni:
				ProcessNoticeUvolneniePredajni(message);
			break;

			case Mc.PridelenieZakaznikaPredajni:
				ProcessPridelenieZakaznikaPredajni(message);
			break;

			case Mc.VstupDoPredajne:
				ProcessVstupDoPredajne(message);
			break;

			case Mc.Init:
				ProcessInit(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new AgentPredajne MyAgent
		{
			get
			{
				return (AgentPredajne)base.MyAgent;
			}
		}
	}
}