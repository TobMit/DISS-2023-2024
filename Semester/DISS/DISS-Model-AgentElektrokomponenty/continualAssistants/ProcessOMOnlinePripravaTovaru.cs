using OSPABA;
using simulation;
using agents;
namespace continualAssistants
{
	//meta! id="108"
	public class ProcessOMOnlinePripravaTovaru : Process
	{
		public ProcessOMOnlinePripravaTovaru(int id, Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="AgentObsluzneMiesto", id="109", type="Start"
		public void ProcessStart(MessageForm message)
		{
			var sprava = (MyMessage)message.CreateCopy();
			Constants.Log($"ProcessOMOnlinePripravaTovaru ({TimeSpan.FromSeconds(MySim.CurrentTime + Constants.START_DAY).ToString(@"hh\:mm\:ss")}): Zakaznik {sprava.Zakaznik.ID} ProcessStart", Constants.LogType.ContinualAssistantLog);
			sprava.Code = Mc.Finish;
			//sprava.ObsluzneMiesto.Obsluz(sprava.Zakaznik);
			Hold(((MySimulation)MySim).RndTrvanieDiktovania.Next(), sprava);
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
				case Mc.Finish:
					var sprava = (MyMessage)message.CreateCopy();
					Constants.Log($"ProcessOMOninePripravaTovaru {TimeSpan.FromSeconds(MySim.CurrentTime + Constants.START_DAY).ToString(@"hh\:mm\:ss")}: Zakaznik {sprava.Zakaznik.ID} KoniecObsluhy", Constants.LogType.ContinualAssistantLog);
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