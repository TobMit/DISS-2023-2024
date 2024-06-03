using OSPABA;
using simulation;
using agents;
namespace continualAssistants
{
	//meta! id="32"
	public class ProcessObsluhaAutomatu : Process
	{
		public ProcessObsluhaAutomatu(int id, Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
		}

		public override void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="AgentAutomatu", id="33", type="Start"
		public void ProcessStart(MessageForm message)
		{
			var sprava = (MyMessage)message.CreateCopy();
			Constants.Log("ProcessObsluhaAutomatu", MySim.CurrentTime, sprava.Zakaznik,"ProcessStart", Constants.LogType.ContinualAssistantLog);
			//sprava.Addressee = MySim.FindAgent(SimId.AgentPredajne);
			sprava.Code = Mc.NoticeKoniecObsluhy;
			sprava.Zakaznik.StavZakaznika = Constants.StavZakaznika.ObsluhujeAutomat;
			Hold(((MySimulation)MySim).RndTrvanieAutomatu.Next(), sprava);
			
			
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
				case Mc.NoticeKoniecObsluhy:
					var sprava = (MyMessage)message;
					Constants.Log("ProcessObsluhaAutomatu", MySim.CurrentTime, sprava.Zakaznik,"ProcessNoticeKoniecObsluhy", Constants.LogType.ContinualAssistantLog);
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
		public new AgentAutomatu MyAgent
		{
			get
			{
				return (AgentAutomatu)base.MyAgent;
			}
		}
	}
}