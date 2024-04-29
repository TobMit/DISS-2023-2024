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
			var sprava = (MyMessage)message.CreateCopy();
			Constants.Log("ManagerPredajne", MySim.CurrentTime, sprava.Zakaznik,"ProcessInit", Constants.LogType.ManagerLog);
			sprava.Addressee = MySim.FindAgent(SimId.AgentAutomatu);
			Notice(new MyMessage(sprava));
			//todo add more init to other agents
		}

		//meta! sender="AgentModelu", id="28", type="Notice"
		public void ProcessVstupDoPredajne(MessageForm message)
		{
			var sprava = (MyMessage)message.CreateCopy();
			Constants.Log("ManagerPredajne", MySim.CurrentTime, sprava.Zakaznik,"ProcessVstupDoPredajne", Constants.LogType.ManagerLog);
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

		//meta! sender="AgentAutomatu", id="35", type="Notice"
		public void ProcessNoticeKoniecObsluhy(MessageForm message)
		{
			var sprava = (MyMessage)message.CreateCopy();
			Constants.Log("ManagerPredajne", MySim.CurrentTime, sprava.Zakaznik,"ProcessNoticeKoniecObsluhy", Constants.LogType.ManagerLog);
			sprava.Addressee = MySim.FindAgent(SimId.AgentObsluzneMiesto);
			sprava.Code = Mc.NoticeZaciatokObsluhyOm;
			Notice(sprava);
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
			Constants.Log("ManagerPredajne", MySim.CurrentTime, sprava.Zakaznik," ProcessPocetMiestVRade", Constants.LogType.ManagerLog);
			sprava.Addressee = MySim.FindAgent(SimId.AgentAutomatu);
			sprava.Code = Mc.NoticeZaciatokObsluhy;
			Notice(sprava);
		}

		//meta! sender="AgentObsluzneMiesto", id="104", type="Notice"
		public void ProcessNoticeKoniecObsluhyOm(MessageForm message)
		{
			var sprava = (MyMessage)message.CreateCopy();
			Constants.Log("ManagerPredajne", MySim.CurrentTime, sprava.Zakaznik,"ProcessNoticeKoniecObsluhyOm", Constants.LogType.ManagerLog);
			if (sprava.TovarVydvihnty)
			{
				sprava.Addressee = MySim.FindAgent(SimId.AgentModelu);
				sprava.Code = Mc.NoticeOdchodZakaznika;
				Notice(sprava);
			}
			else
			{
				sprava.Addressee = MySim.FindAgent(SimId.AgentPokladni);
				sprava.Code = Mc.NoticeZaciatokPokladne;
				Notice(sprava);
			}
		}

		//meta! sender="AgentPokladni", id="115", type="Notice"
		public void ProcessNoticeKoniecPokladne(MessageForm message)
		{
			var sprava = (MyMessage)message.CreateCopy();
			Constants.Log("ManagerPredajne", MySim.CurrentTime, sprava.Zakaznik,"ProcessNoticeKoniecPokladne", Constants.LogType.ManagerLog);
			if (sprava.Zakaznik.TypVelkostiNakladu == Constants.TypVelkostiNakladu.Normálna)
			{
				sprava.Addressee = MySim.FindAgent(SimId.AgentModelu);
				sprava.Code = Mc.NoticeOdchodZakaznika;
				Notice(sprava);
			}
			else
			{
				sprava.Addressee = MySim.FindAgent(SimId.AgentObsluzneMiesto);
				sprava.Code = Mc.NoticeUvolnenieOm;
				Notice(sprava);
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
			case Mc.PocetMiestVRade:
				ProcessPocetMiestVRade(message);
			break;

			case Mc.NoticeKoniecPokladne:
				ProcessNoticeKoniecPokladne(message);
			break;

			case Mc.Init:
				ProcessInit(message);
			break;

			case Mc.NoticeKoniecObsluhyOm:
				ProcessNoticeKoniecObsluhyOm(message);
			break;

			case Mc.VstupDoPredajne:
				ProcessVstupDoPredajne(message);
			break;

			case Mc.NoticeKoniecObsluhy:
				ProcessNoticeKoniecObsluhy(message);
			break;

			case Mc.NoticePrestavkaKoniec:
				switch (message.Sender.Id)
				{
				case SimId.AgentObsluzneMiesto:
					ProcessNoticePrestavkaKoniecAgentObsluzneMiesto(message);
				break;

				case SimId.AgentPokladni:
					ProcessNoticePrestavkaKoniecAgentPokladni(message);
				break;
				}
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