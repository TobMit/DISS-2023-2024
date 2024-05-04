using OSPABA;
using simulation;
using agents;
using managers;

namespace continualAssistants
{
	//meta! id="56"
	public class SchedulerPrestavkaPokladne : Scheduler
	{
		public SchedulerPrestavkaPokladne(int id, Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
		}

		public override void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="AgentPokladni", id="57", type="Start"
		public void ProcessStart(MessageForm message)
		{
			var sprava = (MyMessage)message.CreateCopy();
			Constants.Log("SchedulerPrestavkaPokladne", MySim.CurrentTime, null, "ProcessStart", Constants.LogType.ContinualAssistantLog);
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
					Constants.Log("SchedulerPrestavkaPokladne", MySim.CurrentTime, null, "ProcessFinishSchedulerPrestavkaPokladne", Constants.LogType.ContinualAssistantLog);
					((ManagerPokladni)MyAgent.MyManager).Break = true;
					//todo všetky neobsadené pokladne idú rovno na prestávku
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