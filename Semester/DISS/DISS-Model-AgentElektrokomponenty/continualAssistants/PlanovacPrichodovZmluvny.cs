using OSPABA;
using simulation;
using agents;
namespace continualAssistants
{
	//meta! id="75"
	public class PlanovacPrichodovZmluvny : Scheduler
	{
		public PlanovacPrichodovZmluvny(int id, Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="AgentOkolia", id="76", type="Start"
		public void ProcessStart(MessageForm message)
		{
			var sprava = (MyMessage)message.CreateCopy();
			Constants.Log("PlanovacPrichodovZmluvny", MySim.CurrentTime, sprava.Zakaznik,"ProcessStart", Constants.LogType.ContinualAssistantLog);
			sprava.Code = Mc.NoticeNovyZmluvny;
			var newTime = ((MySimulation)MySim).RndPrichodZakaznikaZmluvny.Next();
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

		//meta! sender="AgentOkolia", id="85", type="Notice"
		public void ProcessNoticeNovyZmluvny(MessageForm message)
		{
			var sprava = (MyMessage)message.CreateCopy();
			Constants.Log("PlanovacPrichodovZmluvny", MySim.CurrentTime, sprava.Zakaznik,"ProcessNoticeNovyZmluvny", Constants.LogType.ContinualAssistantLog);
			sprava.Addressee = MyAgent;
			Notice(sprava);
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
			case Mc.Start:
				ProcessStart(message);
			break;

			case Mc.NoticeNovyZmluvny:
				ProcessNoticeNovyZmluvny(message);
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