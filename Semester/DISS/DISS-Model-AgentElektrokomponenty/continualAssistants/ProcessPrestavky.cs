using OSPABA;
using simulation;
using agents;
namespace continualAssistants
{
	//meta! id="138"
	public class ProcessPrestavky : Process
	{
		public ProcessPrestavky(int id, Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
		}

		public override void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="AgentPokladni", id="139", type="Start"
		public void ProcessStart(MessageForm message)
		{
			var sprava = (MyMessage)message.CreateCopy();
			Constants.Log("ProcessPrestavky", MySim.CurrentTime, null, $"Pokladna {sprava.Pokladna?.ID} ProcessStart", Constants.LogType.ContinualAssistantLog);
			sprava.Code = Mc.Finish;
			Hold(Constants.BREAK_DURATION, sprava);
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
				case Mc.Finish:
					var sprava = (MyMessage)message.CreateCopy();
					Constants.Log("ProcessPrestavky", MySim.CurrentTime, null, $"Pokladna {sprava.Pokladna?.ID} ProcessFinish", Constants.LogType.ContinualAssistantLog);
					AssistantFinished(sprava);
					break;
			}
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
			case Mc.Start:
				ProcessStart(message);
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