using AbbaNovynovyStanok.simulation;
using OSPABA;
using simulation;
using agents;
using OSPDataStruct;

namespace managers
{
	//meta! id="4"
	public class ManagerStanku : Manager
	{
		public bool PokladnaObsadena { get; set; }
		public SimQueue<MyMessage> Front { get; set; }
		public ManagerStanku(int id, Simulation mySim, Agent myAgent) :
			base(id, mySim, myAgent)
		{
			Init();
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication

			PokladnaObsadena = false;
			Front = new();
			
			if (PetriNet != null)
			{
				PetriNet.Clear();
			}
		}

		//meta! sender="ProcesObsluhy", id="15", type="Finish"
		public void ProcessFinish(MessageForm message)
		{
			// Console.WriteLine("ManagerStanku: ProcessFinish");
			var sprava1 = (MyMessage)message.CreateCopy();
			sprava1.Code = Mc.NoticeKoniecObsluhy;
			Response(sprava1);

			if (Front.Count > 0)
			{
				// Console.WriteLine("ManagerStanku: Začal obsluhovať");
				var sprava = Front.Dequeue();
				((MySimulation)MySim).KumulativnyCasCakania += MySim.CurrentTime - sprava.ZaciatokCakania;
				
				sprava.Addressee = MyAgent.FindAssistant(SimId.ProcesObsluhy);
				StartContinualAssistant(sprava);
			}
			else
			{
				// Console.WriteLine("ManagerStanku: Pokladna uvolnena");
				PokladnaObsadena = false;
			}
		}

		//meta! sender="AgentModelu", id="9", type="Request"
		public void ProcessObsluha(MessageForm message)
		{
			// Console.WriteLine("ManagerStanku: ProcessObsluha");
			var sprava = (MyMessage)message.CreateCopy();
			sprava.ZaciatokCakania = MySim.CurrentTime;
			((MySimulation)MySim).KumulativnyPocetCakajucich++;
			if (PokladnaObsadena)
			{
				// Console.WriteLine("ManagerStanku: Pridané do frontu");
				Front.Enqueue(sprava);
			}
			else
			{
				// Console.WriteLine("ManagerStanku: Začal obsluhovať");
				PokladnaObsadena = true;
				sprava.Addressee = MyAgent.FindAssistant(SimId.ProcesObsluhy);
				StartContinualAssistant(sprava);
			}
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
			}
		}

		//meta! userInfo="Removed from model"
		public void ProcessNoticeKoniecObsluhy(MessageForm message)
		{
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		public void Init()
		{
		}

		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
			case Mc.Finish:
				ProcessFinish(message);
			break;

			case Mc.Obsluha:
				ProcessObsluha(message);
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