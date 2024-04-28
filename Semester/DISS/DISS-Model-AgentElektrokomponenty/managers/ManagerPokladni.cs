using OSPABA;
using simulation;
using agents;
using continualAssistants;
using instantAssistants;
namespace managers
{
	//meta! id="44"
	public class ManagerPokladni : Manager
	{
		public ManagerPokladni(int id, Simulation mySim, Agent myAgent) :
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

		//meta! sender="AgentPredajne", id="46", type="Notice"
		public void ProcessInit(MessageForm message)
		{
		}

		//meta! userInfo="Removed from model"
		public void ProcessPridelenieZakaznikaPredajni(MessageForm message)
		{
		}

		//meta! sender="ProcessObsluhyPokladni", id="53", type="Finish"
		public void ProcessFinishProcessObsluhyPokladni(MessageForm message)
		{
		}

		//meta! sender="SchedulerPrestavkaPokladne", id="57", type="Finish"
		public void ProcessFinishSchedulerPrestavkaPokladne(MessageForm message)
		{
		}

		//meta! sender="AgentPredajne", id="58", type="Notice"
		public void ProcessNoticePrestavkaZaciatok(MessageForm message)
		{
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
			}
		}

		//meta! sender="AgentPredajne", id="114", type="Notice"
		public void ProcessNoticeZaciatokPokladne(MessageForm message)
		{
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

			case Mc.NoticePrestavkaZaciatok:
				ProcessNoticePrestavkaZaciatok(message);
			break;

			case Mc.Finish:
				switch (message.Sender.Id)
				{
				case SimId.ProcessObsluhyPokladni:
					ProcessFinishProcessObsluhyPokladni(message);
				break;

				case SimId.SchedulerPrestavkaPokladne:
					ProcessFinishSchedulerPrestavkaPokladne(message);
				break;
				}
			break;

			case Mc.NoticeZaciatokPokladne:
				ProcessNoticeZaciatokPokladne(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new AgentPokladni MyAgent
		{
			get
			{
				return (AgentPokladni)base.MyAgent;
			}
		}
	}
}