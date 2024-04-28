using OSPABA;
using simulation;
using agents;
namespace continualAssistants
{
	//meta! id="106"
	public class ProcessOMPripravaTovaru : Process
	{
		public ProcessOMPripravaTovaru(int id, Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
		}

		public override void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="AgentObsluzneMiesto", id="107", type="Start"
		public void ProcessStart(MessageForm message)
		{
			var sprava = (MyMessage)message.CreateCopy();
			Constants.Log($"ProcessOMPripravaTovaru ({TimeSpan.FromSeconds(MySim.CurrentTime + Constants.START_DAY).ToString(@"hh\:mm\:ss")}): Zakaznik {sprava.Zakaznik.ID} ProcessStart", Constants.LogType.ContinualAssistantLog);
			sprava.Code = Mc.Finish;
			double trvanie = 0;
			switch (sprava.Zakaznik.TypNarocnostiTovaru)
			{
				case Constants.TypNarocnostiTovaru.Simple:
					trvanie = ((MySimulation)MySim).RndTrvaniePripravaSimple.Next();
					break;
				case Constants.TypNarocnostiTovaru.Normal:
					trvanie = ((MySimulation)MySim).RndTrvaniePripravaNormal.Next();
					break;
				case Constants.TypNarocnostiTovaru.Hard:
					trvanie = ((MySimulation)MySim).RndTrvaniePripravaHard.Next();
					break;
			}
			Hold(trvanie, sprava);
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
				case Mc.Finish:
					var sprava = (MyMessage)message.CreateCopy();
					Constants.Log($"ProcessOMPripravaTovaru {TimeSpan.FromSeconds(MySim.CurrentTime + Constants.START_DAY).ToString(@"hh\:mm\:ss")}: Zakaznik {sprava.Zakaznik.ID} KoniecObsluhy", Constants.LogType.ContinualAssistantLog);
					message.Addressee = MyAgent;
					AssistantFinished(message);
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