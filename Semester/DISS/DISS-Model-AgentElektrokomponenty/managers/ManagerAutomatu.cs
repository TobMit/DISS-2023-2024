using OSPABA;
using simulation;
using agents;
using continualAssistants;
using DISS_Model_AgentElektrokomponenty.Entity;
using instantAssistants;
using OSPDataStruct;
using OSPStat;

namespace managers
{
	//meta! id="25"
	public class ManagerAutomatu : Manager
	{
		//Id of the people in AgentAutomat
		public int Id { get; set; }
		public bool Obsluhuje { get; set; }

		private Person? _person;
		
		public SimQueue<MyMessage> Front { get; set; }
		public ManagerAutomatu(int id, Simulation mySim, Agent myAgent) :
			base(id, mySim, myAgent)
		{
			Init();
		}

		public override void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
			Front = new(((MySimulation)MySim).StatPriemernaDlzkaRaduPredAutomatom);
			if (PetriNet != null)
			{
				PetriNet.Clear();
			}
		}

		//meta! sender="ProcessObsluhaAutomatu", id="33", type="Finish"
		public void ProcessFinishProcessObsluhaAutomatu(MessageForm message)
		{
			var sprava = (MyMessage)message.CreateCopy();
			Constants.Log("ManagerAutomatu", MySim.CurrentTime, sprava.Zakaznik,"ProcessFinishProcessObsluhaAutomatu", Constants.LogType.ManagerLog);
			if (!Front.IsEmpty())
			{
				var newSprava = new MyMessage(MySim, null)
				{
					Addressee = MySim.FindAgent(SimId.AgentPredajne),
					Code = Mc.PocetMiestVRade,
					SimpleMessage = true
				};
				Request(newSprava);
			}
			else
			{
				Obsluhuje = false;
				_person = null;
				((MySimulation)MySim).StatVyuzitieAutomatu.AddSample(0);
			}

			sprava.Addressee = MySim.FindAgent(SimId.AgentPredajne);
			sprava.Code = Mc.NoticeKoniecObsluhy;
			Obsluhuje = false;
			_person = null;
			Notice(sprava);
		}

		//meta! sender="SchedulerZatvorenieAutomatu", id="55", type="Finish"
		public void ProcessFinishSchedulerZatvorenieAutomatu(MessageForm message)
		{
			Constants.Log("ManagerAutomatu", MySim.CurrentTime, new (),"ProcessFinishSchedulerZatvorenieAutomatu", Constants.LogType.ManagerLog);
			
			//todo add logics + statistics
			while (!Front.IsEmpty())
			{
				var spravaNew = Front.Dequeue();
				spravaNew.Zakaznik.StavZakaznika = Constants.StavZakaznika.OdišielZPredajne;
			}
		}

		//meta! sender="AgentPredajne", id="34", type="Notice"
		public void ProcessNoticeZaciatokObsluhy(MessageForm message)
		{
			var sprava = (MyMessage)message.CreateCopy();
			Constants.Log("ManagerAutomatu", MySim.CurrentTime, sprava.Zakaznik,"ProcessNoticeZaciatokObsluhy", Constants.LogType.ManagerLog);
			sprava.Zakaznik.ID = Id++;
			sprava.Zakaznik.TimeOfArrival = MySim.CurrentTime; 
			sprava.Zakaznik.SetTypNarocnostiTovaru(((MySimulation)MySim).RndTypNarocnostTovaru.Next());
			sprava.Zakaznik.SetTypVelkostiNakladu(((MySimulation)MySim).RndTypVelkostiNakladu.Next());
			if (Front.Count > 0)
			{
				Constants.Log("ManagerAutomatu", MySim.CurrentTime, sprava.Zakaznik,"Pridané do frontu", Constants.LogType.ManagerLog);
				sprava.Zakaznik.StavZakaznika = Constants.StavZakaznika.RadPredAutomatom;
				Front.Enqueue(sprava);
			}
			else if (Obsluhuje)
			{
				Constants.Log("ManagerAutomatu", MySim.CurrentTime, sprava.Zakaznik,"Pridané do frontu", Constants.LogType.ManagerLog);
				sprava.Zakaznik.StavZakaznika = Constants.StavZakaznika.RadPredAutomatom;
				Front.Enqueue(sprava);
			}
			else if (sprava.PocetLudiVOM >= Constants.RADA_PRED_OBSLUZNYM_MIESTOM)
			{
				Constants.Log("ManagerAutomatu", MySim.CurrentTime, sprava.Zakaznik,"Pridané do frontu", Constants.LogType.ManagerLog);
				sprava.Zakaznik.StavZakaznika = Constants.StavZakaznika.RadPredAutomatom;
				Front.Enqueue(sprava);
			}
			else
			{
				sprava.Addressee = MyAgent.FindAssistant(SimId.ProcessObsluhaAutomatu);
				StartContinualAssistant(sprava);
				Obsluhuje = true;
				_person = sprava.Zakaznik;
				((MySimulation)MySim).StatCasStravenyPredAutomatom.AddSample(MySim.CurrentTime - sprava.Zakaznik.TimeOfArrival);
				((MySimulation)MySim).StatVyuzitieAutomatu.AddSample(1);
			}
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
			}
		}

		//meta! sender="AgentPredajne", id="91", type="Notice"
		public void ProcessInit(MessageForm message)
		{
			Constants.Log("ManagerAutomatu", MySim.CurrentTime, null,"ProcessInit", Constants.LogType.ManagerLog);
			Id = 0;
			Front.Clear();
			Obsluhuje = false;
			var sprava = (MyMessage)message.CreateCopy();
			sprava.Addressee = MyAgent.FindAssistant(SimId.SchedulerZatvorenieAutomatu);
			StartContinualAssistant(sprava);
		}

		//meta! sender="SchedulerZatvorenieAutomatu", id="94", type="Notice"
		public void ProcessNoticeZatvorenieAutomatu(MessageForm message)
		{
			Constants.Log("ManagerAutomatu", MySim.CurrentTime, null,"ProcessNoticeZatvorenieAutomatu", Constants.LogType.ManagerLog);
			//todo add logic
		}

		//meta! sender="ProcessObsluhaAutomatu", id="98", type="Notice"
		public void ProcessNoticeKoniecObsluhy(MessageForm message)
		{
		}


		//meta! sender="AgentPredajne", id="132", type="Response"
		public void ProcessPocetMiestVRade(MessageForm message)
		{
			var sprava = (MyMessage)message.CreateCopy();
			if (sprava.PocetLudiVOM <= Constants.RADA_PRED_OBSLUZNYM_MIESTOM && Obsluhuje == false)
			{
				var newSprava = new MyMessage(Front.Dequeue());
				((MySimulation)MySim).StatCasStravenyPredAutomatom.AddSample(MySim.CurrentTime - newSprava.Zakaznik.TimeOfArrival);
				newSprava.Addressee = MyAgent.FindAssistant(SimId.ProcessObsluhaAutomatu);
				_person = newSprava.Zakaznik;
				Obsluhuje = true;
				StartContinualAssistant(newSprava);	
			}
		}

		//meta! sender="AgentPredajne", id="135", type="Notice"
		public void ProcessNoticeUvolnenieRadu(MessageForm message)
		{
			var sprava = (MyMessage)message.CreateCopy();
			Constants.Log("ManagerAutomatu", MySim.CurrentTime, null,"ProcessNoticeUvolnenieRadu", Constants.LogType.ManagerLog);
			if (Obsluhuje)
			{
				return;
			}
			//todo chcek či nie je viac ľudí v rade ako je maximálny počet
			if (Front.Count > 0)
			{
				var newSprava = new MyMessage(Front.Dequeue());
				Obsluhuje = true;
				((MySimulation)MySim).StatCasStravenyPredAutomatom.AddSample(MySim.CurrentTime - newSprava.Zakaznik.TimeOfArrival);
				newSprava.Addressee = MyAgent.FindAssistant(SimId.ProcessObsluhaAutomatu);
				_person = newSprava.Zakaznik;
				StartContinualAssistant(newSprava);
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
			case Mc.Init:
				ProcessInit(message);
			break;

			case Mc.NoticeZatvorenieAutomatu:
				ProcessNoticeZatvorenieAutomatu(message);
			break;

			case Mc.Finish:
				switch (message.Sender.Id)
				{
				case SimId.ProcessObsluhaAutomatu:
					ProcessFinishProcessObsluhaAutomatu(message);
				break;

				case SimId.SchedulerZatvorenieAutomatu:
					ProcessFinishSchedulerZatvorenieAutomatu(message);
				break;
				}
			break;

			case Mc.NoticeZaciatokObsluhy:
				ProcessNoticeZaciatokObsluhy(message);
			break;

			case Mc.NoticeUvolnenieRadu:
				ProcessNoticeUvolnenieRadu(message);
			break;

			case Mc.NoticeKoniecObsluhy:
				ProcessNoticeKoniecObsluhy(message);
			break;

			case Mc.PocetMiestVRade:
				ProcessPocetMiestVRade(message);
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
		
		public string GuiToString()
		{
			double vytaznie = 0;
			if (((MySimulation)MySim).StatVyuzitieAutomatu.SampleSize > 2)
			{
				vytaznie = ((MySimulation)MySim).StatVyuzitieAutomatu.Mean() * 100;
			}
			double ldzkaRadu = 0;
			if (((MySimulation)MySim).StatPriemernaDlzkaRaduPredAutomatom.SampleSize > 0)
			{
				ldzkaRadu = ((MySimulation)MySim).StatPriemernaDlzkaRaduPredAutomatom.Mean();
			}
			if (_person is null)
			{
				return $"Automat: \n\t- Voľný \n\t- Vyťaženie: {vytaznie:0.00}%\n\t- Dĺžka radu: {ldzkaRadu:0.00}";
			}
			return $"Automat: \n\t- Stojí Person: {_person?.ID}\n\t- Vyťaženie: {vytaznie:0.00}%\n\t- Dĺžka radu: {ldzkaRadu:0.00}";
		}
	}
}