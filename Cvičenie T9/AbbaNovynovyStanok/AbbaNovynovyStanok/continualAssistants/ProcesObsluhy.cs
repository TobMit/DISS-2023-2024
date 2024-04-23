using AbbaNovynovyStanok.simulation;
using OSPABA;
using simulation;
using agents;
namespace continualAssistants
{
	//meta! id="14"
	public class ProcesObsluhy : Process
	{
		public ProcesObsluhy(int id, Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="AgentStanku", id="15", type="Start"
		public void ProcessStart(MessageForm message)
		{
			// Console.WriteLine("ProcesObsluhy: ProcessStart");
			var sprava = (MyMessage)message.CreateCopy();
			sprava.Code = Mc.NoticeKoniecObsluhy;
			Hold(((MySimulation)MySim).GeneratorCasovObsluhy.Next(), sprava);
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
			}
		}

		//meta! sender="AgentStanku", id="34", type="Notice"
		public void ProcessNoticeKoniecObsluhy(MessageForm message)
		{
			// Console.WriteLine("ProcesObsluhy: ProcessNoticeKoniecObsluhy");
			message.Addressee = MyAgent;
			AssistantFinished(message);
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
			case Mc.Start:
				ProcessStart(message);
			break;

			case Mc.NoticeKoniecObsluhy:
				ProcessNoticeKoniecObsluhy(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new AgentStanku MyAgent
		{
			get
			{
				return (AgentStanku)base.MyAgent;
			}
		}
	}
}