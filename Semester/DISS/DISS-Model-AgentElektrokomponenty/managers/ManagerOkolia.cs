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
			Constants.Log("ManagerOkolia: ProcessInit", Constants.LogType.ManagerLog);
			var sprava = (MyMessage)message.CreateCopy();
			sprava.Addressee = MyAgent.FindAssistant(SimId.PlanovacPrichodovBasic);
			StartContinualAssistant(new MyMessage(sprava));
			
			sprava.Addressee = MyAgent.FindAssistant(SimId.PlanovacPrichodovZmluvny);
			StartContinualAssistant(new MyMessage(sprava));
			
			sprava.Addressee = MyAgent.FindAssistant(SimId.PlanovacPrichodovOnline);
			StartContinualAssistant(new MyMessage(sprava));
		}

		//meta! sender="AgentModelu", id="17", type="Notice"
		public void ProcessNoticeOdchodZakaznika(MessageForm message)
		{
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
			
			switch (message.Code)
			{
				case Mc.NoticeNovyBasic:
					Constants.Log("ManagerOkolia: ProcessDefault: NoticeNovyBasic", Constants.LogType.ManagerLog);
					sprava.Addressee = MySim.FindAgent(SimId.AgentModelu);
					sprava.Code = Mc.NoticePrichodZakaznika;
					tmpPerson.TypZakaznika = Constants.TypZakaznika.Basic;
					sprava.Zakaznik = tmpPerson;
					Notice(new MyMessage(sprava));
					
					sprava.Addressee = MyAgent.FindAssistant(SimId.PlanovacPrichodovBasic);
					break;
				
				case Mc.NoticeNovyZmluvny:
					Constants.Log("ManagerOkolia: ProcessDefault: NoticeNovyBasic", Constants.LogType.ManagerLog);
					sprava.Addressee = MySim.FindAgent(SimId.AgentModelu);
					sprava.Code = Mc.NoticePrichodZakaznika;
					tmpPerson.TypZakaznika = Constants.TypZakaznika.Zmluvný;
					sprava.Zakaznik = tmpPerson;
					Notice(new MyMessage(sprava));
					
					sprava.Addressee = MyAgent.FindAssistant(SimId.PlanovacPrichodovZmluvny);
					break;
				case Mc.NoticeNovyOnline:
					Constants.Log("ManagerOkolia: ProcessDefault: NoticeNovyBasic", Constants.LogType.ManagerLog);
					sprava.Addressee = MySim.FindAgent(SimId.AgentModelu);
					sprava.Code = Mc.NoticePrichodZakaznika;
					tmpPerson.TypZakaznika = Constants.TypZakaznika.Online;
					sprava.Zakaznik = tmpPerson;
					Notice(new MyMessage(sprava));
					
					sprava.Addressee = MyAgent.FindAssistant(SimId.PlanovacPrichodovOnline);
					break;
			}
			
			// Ak nie sme v debugu tak sa budu generovať viacerý zákazníci
			if (!Constants.DEBUG)
			{
				StartContinualAssistant(new MyMessage(sprava));
			}
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		public void Init()
		{
		}

		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
			case Mc.NoticeOdchodZakaznika:
				ProcessNoticeOdchodZakaznika(message);
			break;

			case Mc.Init:
				ProcessInit(message);
			break;

			case Mc.Finish:
				switch (message.Sender.Id)
				{
				case SimId.PlanovacPrichodovOnline:
					ProcessFinishPlanovacPrichodovOnline(message);
				break;

				case SimId.PlanovacPrichodovZmluvny:
					ProcessFinishPlanovacPrichodovZmluvny(message);
				break;

				case SimId.PlanovacPrichodovBasic:
					ProcessFinishPlanovacPrichodovBasic(message);
				break;
				}
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