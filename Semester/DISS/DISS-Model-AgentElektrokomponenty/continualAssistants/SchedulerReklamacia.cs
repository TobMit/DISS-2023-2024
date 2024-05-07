using OSPABA;
using simulation;
using agents;
namespace continualAssistants
{
	//meta! id="155"
	public class SchedulerReklamacia : Scheduler
	{
		public SchedulerReklamacia(int id, Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
		}

		public override void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="AgentOkolia", id="156", type="Start"
		public void ProcessStart(MessageForm message)
		{
			var sprava = (MyMessage)message.CreateCopy();
			Constants.Log("SchedulerReklamacia", MySim.CurrentTime,null,"ProcessStart", Constants.LogType.ContinualAssistantLog);
			sprava.Code = Mc.Finish;
			var newTime = ((MySimulation)MySim).RndPrichodZakaznikaReklamacia.Next();
			if (newTime + MySim.CurrentTime < Constants.END_ARRIVAL_SIMULATION_TIME)
			{
				Hold(newTime, sprava);	
			}
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
				case Mc.Finish:
					var sprava = (MyMessage)message.CreateCopy();
					Constants.Log("SchedulerReklamacia", MySim.CurrentTime,null,"ProcessFinish", Constants.LogType.ContinualAssistantLog);
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
		public new AgentOkolia MyAgent
		{
			get
			{
				return (AgentOkolia)base.MyAgent;
			}
		}
	}
}
