using OSPABA;
using simulation;
using agents;
namespace continualAssistants
{
	//meta! id="7"
	public class PlanovacPrichodovBasic : Scheduler
	{
		public PlanovacPrichodovBasic(int id, Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
		}

		public override void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="AgentOkolia", id="80", type="Notice"
		public void ProcessNoticeNovyBasic(MessageForm message)
		{
			var sprava = (MyMessage)message.CreateCopy();
			Constants.Log("PlanovacPrichodovBasic", MySim.CurrentTime, sprava.Zakaznik,"ProcessNoticeNovyBasic", Constants.LogType.ContinualAssistantLog);
			sprava.Addressee = MyAgent;
			Notice(sprava);
		}

		//meta! sender="AgentOkolia", id="8", type="Start"
		public void ProcessStart(MessageForm message)
		{
			var sprava = (MyMessage)message.CreateCopy();
			Constants.Log("PlanovacPrichodovBasic", MySim.CurrentTime, sprava.Zakaznik,"ProcessStart", Constants.LogType.ContinualAssistantLog);
			sprava.Code = Mc.NoticeNovyBasic;
			var newTime = ((MySimulation)MySim).RndPrichodZakaznikaBasic.Next();
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

		//meta! userInfo="Generated code: do not modify", tag="begin"
		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
			case Mc.Start:
				ProcessStart(message);
			break;

			case Mc.NoticeNovyBasic:
				ProcessNoticeNovyBasic(message);
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