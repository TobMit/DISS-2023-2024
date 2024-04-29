using OSPABA;
using simulation;
using agents;
namespace continualAssistants
{
	//meta! id="54"
	public class SchedulerZatvorenieAutomatu : Scheduler
	{
		public SchedulerZatvorenieAutomatu(int id, Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="AgentAutomatu", id="55", type="Start"
		public void ProcessStart(MessageForm message)
		{
			var sprava = (MyMessage)message.CreateCopy();
			Constants.Log("SchedulerZatvorenieAutomatu", MySim.CurrentTime, sprava.Zakaznik,"ProcessStart", Constants.LogType.ContinualAssistantLog);
			//sprava.Addressee = MyAgent;
			sprava.Code = Mc.NoticeZatvorenieAutomatu;
			Hold(Constants.END_ARRIVAL_SIMULATION_TIME, sprava);

		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
				case Mc.NoticeZatvorenieAutomatu:
					var sprava = (MyMessage)message;
					Constants.Log("SchedulerZatvorenieAutomatu", MySim.CurrentTime, sprava.Zakaznik,"NoticeZatvorenieAutomatu", Constants.LogType.ContinualAssistantLog);
					Notice(new MyMessage((MyMessage)message));
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
		public new AgentAutomatu MyAgent
		{
			get
			{
				return (AgentAutomatu)base.MyAgent;
			}
		}
	}
}