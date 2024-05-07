using OSPABA;
using simulation;
using agents;
using continualAssistants;
using DISS_Model_AgentElektrokomponenty.Entity;
using instantAssistants;
namespace managers
{
	//meta! id="2"
	public class ManagerOkolia : Manager
	{
		public ManagerOkolia(int id, Simulation mySim, Agent myAgent) :
			base(id, mySim, myAgent)
		{
			Init();
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication

			if (PetriNet != null)
			{
				PetriNet.Clear();
			}
		}

		//meta! sender="AgentModelu", id="19", type="Notice"
		public void ProcessInit(MessageForm message)
		{
			var sprava = (MyMessage)message.CreateCopy();
			Constants.Log("ManagerOkolia", MySim.CurrentTime, sprava.Zakaznik,"ProcessInit", Constants.LogType.ManagerLog);
			sprava.Addressee = MyAgent.FindAssistant(SimId.PlanovacPrichodovBasic);
			StartContinualAssistant(new MyMessage(MySim, null){Addressee = MyAgent.FindAssistant(SimId.PlanovacPrichodovBasic)});
			
			sprava.Addressee = MyAgent.FindAssistant(SimId.PlanovacPrichodovZmluvny);
			StartContinualAssistant(new MyMessage(sprava));
			
			sprava.Addressee = MyAgent.FindAssistant(SimId.PlanovacPrichodovOnline);
			StartContinualAssistant(new MyMessage(sprava));
		}

		//meta! sender="AgentModelu", id="17", type="Notice"
		public void ProcessNoticeOdchodZakaznika(MessageForm message)
		{
			var sprava = (MyMessage)message.CreateCopy();
			Constants.Log("ManagerOkolia", MySim.CurrentTime, sprava.Zakaznik,"ProcessNoticeOdchodZakaznika", Constants.LogType.ManagerLog);
			if (sprava.Zakaznik.StavZakaznika == Constants.StavZakaznika.OdišielZPredajne)
			{
				throw new InvalidOperationException("Zakaznik už odyšiel");
			}
			sprava.Zakaznik.StavZakaznika = Constants.StavZakaznika.OdišielZPredajne;
			((MySimulation)MySim).PocetObsluzenychZakaznikov++;
			
			((MySimulation)MySim).StatPriemernyCasVObchode.AddSample(MySim.CurrentTime - sprava.Zakaznik.TimeOfArrival);
			if (MySim.CurrentTime >= Constants.END_ARRIVAL_SIMULATION_TIME && ((MySimulation)MySim).CelkovyPocetZakaznikov == ((MySimulation)MySim).PocetObsluzenychZakaznikov + ((MySimulation)MySim).PocetVyhodenychZakaznikov)
			{
				MySim.StopReplication();
			}
		}

		//meta! sender="PlanovacPrichodovOnline", id="78", type="Finish"
		public void ProcessFinishPlanovacPrichodovOnline(MessageForm message)
		{
		}

		//meta! sender="PlanovacPrichodovZmluvny", id="76", type="Finish"
		public void ProcessFinishPlanovacPrichodovZmluvny(MessageForm message)
		{
		}

		//meta! sender="PlanovacPrichodovBasic", id="8", type="Finish"
		public void ProcessFinishPlanovacPrichodovBasic(MessageForm message)
		{
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			Person tmpPerson = new();
			((MySimulation)MySim).Persons.Add(tmpPerson);
			var sprava = (MyMessage)message.CreateCopy();
			sprava.Zakaznik = tmpPerson;
			sprava.Addressee = MySim.FindAgent(SimId.AgentModelu);
			sprava.Code = Mc.NoticePrichodZakaznika;
			
			AgentComponent? address = this;
			
			switch (message.Code)
			{
				case Mc.NoticeNovyBasic:
					Constants.Log("ManagerOkolia", MySim.CurrentTime, sprava.Zakaznik,"ProcessDefault - NoticeNovyBasic", Constants.LogType.ManagerLog);
					tmpPerson.TypZakaznika = Constants.TypZakaznika.Basic;
					address = MyAgent.FindAssistant(SimId.PlanovacPrichodovBasic);
					break;
				
				case Mc.NoticeNovyZmluvny:
					Constants.Log("ManagerOkolia", MySim.CurrentTime, sprava.Zakaznik,"ProcessDefault - NoticeNovyBasic", Constants.LogType.ManagerLog);
					tmpPerson.TypZakaznika = Constants.TypZakaznika.Zmluvný;
					address = MyAgent.FindAssistant(SimId.PlanovacPrichodovZmluvny);
					break;
				
				case Mc.NoticeNovyOnline:
					Constants.Log("ManagerOkolia", MySim.CurrentTime, sprava.Zakaznik,"ProcessDefault - NoticeNovyBasic", Constants.LogType.ManagerLog);
					tmpPerson.TypZakaznika = Constants.TypZakaznika.Online;
					address = MyAgent.FindAssistant(SimId.PlanovacPrichodovOnline);
					break;
			}
			Notice(sprava);
			
			var NewSprava = new MyMessage(MySim, null){Addressee = address};
			
			// Ak nie sme v debugu tak sa budu generovať viacerý zákazníci
			if (!Constants.DEBUG)
			{
				StartContinualAssistant(NewSprava);
			}
		}

		//meta! sender="SchedulerReklamacia", id="156", type="Finish"
		public void ProcessFinishSchedulerReklamacia(MessageForm message)
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
			case Mc.Init:
				ProcessInit(message);
			break;

			case Mc.Finish:
				switch (message.Sender.Id)
				{
				case SimId.PlanovacPrichodovBasic:
					ProcessFinishPlanovacPrichodovBasic(message);
				break;

				case SimId.PlanovacPrichodovZmluvny:
					ProcessFinishPlanovacPrichodovZmluvny(message);
				break;

				case SimId.PlanovacPrichodovOnline:
					ProcessFinishPlanovacPrichodovOnline(message);
				break;

				case SimId.SchedulerReklamacia:
					ProcessFinishSchedulerReklamacia(message);
				break;
				}
			break;

			case Mc.NoticeOdchodZakaznika:
				ProcessNoticeOdchodZakaznika(message);
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