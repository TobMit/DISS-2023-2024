using OSPABA;
using simulation;
using agents;
using continualAssistants;
using instantAssistants;
namespace managers
{
	//meta! id="44"
	public class ManagerPokladni : Manager
	{
		public List<Pokladna> ListPokladni { get; private set; }
		public ManagerPokladni(int id, Simulation mySim, Agent myAgent) :
			base(id, mySim, myAgent)
		{
			Init();
		}
		
		public void InitPokladne()
		{
			var core = (MySimulation)MySim;
			for (int i = 0; i < core.PocetPokladni; i++)
			{
				ListPokladni.Add(new(i, core.ListStatPriemerneDlzkyRadovPredPokladnami[i], 
					core.ListStatPriemerneVytazeniePokladni[i]));
			}
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication

			if (PetriNet != null)
			{
				PetriNet.Clear();
			}
			ListPokladni = new();
			InitPokladne();
		}

		/// <summary>
		/// Vráti voľnú pokladňu ak nie nikto obsluhovaný a je voľný rad ak je voľných viac vyberie náhodne
		/// </summary>
		/// <returns>Pokladňu ak splna požiadavky inak null</returns>
		public Pokladna? GetVolnaPokladnaPrazdnyRad()
		{
			// vytvorí list kde je rad 0 a pokladňa nie je obsadená
			var listPokladni = ListPokladni
				.Where(p => !p.Obsadena && p.Queue.Count == 0)
				.OrderBy(g => g.ID)
				.ToList();

			if (listPokladni.Count > 0)
			{
				return listPokladni[((MySimulation)MySim).RndPickPokladna.Next(listPokladni.Count)];
			}

			return null;
		}

		/// <summary>
		/// Priradí zákazníka do najkratšej rady, a keď majú viaceré rovnaké dĺžky tak náhodne vyberie jednu
		/// </summary>
		/// <param name="person">Človek ktorý sa pridáva do rady</param>
		/// <param name="core">Jadro simulácie</param>
		public void PriradZakaznikaDoRady(MyMessage person)
		{
			// spraví list pokladní s najkratšími radami
			var listPokladni = ListPokladni.GroupBy(c => c.Queue.Count)
				.OrderBy(g => g.Key)
				.FirstOrDefault();

			if (listPokladni is not null)
			{
				var list = listPokladni.ToList();
				var pokladna = list[((MySimulation)MySim).RndPickPokladna.Next(list.Count)];
				pokladna.Queue.Enqueue(person);
				person.Zakaznik.StavZakaznika = Constants.StavZakaznika.PokladňaČakáVRade;
			}
		}
		
		/// <summary>
		/// Informácie na UI
		/// </summary>
		/// <returns>Informácie na UI</returns>
		public List<Pokladna> GetInfoNaUI()
		{
			return ListPokladni;
		}
		
		//meta! sender="AgentPredajne", id="46", type="Notice"
		public void ProcessInit(MessageForm message)
		{
		}
		

		//meta! sender="ProcessObsluhyPokladni", id="53", type="Finish"
		public void ProcessFinishProcessObsluhyPokladni(MessageForm message)
		{
			var sprava = (MyMessage)message.CreateCopy();
			Constants.Log("ManagerPokladni", MySim.CurrentTime, sprava.Zakaznik,"ProcessFinishProcessObsluhyPokladni", Constants.LogType.ManagerLog);
			sprava.Pokladna.UvolniPokladnu();
			if (sprava.Pokladna.Queue.Count > 0)
			{
				var newSprava = sprava.Pokladna.Queue.Dequeue();
				if (newSprava.Pokladna is not null)
				{
					throw new InvalidOperationException("Zakaznik už bol boslúžený pri pokladni");
				}
				sprava.Pokladna.ObsadPokladnu(newSprava.Zakaznik);
				newSprava.Addressee = MyAgent.FindAssistant(SimId.ProcessObsluhyPokladni);
				newSprava.Pokladna = sprava.Pokladna;
				StartContinualAssistant(newSprava);
			}
			sprava.Code = Mc.NoticeKoniecPokladne;
			sprava.Addressee = MySim.FindAgent(SimId.AgentPredajne);
			Notice(sprava);
		}

		//meta! sender="SchedulerPrestavkaPokladne", id="57", type="Finish"
		public void ProcessFinishSchedulerPrestavkaPokladne(MessageForm message)
		{
		}

		//meta! sender="AgentPredajne", id="58", type="Notice"
		public void ProcessNoticePrestavkaZaciatok(MessageForm message)
		{
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
			}
		}

		//meta! sender="AgentPredajne", id="114", type="Notice"
		public void ProcessNoticeZaciatokPokladne(MessageForm message)
		{
			var sprava = (MyMessage)message.CreateCopy();
			Constants.Log("ManagerPokladni", MySim.CurrentTime, sprava.Zakaznik,"ProcessNoticeZaciatokPokladne", Constants.LogType.ManagerLog);
			var pokladna = GetVolnaPokladnaPrazdnyRad();
			if (pokladna is not null)
			{
				pokladna.ObsadPokladnu(sprava.Zakaznik);
				sprava.Addressee = MyAgent.FindAssistant(SimId.ProcessObsluhyPokladni);
				sprava.Pokladna = pokladna;
				StartContinualAssistant(sprava);
			}
			else
			{
				PriradZakaznikaDoRady(sprava);
			}
		}

		//meta! sender="ProcessPrestavky", id="139", type="Finish"
		public void ProcessFinishProcessPrestavky(MessageForm message)
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
				case SimId.SchedulerPrestavkaPokladne:
					ProcessFinishSchedulerPrestavkaPokladne(message);
				break;

				case SimId.ProcessObsluhyPokladni:
					ProcessFinishProcessObsluhyPokladni(message);
				break;

				case SimId.ProcessPrestavky:
					ProcessFinishProcessPrestavky(message);
				break;
				}
			break;

			case Mc.NoticeZaciatokPokladne:
				ProcessNoticeZaciatokPokladne(message);
			break;

			case Mc.NoticePrestavkaZaciatok:
				ProcessNoticePrestavkaZaciatok(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new AgentPokladni MyAgent
		{
			get
			{
				return (AgentPokladni)base.MyAgent;
			}
		}
	}
}