using OSPABA;
using simulation;
using agents;
using managers;

namespace continualAssistants
{
	//meta! id="63"
	public class SchedulerPrestavkaOM : Scheduler
	{
		public SchedulerPrestavkaOM(int id, Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
		}

		public override void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="AgentObsluzneMiesto", id="64", type="Start"
		public void ProcessStart(MessageForm message)
		{
			var sprava = (MyMessage)message.CreateCopy();
			Constants.Log("SchedulerPrestavkaOM", MySim.CurrentTime, null,"ProcessStart", Constants.LogType.ContinualAssistantLog);
			sprava.Code = Mc.Finish;
			Hold(Constants.STAR_BREAK, sprava);
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
				case Mc.Finish:
					var sprava = (MyMessage)message.CreateCopy();
					Constants.Log("SchedulerPrestavkaOM", MySim.CurrentTime, null,"ProcessFinishSchedulerPrestavkaOM", Constants.LogType.ContinualAssistantLog);
					((ManagerObsluzneMiesto)MyAgent.MyManager).Break = true;
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
		public new AgentObsluzneMiesto MyAgent
		{
			get
			{
				return (AgentObsluzneMiesto)base.MyAgent;
			}
		}
	}
}