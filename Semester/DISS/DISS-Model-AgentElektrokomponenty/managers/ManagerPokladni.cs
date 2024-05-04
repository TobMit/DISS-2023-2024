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
		public bool Break { get; set; }
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
			// vytvorí list kde je rad 0 a pokladňa nie je obsadená a nie je na prestávke
			var listPokladni = ListPokladni
				.Where(p => !p.Obsadena && p.Queue.Count == 0 && !p.Break)
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
			var listPokladni = ListPokladni.Where(p => !p.Break) //todo vyriešiť toto nerobilo dobrotu
				.GroupBy(c => c.Queue.Count)
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
			var sprava = (MyMessage)message.CreateCopy();
			Constants.Log("ManagerPokladni", MySim.CurrentTime, null, "PokladneInit");
			sprava.Addressee = MyAgent.FindAssistant(SimId.SchedulerPrestavkaPokladne);
			StartContinualAssistant(sprava);
		}
		

		//meta! sender="ProcessObsluhyPokladni", id="53", type="Finish"
		public void ProcessFinishProcessObsluhyPokladni(MessageForm message)
		{
			var sprava = (MyMessage)message.CreateCopy();
			Constants.Log("ManagerPokladni", MySim.CurrentTime, sprava.Zakaznik,"ProcessFinishProcessObsluhyPokladni", Constants.LogType.ManagerLog);
			sprava.Pokladna.UvolniPokladnu();
			if (Break && sprava.Pokladna.ID != 0) // je prestávka a zároveň nie je to prvá pokadňa
			{
				Constants.Log("ManagerPokladni", MySim.CurrentTime, null,$"Pokladna {sprava.Pokladna.ID} ide na prestavku", Constants.LogType.ManagerLog);
				while (!sprava.Pokladna.Queue.IsEmpty())
				{
					ListPokladni[0].Queue.Enqueue(sprava.Pokladna.Queue.Dequeue());
				}
				sprava.Pokladna.Break = true;
				var newSprava = new MyMessage(MySim, null)
				{
					Addressee = MyAgent.FindAssistant(SimId.ProcessPrestavky)
				};
				newSprava.Pokladna = sprava.Pokladna;
				StartContinualAssistant(newSprava);
			}
			else if (sprava.Pokladna.Queue.Count > 0)
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
		public void ProcessNoticePrestavkaZaciatokAgentPredajne(MessageForm message)
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
			if (Break)
			{
				if (ListPokladni[0].Queue.IsEmpty())
				{
					sprava.Pokladna = ListPokladni[0];
					sprava.Pokladna.ObsadPokladnu(sprava.Zakaznik);
					sprava.Addressee = MyAgent.FindAssistant(SimId.ProcessObsluhyPokladni);
					StartContinualAssistant(sprava);
				}
				else
				{
					ListPokladni[0].Queue.Enqueue(sprava);
				}
				return;
			}
			var pokladna = GetVolnaPokladnaPrazdnyRad(); //todo ak je prestávka tak rovno ide k 1 pokladne
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
			var sprava = (MyMessage)message.CreateCopy();
			Constants.Log("ManagerPokladni", MySim.CurrentTime, null, $"Pokladna {sprava.Pokladna?.ID} ProcessFinishProcessPrestavky", Constants.LogType.ManagerLog);
			Break = false; // po 30 min môže byť false aj keď pre niektoré ešte nie je
			// ale obsluha trvá max 8 min tak taký prípad nenastane že by nestihol odísť na prestávku
			if (sprava.Pokladna is null)
			{
				throw new InvalidOperationException("[ManagerPokladni] - Pokladna je null po prestávke");
			}

			if (!sprava.Pokladna.Queue.IsEmpty())
			{
				throw new InvalidOperationException("[ManagerPokladni] - Pokladna nie je prázdna po prestávke");
			}
			sprava.Pokladna.Break = false;
			
		}

		//meta! sender="SchedulerPrestavkaPokladne", id="143", type="Notice"
		public void ProcessNoticePrestavkaZaciatokSchedulerPrestavkaPokladne(MessageForm message)
		{
			Constants.Log("ManagerPokladni", MySim.CurrentTime, null, "ProcessNoticePrestavkaZaciatokSchedulerPrestavkaPokladne", Constants.LogType.ManagerLog);
			var listPokladni = ListPokladni.Where(p => !p.Break && !p.Obsadena && p.ID != 0).ToList();
			foreach (var pokladna in listPokladni)
			{
				pokladna.Break = true;
				if (!pokladna.Queue.IsEmpty())
				{
					throw new InvalidOperationException("[ManagerPokladni]- prestavka init Pokladna nie je prázdna pri prestávke");
				}
				var newSprava = new MyMessage(MySim, null)
				{
					Addressee = MyAgent.FindAssistant(SimId.ProcessPrestavky)
				};
				newSprava.Pokladna = pokladna;
				Constants.Log("ManagerPokladni", MySim.CurrentTime, null, $"Pokladna {pokladna.ID} ide na prestavku", Constants.LogType.ManagerLog);
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

			case Mc.NoticePrestavkaZaciatok:
				switch (message.Sender.Id)
				{
				case SimId.SchedulerPrestavkaPokladne:
					ProcessNoticePrestavkaZaciatokSchedulerPrestavkaPokladne(message);
				break;

				case SimId.AgentPredajne:
					ProcessNoticePrestavkaZaciatokAgentPredajne(message);
				break;
				}
			break;

			case Mc.NoticeZaciatokPokladne:
				ProcessNoticeZaciatokPokladne(message);
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