using OSPABA;
using simulation;
using agents;
namespace managers
{
	//meta! id="4"
	public class ManagerStanku : Manager
	{
		public ManagerStanku(int id, Simulation mySim, Agent myAgent) :
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

		//meta! sender="ProcesObsluhy", id="15", type="Finish"
		public void ProcessFinish(MessageForm message)
		{
		}

		//meta! sender="AgentModelu", id="9", type="Request"
		public void ProcessObsluha(MessageForm message)
		{
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
			case Mc.Obsluha:
				ProcessObsluha(message);
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
		public new AgentStanku MyAgent
		{
			get
			{
				return (AgentStanku)base.MyAgent;
			}
		}
	}
}
