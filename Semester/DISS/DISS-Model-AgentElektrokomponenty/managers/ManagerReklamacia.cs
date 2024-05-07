using OSPABA;
using simulation;
using agents;
using continualAssistants;
using instantAssistants;
namespace managers
{
	//meta! id="148"
	public class ManagerReklamacia : Manager
	{
		public List<Reklamacia> ListReklamacia { get; private set; }
		public ManagerReklamacia(int id, Simulation mySim, Agent myAgent) :
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
			ListReklamacia = new(Constants.POCET_PRACOVNIKOV_REKLAMACIE);
			for (int i = 0; i < Constants.POCET_PRACOVNIKOV_REKLAMACIE; i++)
			{
				ListReklamacia.Add(new(i));
			}
		}
		
		public Reklamacia? GetVolnaReklamaciaPrazdnyRad()
		{
			// vytvorí list kde je rad 0 a pokladňa nie je obsadená a nie je na prestávke
			var listPokladni = ListReklamacia
				.Where(p => !p.Obsadena && p.Queue.Count == 0 && !p.Break)
				.OrderBy(g => g.ID)
				.ToList();

			if (listPokladni.Count > 0)
			{
				return listPokladni[((MySimulation)MySim).RndPickReklamacia.Next(listPokladni.Count)];
			}

			return null;
		}
		
		public void PriradZakaznikaDoRady(MyMessage person)
		{
			// spraví list pokladní s najkratšími radami
			var listPokladni = ListReklamacia.Where(p => !p.Break)
				.GroupBy(c => c.Queue.Count)
				.OrderBy(g => g.Key)
				.FirstOrDefault();

			if (listPokladni is not null)
			{
				var list = listPokladni.ToList();
				var pokladna = list[((MySimulation)MySim).RndPickReklamacia.Next(list.Count)];
				pokladna.Queue.Enqueue(person);
				person.Zakaznik.StavZakaznika = Constants.StavZakaznika.PokladňaČakáVRade;
			}
		}

		//meta! sender="AgentPredajne", id="152", type="Notice"
		public void ProcessNoticeZaciatokReklamacie(MessageForm message)
		{
			var sprava = (MyMessage)message.CreateCopy();
			Constants.Log("ManagerReklamacia", MySim.CurrentTime, sprava.Zakaznik,"ProcessNoticeZaciatokReklamacie", Constants.LogType.ManagerLog);
			var reklamacia = GetVolnaReklamaciaPrazdnyRad();
			if (reklamacia is not null)
			{
				reklamacia.ObsadReklamaciu(sprava.Zakaznik);
				sprava.Addressee = MyAgent.FindAssistant(SimId.ProcessReklamacie);
				sprava.Reklamacia = reklamacia;
				StartContinualAssistant(sprava);
			}
			else
			{
				PriradZakaznikaDoRady(sprava);
			}
		}

		//meta! sender="ProcessReklamacie", id="160", type="Finish"
		public void ProcessFinish(MessageForm message)
		{
			var sprava = (MyMessage)message.CreateCopy();
			Constants.Log("ManagerReklamacia", MySim.CurrentTime, sprava.Zakaznik, "ProcessFinish", Constants.LogType.ManagerLog);
			sprava.Reklamacia.UvolniReklamaciu();
			if (sprava.Reklamacia.Queue.Count > 0)
			{
				var newSprava = sprava.Reklamacia.Queue.Dequeue();
				if (newSprava.Reklamacia is not null)
				{
					throw new InvalidOperationException("Zakaznik už bol boslúžený pri pokladni");
				}
				sprava.Reklamacia.ObsadReklamaciu(newSprava.Zakaznik);
				newSprava.Addressee = MyAgent.FindAssistant(SimId.ProcessReklamacie);
				newSprava.Reklamacia = sprava.Reklamacia;
				StartContinualAssistant(newSprava);
			}
			sprava.Code = Mc.NoticeKoniecPokladne;
			sprava.Addressee = MySim.FindAgent(SimId.AgentPredajne);
			sprava.TovarVydvihnty = true;
			Notice(sprava);
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
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
				ProcessFinish(message);
			break;

			case Mc.NoticeZaciatokReklamacie:
				ProcessNoticeZaciatokReklamacie(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new AgentReklamacia MyAgent
		{
			get
			{
				return (AgentReklamacia)base.MyAgent;
			}
		}
	}
}
