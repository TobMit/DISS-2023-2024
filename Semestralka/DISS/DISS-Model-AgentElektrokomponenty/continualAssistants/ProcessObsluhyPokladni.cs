using OSPABA;
using simulation;
using agents;
namespace continualAssistants
{
	//meta! id="52"
	public class ProcessObsluhyPokladni : Process
	{
		public ProcessObsluhyPokladni(int id, Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="AgentPokladni", id="53", type="Start"
		public void ProcessStart(MessageForm message)
		{
			var sprava = (MyMessage)message.CreateCopy();
			Constants.Log("ProcessObsluhyPokladni", MySim.CurrentTime, sprava.Zakaznik,"ProcessStart", Constants.LogType.ContinualAssistantLog);
			sprava.Code = Mc.Finish;
			Hold(((MySimulation)MySim).RndTrvaniePladba.Next(), sprava);
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
				case Mc.Finish:
					var sprava = (MyMessage)message.CreateCopy();
					Constants.Log("ProcessObsluhyPokladni", MySim.CurrentTime, sprava.Zakaznik,"ProcessFinish", Constants.LogType.ContinualAssistantLog);
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