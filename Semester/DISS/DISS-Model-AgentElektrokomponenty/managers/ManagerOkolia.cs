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
			switch (message.Code)
			{
				case Mc.NoticeNovyBasic:
					Constants.Log("ManagerOkolia: ProcessDefault: NoticeNovyBasic", Constants.LogType.ManagerLog);
					var spravaBasic = (MyMessage)message.CreateCopy();
					spravaBasic.Addressee = MySim.FindAgent(SimId.AgentModelu);
					spravaBasic.Code = Mc.NoticePrichodZakaznika;
					tmpPerson.TypZakaznika = Constants.TypZakaznika.Basic;
					spravaBasic.Zakaznik = tmpPerson;
					Notice(new MyMessage(spravaBasic));
					
					spravaBasic.Addressee = MyAgent.FindAssistant(SimId.PlanovacPrichodovBasic);
					StartContinualAssistant(new MyMessage(spravaBasic));
					break;
				
				case Mc.NoticeNovyZmluvny:
					Constants.Log("ManagerOkolia: ProcessDefault: NoticeNovyBasic", Constants.LogType.ManagerLog);
					var spravaZmluvny = (MyMessage)message.CreateCopy();
					spravaZmluvny.Addressee = MySim.FindAgent(SimId.AgentModelu);
					spravaZmluvny.Code = Mc.NoticePrichodZakaznika;
					tmpPerson.TypZakaznika = Constants.TypZakaznika.Zmluvný;
					spravaZmluvny.Zakaznik = tmpPerson;
					Notice(new MyMessage(spravaZmluvny));
					
					spravaZmluvny.Addressee = MyAgent.FindAssistant(SimId.PlanovacPrichodovZmluvny);
					StartContinualAssistant(new MyMessage(spravaZmluvny));
					break;
				case Mc.NoticeNovyOnline:
					Constants.Log("ManagerOkolia: ProcessDefault: NoticeNovyBasic", Constants.LogType.ManagerLog);
					var spravaOnline = (MyMessage)message.CreateCopy();
					spravaOnline.Addressee = MySim.FindAgent(SimId.AgentModelu);
					spravaOnline.Code = Mc.NoticePrichodZakaznika;
					tmpPerson.TypZakaznika = Constants.TypZakaznika.Online;
					spravaOnline.Zakaznik = tmpPerson;
					Notice(new MyMessage(spravaOnline));
					
					spravaOnline.Addressee = MyAgent.FindAssistant(SimId.PlanovacPrichodovOnline);
					StartContinualAssistant(new MyMessage(spravaOnline));
					break;
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

			case Mc.NoticeOdchodZakaznika:
				ProcessNoticeOdchodZakaznika(message);
			break;

			case Mc.Init:
				ProcessInit(message);
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