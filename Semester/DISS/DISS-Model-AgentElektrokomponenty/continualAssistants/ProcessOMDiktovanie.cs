using OSPABA;
using simulation;
using agents;
namespace continualAssistants
{
	//meta! id="61"
	public class ProcessOMDiktovanie : Process
	{
		public ProcessOMDiktovanie(int id, Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
		}

		public override void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="AgentObsluzneMiesto", id="62", type="Start"
		public void ProcessStart(MessageForm message)
		{
			var sprava = (MyMessage)message.CreateCopy();
			Constants.Log("ProcessOMDiktovanie", MySim.CurrentTime, sprava.Zakaznik,"ProcessStart", Constants.LogType.ContinualAssistantLog);
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
					Constants.Log("ProcessOMDiktovanie", MySim.CurrentTime, sprava.Zakaznik,"KoniecObsluhy", Constants.LogType.ContinualAssistantLog);
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