using OSPABA;
using simulation;
using agents;
namespace continualAssistants
{
	//meta! id="119"
	public class ProcessVyzdvihnutieTovaru : Process
	{
		public ProcessVyzdvihnutieTovaru(int id, Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
		}

		public override void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="AgentObsluzneMiesto", id="120", type="Start"
		public void ProcessStart(MessageForm message)
		{
			var sprava = (MyMessage)message.CreateCopy();
			Constants.Log($"ProcessVyzdvihnutieTovaru: Zakaznik {sprava.Zakaznik.ID} ProcessStart", Constants.LogType.ContinualAssistantLog);
			sprava.Code = Mc.Finish;
			sprava.Zakaznik.StavZakaznika = Constants.StavZakaznika.ObslužnéMiestoVraciaSaPreVeľkýTovar;
			Hold(((MySimulation)MySim).RndTrvanieVyzdvyhnutieVelkehoTovaru.Next(), sprava);
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
				case Mc.Finish:
					var sprava = (MyMessage)message.CreateCopy();
					Constants.Log($"ProcessVyzdvihnutieTovaru: Zakaznik {sprava.Zakaznik.ID} ProcessFinish", Constants.LogType.ContinualAssistantLog);
					sprava.ObsluzneMiesto.Uvolni(false); // keď počítam vyťaženie človeka tak je tuto false lebo človek už je voľný
					sprava.Addressee = MyAgent;
					sprava.TovarVydvihnty = true;
					AssistantFinished(sprava);
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
