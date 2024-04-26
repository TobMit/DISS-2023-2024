using OSPABA;
using simulation;
using agents;
using continualAssistants;
using instantAssistants;
using OSPDataStruct;

namespace managers
{
	//meta! id="25"
	public class ManagerAutomatu : Manager
	{
		//Id of the people in AgentAutomat
		public int Id { get; set; }
		public bool Obsluhuje { get; set; }

		public SimQueue<MyMessage> Front { get; set; }
		public ManagerAutomatu(int id, Simulation mySim, Agent myAgent) :
			base(id, mySim, myAgent)
		{
			Init();
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
			Front = new();
			if (PetriNet != null)
			{
				PetriNet.Clear();
			}
		}

		//meta! sender="ProcessObsluhaAutomatu", id="33", type="Finish"
		public void ProcessFinishProcessObsluhaAutomatu(MessageForm message)
		{
			Constants.Log("ManagerAutomatu: ProcessFinishProcessObsluhaAutomatu", Constants.LogType.ManagerLog);
		}

		//meta! sender="SchedulerZatvorenieAutomatu", id="55", type="Finish"
		public void ProcessFinishSchedulerZatvorenieAutomatu(MessageForm message)
		{
			Constants.Log($"ManagerAutomatu {TimeSpan.FromSeconds(MySim.CurrentTime + Constants.START_DAY).ToString(@"hh\:mm\:ss")}: ProcessFinishSchedulerZatvorenieAutomatu", Constants.LogType.ManagerLog);
			//todo add logics + statistics
		}

		//meta! sender="AgentPredajne", id="34", type="Notice"
		public void ProcessNoticeZaciatokObsluhy(MessageForm message)
		{
			Constants.Log("ManagerAutomatu: ProcessNoticeZaciatokObsluhy", Constants.LogType.ManagerLog);
			var sprava = (MyMessage)message.CreateCopy();
			sprava.Zakaznik.ID = Id++;
			if (Front.Count > 0)
			{
				sprava.Zakaznik.StavZakaznika = Constants.StavZakaznika.RadPredAutomatom;
				Front.Enqueue(sprava);
			}
			else if (Obsluhuje)
			{
				sprava.Zakaznik.StavZakaznika = Constants.StavZakaznika.RadPredAutomatom;
				Front.Enqueue(sprava);
			}
			else
			{
				//todo vyriešiť ak je počet ľudí viac ako 9 tak ide do rady
				sprava.Addressee = MyAgent.FindAssistant(SimId.ProcessObsluhaAutomatu);
				StartContinualAssistant(sprava);
				Obsluhuje = true;
				((MySimulation)MySim).StatCasStravenyPredAutomatom.AddSample(MySim.CurrentTime - sprava.Zakaznik.TimeOfArrival);
			}
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
			}
		}

		//meta! sender="AgentPredajne", id="91", type="Notice"
		public void ProcessInit(MessageForm message)
		{
			Constants.Log("ManagerAutomatu: ProcessInit", Constants.LogType.ManagerLog);
			Id = 0;
			Front.Clear();
			Obsluhuje = false;
			var sprava = (MyMessage)message.CreateCopy();
			sprava.Addressee = MyAgent.FindAssistant(SimId.SchedulerZatvorenieAutomatu);
			StartContinualAssistant(sprava);
		}

		//meta! sender="SchedulerZatvorenieAutomatu", id="94", type="Notice"
		public void ProcessNoticeZatvorenieAutomatu(MessageForm message)
		{
			Constants.Log($"ManagerAutomatu {TimeSpan.FromSeconds(MySim.CurrentTime + Constants.START_DAY).ToString(@"hh\:mm\:ss")}: ProcessNoticeZatvorenieAutomatu", Constants.LogType.ManagerLog);
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		public void Init()
		{
		}

		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
			case Mc.Init:
				ProcessInit(message);
			break;

			case Mc.NoticeZaciatokObsluhy:
				ProcessNoticeZaciatokObsluhy(message);
			break;

			case Mc.Finish:
				switch (message.Sender.Id)
				{
				case SimId.SchedulerZatvorenieAutomatu:
					ProcessFinishSchedulerZatvorenieAutomatu(message);
				break;

				case SimId.ProcessObsluhaAutomatu:
					ProcessFinishProcessObsluhaAutomatu(message);
				break;
				}
			break;

			case Mc.NoticeZatvorenieAutomatu:
				ProcessNoticeZatvorenieAutomatu(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new AgentAutomatu MyAgent
		{
			get
			{
				return (AgentAutomatu)base.MyAgent;
			}
		}
	}
}