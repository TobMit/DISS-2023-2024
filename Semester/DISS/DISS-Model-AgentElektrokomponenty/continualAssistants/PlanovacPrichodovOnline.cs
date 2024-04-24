using OSPABA;
using simulation;
using agents;
namespace continualAssistants
{
	//meta! id="77"
	public class PlanovacPrichodovOnline : Scheduler
	{
		public PlanovacPrichodovOnline(int id, Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="AgentOkolia", id="78", type="Start"
		public void ProcessStart(MessageForm message)
		{
			Constants.Log("PlanovacPrichodovOnline: ProcessStart", Constants.LogType.ContinualAssistantLog);
			var sprava = (MyMessage)message.CreateCopy();
			sprava.Code = Mc.NoticeNovyOnline;
			var newTime = ((MySimulation)MySim).RndPrichodZakaznikaOnline.Next();
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
			}
		}

		//meta! sender="AgentOkolia", id="86", type="Notice"
		public void ProcessNoticeNovyOnline(MessageForm message)
		{
			Constants.Log("PlanovacPrichodovOnline: ProcessNoticeNovyOnline", Constants.LogType.ContinualAssistantLog);
			var sprava = (MyMessage)message.CreateCopy();
			sprava.Addressee = MyAgent;
			Notice(sprava);
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
			case Mc.NoticeNovyOnline:
				ProcessNoticeNovyOnline(message);
			break;

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