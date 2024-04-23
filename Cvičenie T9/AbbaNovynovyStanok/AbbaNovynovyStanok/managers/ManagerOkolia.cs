using OSPABA;
using simulation;
using agents;
namespace managers
{
	//meta! id="3"
	public class ManagerOkolia : Manager
	{
		public ManagerOkolia(int id, Simulation mySim, Agent myAgent) :
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

		//meta! sender="PlanovacPrichodov", id="12", type="Finish"
		public void ProcessFinish(MessageForm message)
		{
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
			}
		}

		//meta! sender="AgentModelu", id="20", type="Notice"
		public void ProcessOdchodZakaznika(MessageForm message)
		{
		}

		//meta! sender="AgentModelu", id="23", type="Notice"
		public void ProcessInicializacia(MessageForm message)
		{
			message.Addressee = ((AgentOkolia)MyAgent).AsistentPrichodov;
			StartContinualAssistant(message);
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		public void Init()
		{
		}

		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
			case Mc.Inicializacia:
				ProcessInicializacia(message);
			break;

			case Mc.OdchodZakaznika:
				ProcessOdchodZakaznika(message);
			break;

			case Mc.Finish:
				ProcessFinish(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new AgentOkolia MyAgent
		{
			get
			{
				return (AgentOkolia)base.MyAgent;
			}
		}
	}
}