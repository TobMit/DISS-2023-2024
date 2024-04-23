using AbbaNovynovyStanok.simulation;
using OSPABA;
using simulation;
using agents;
namespace continualAssistants
{
	//meta! id="11"
	public class PlanovacPrichodov : Scheduler
	{
		public PlanovacPrichodov(int id, Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="AgentOkolia", id="12", type="Start"
		public void ProcessStart(MessageForm message)
		{
			Console.WriteLine("PlanovacPrichodov: ProcessStart");
			var sprava = (MyMessage)message.CreateCopy();
			sprava.Code = Mc.NoticeNovyZakaznik;
			Hold(((MySimulation)MySim).GeneratorCasovPrichodov.Next(),sprava);
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
				case Mc.NoticeNovyZakaznik:
					Console.WriteLine("PlanovacPrichodov: NoticeNovyZakaznik");
					var sprava = (MyMessage)message.CreateCopy();
					sprava.Addressee = MyAgent;
					Notice(sprava);
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