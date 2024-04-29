using OSPABA;
using simulation;
using agents;
using continualAssistants;
using DISS_Model_AgentElektrokomponenty.Entity;
using instantAssistants;

namespace managers
{
    //meta! id="36"
    public class ManagerObsluzneMiesto : Manager
    {
        private RadaPredObsluznymMiestom _radaPredObsluznymMiestom;
        public List<ObsluzneMiesto> ListObsluhaOnline { get; private set; }
        public List<ObsluzneMiesto> ListObsluhaOstatne { get; private set; }

        public ManagerObsluzneMiesto(int id, Simulation mySim, Agent myAgent) :
            base(id, mySim, myAgent)
        {
            Init();
            ListObsluhaOnline = new();
            ListObsluhaOstatne = new();
            InitObsluzneMiesta();
            var core = (MySimulation)MySim;
            _radaPredObsluznymMiestom = new(core.StatPriemernaDlzkaRaduPredObsluhouBasic,
                core.StatPriemernaDlzkaRaduPredObsluhouZmluvny, core.StatPriemernaDlzkaRaduPredObsluhouOnline);
        }

        public void InitObsluzneMiesta()
        {
            var core = (MySimulation)MySim;
            for (int i = 0; i < ((MySimulation)MySim).PocetObsluhyOnline; i++)
            {
                ListObsluhaOnline.Add(new(null, i, core.ListStatVytazenieObsluhOnline[i], true));
            }

            for (int i = 0; i < ((MySimulation)MySim).PocetObsluhyOstatne; i++)
            {
                ListObsluhaOstatne.Add(new(null, i, core.ListStatVytazenieObsluhOstane[i]));
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

            ListObsluhaOnline.ForEach(miesto => miesto.Clear());
            ListObsluhaOstatne.ForEach(miesto => miesto.Clear());
        }
        
        /// <summary>
        /// Vráti voľne obslužné miesto pre online zákazníkov
        /// </summary>
        /// <returns>Ak je volne vráti obslužné miesto inak null</returns>
        public ObsluzneMiesto? GetVolneOnline()
        {
	        return ListObsluhaOnline.FirstOrDefault(miesto => !miesto.Obsadena);
        }

        /// <summary>
        /// Vráti voľne obslužné miesto pre ostatných zákazníkov
        /// </summary>
        /// <returns>Ak je volne vráti obslužné miesto inak null</returns>
        public ObsluzneMiesto? GetVolneOstatne()
        {
	        return ListObsluhaOstatne.FirstOrDefault(miesto => !miesto.Obsadena);
        }

        /// <summary>
        /// Informácie na obrazovku
        /// </summary>
        /// <returns>Vráti informácie na obrazovku</returns>
        public List<ObsluzneMiesto> GetInfoNaUI()
        {
            return ListObsluhaOnline.Concat(ListObsluhaOstatne).ToList();
        }

		//meta! sender="AgentPredajne", id="39", type="Notice"
		public void ProcessInit(MessageForm message)
        {
	        //todo ak sa bude tu štartovať pristávka tak sa musí sem pridať
        }
		

		//meta! sender="SchedulerPrestavkaOM", id="64", type="Finish"
		public void ProcessFinishSchedulerPrestavkaOM(MessageForm message)
        {
        }

		//meta! sender="ProcessOMDiktovanie", id="62", type="Finish"
		public void ProcessFinishProcessOMDiktovanie(MessageForm message)
        {
	        var sprava = (MyMessage)message.CreateCopy();
	        Constants.Log("ManagerObsluzneMiesto", MySim.CurrentTime, sprava.Zakaznik,"ProcessFinishProcessOMDiktovanie", Constants.LogType.ManagerLog);
	        sprava.Addressee = MyAgent.FindAssistant(SimId.ProcessOMPripravaTovaru);
	        StartContinualAssistant(sprava);
        }

		//meta! sender="AgentPredajne", id="49", type="Notice"
		public void ProcessNoticeUvolnenieOm(MessageForm message)
        {
	        var sprava = (MyMessage)message.CreateCopy();
	        Constants.Log("ManagerObsluzneMiesto", MySim.CurrentTime, sprava.Zakaznik,"ProcessNoticeUvolnenieOm", Constants.LogType.ManagerLog);
	        sprava.Addressee = MyAgent.FindAssistant(SimId.ProcessVyzdvihnutieTovaru);
	        StartContinualAssistant(sprava);
        }

		//meta! sender="AgentPredajne", id="65", type="Notice"
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

		//meta! sender="AgentPredajne", id="97", type="Request"
		public void ProcessPocetMiestVRade(MessageForm message)
        {
	        var sprava = (MyMessage)message;
	        Constants.Log("ManagerObsluzneMiesto", MySim.CurrentTime, sprava.Zakaznik,"ProcessPocetMiestVRade", Constants.LogType.ManagerLog);
            sprava.PocetLudiVOM = _radaPredObsluznymMiestom.Count;
            Response(sprava);
        }

		//meta! sender="AgentPredajne", id="103", type="Notice"
		public void ProcessNoticeZaciatokObsluhyOm(MessageForm message)
		{
			var sprava = (MyMessage)message.CreateCopy();
			Constants.Log("ManagerObsluzneMiesto", MySim.CurrentTime, sprava.Zakaznik,"ProcessNoticeZaciatokObsluhyOm", Constants.LogType.ManagerLog);
			if (_radaPredObsluznymMiestom.Count >= 1)
			{
				_radaPredObsluznymMiestom.Enqueue(sprava);
				return;
			}
			sprava.Addressee = MyAgent.FindAssistant(SimId.ActionPridelenieOm);
			Execute(sprava);
			if (sprava.ObsluzneMiesto is not null)
			{
				if (sprava.Zakaznik.TypZakaznika == Constants.TypZakaznika.Online)
				{
					sprava.Addressee = MyAgent.FindAssistant(SimId.ProcessOMOnlinePripravaTovaru);
				}
				else
				{
					sprava.Addressee = MyAgent.FindAssistant(SimId.ProcessOMDiktovanie);
				}
				sprava.ObsluzneMiesto.Obsluz(sprava.Zakaznik);
				StartContinualAssistant(sprava);
			}
			else
			{
				Constants.Log("ManagerObsluzneMiesto", MySim.CurrentTime, sprava.Zakaznik,"ProcessNoticeZaciatokObsluhyOm - Pridany do frontu", Constants.LogType.ManagerLog);
				_radaPredObsluznymMiestom.Enqueue(sprava);
			}
		}

		//meta! sender="ProcessOMOnlinePripravaTovaru", id="109", type="Finish"
		public void ProcessFinishProcessOMOnlinePripravaTovaru(MessageForm message)
		{
			var sprava = (MyMessage)message.CreateCopy();
			Constants.Log("ManagerObsluzneMiesto", MySim.CurrentTime, sprava.Zakaznik,"ProcessFinishProcessOMOnlinePripravaTovaru", Constants.LogType.ManagerLog);
			KoniecObsluhy(sprava);
		}

		//meta! sender="ProcessOMPripravaTovaru", id="107", type="Finish"
		public void ProcessFinishProcessOMPripravaTovaru(MessageForm message)
		{
			var sprava = (MyMessage)message.CreateCopy();
			Constants.Log("ManagerObsluzneMiesto", MySim.CurrentTime, sprava.Zakaznik,"ProcessFinishProcessOMPripravaTovaru", Constants.LogType.ManagerLog);
			KoniecObsluhy(sprava);
		}

		public void KoniecObsluhy(MyMessage sprava)
		{
			if (sprava.Zakaznik.TypVelkostiNakladu == Constants.TypVelkostiNakladu.Normálna)
			{
				sprava.ObsluzneMiesto.Uvolni();
				ObsluzneMiesto? tmpObsluzneMiesto = GetVolneOnline();
				while (_radaPredObsluznymMiestom.CountOnline >= 1 && tmpObsluzneMiesto is not null)
				{
					var spraveNew = (MyMessage)_radaPredObsluznymMiestom.Dequeue(true).CreateCopy();
					if (spraveNew.ObsluzneMiesto is not null)
					{
						throw new InvalidOperationException("Zákazník už bol obslúžený");
					}
					spraveNew.ObsluzneMiesto = tmpObsluzneMiesto;
					spraveNew.Addressee = MyAgent.FindAssistant(SimId.ProcessOMOnlinePripravaTovaru);
					tmpObsluzneMiesto.Obsluz(spraveNew.Zakaznik);
					StartContinualAssistant(spraveNew);
					tmpObsluzneMiesto = GetVolneOnline();
				}
				// to iste aj pre ostatné
				tmpObsluzneMiesto = GetVolneOstatne();
				while (_radaPredObsluznymMiestom.CountOstatne >= 1 && tmpObsluzneMiesto is not null)
				{
					var spraveNew = (MyMessage)_radaPredObsluznymMiestom.Dequeue().CreateCopy();
					if (spraveNew.ObsluzneMiesto is not null)
					{
						throw new InvalidOperationException("Zákazník už bol obslúžený");
					}
					spraveNew.ObsluzneMiesto = tmpObsluzneMiesto;
					spraveNew.Addressee = MyAgent.FindAssistant(SimId.ProcessOMDiktovanie);
					tmpObsluzneMiesto.Obsluz(spraveNew.Zakaznik);
					StartContinualAssistant(spraveNew);
					tmpObsluzneMiesto = GetVolneOstatne();
				}
			}
			else
			{
				sprava.ObsluzneMiesto.UvolniPredavaca();
				sprava.Zakaznik.ObsluzneMiesto = sprava.ObsluzneMiesto;
			}

			sprava.Addressee = MySim.FindAgent(SimId.AgentPredajne);
			sprava.Code = Mc.NoticeKoniecObsluhyOm;
			Notice(sprava);
		}

		//meta! sender="ProcessVyzdvihnutieTovaru", id="120", type="Finish"
		public void ProcessFinishProcessVyzdvihnutieTovaru(MessageForm message)
		{
			var sprava = (MyMessage)message.CreateCopy();
			Constants.Log("ManagerObsluzneMiesto", MySim.CurrentTime, sprava.Zakaznik,"ProcessFinishProcessVyzdvihnutieTovaru", Constants.LogType.ManagerLog);
			sprava.Addressee = MySim.FindAgent(SimId.AgentPredajne);
			sprava.Code = Mc.NoticeKoniecObsluhyOm;
			Notice(sprava);
			
			ObsluzneMiesto? tmpObsluzneMiesto = GetVolneOnline();
			while (_radaPredObsluznymMiestom.CountOnline >= 1 && tmpObsluzneMiesto is not null)
			{
				var spraveNew = (MyMessage)_radaPredObsluznymMiestom.Dequeue(true).CreateCopy();
				spraveNew.ObsluzneMiesto = tmpObsluzneMiesto;
				spraveNew.Addressee = MyAgent.FindAssistant(SimId.ProcessOMOnlinePripravaTovaru);
				tmpObsluzneMiesto.Obsluz(spraveNew.Zakaznik);
				StartContinualAssistant(spraveNew);
				tmpObsluzneMiesto = GetVolneOnline();
			}
			// to iste aj pre ostatné
			tmpObsluzneMiesto = GetVolneOstatne();
			while (_radaPredObsluznymMiestom.CountOstatne >= 1 && tmpObsluzneMiesto is not null)
			{
				var spraveNew = (MyMessage)_radaPredObsluznymMiestom.Dequeue().CreateCopy();
				spraveNew.ObsluzneMiesto = tmpObsluzneMiesto;
				spraveNew.Addressee = MyAgent.FindAssistant(SimId.ProcessOMDiktovanie);
				tmpObsluzneMiesto.Obsluz(spraveNew.Zakaznik);
				StartContinualAssistant(spraveNew);
				tmpObsluzneMiesto = GetVolneOstatne();
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
				case SimId.ProcessOMPripravaTovaru:
					ProcessFinishProcessOMPripravaTovaru(message);
				break;

				case SimId.ProcessOMOnlinePripravaTovaru:
					ProcessFinishProcessOMOnlinePripravaTovaru(message);
				break;

				case SimId.SchedulerPrestavkaOM:
					ProcessFinishSchedulerPrestavkaOM(message);
				break;

				case SimId.ProcessVyzdvihnutieTovaru:
					ProcessFinishProcessVyzdvihnutieTovaru(message);
				break;

				case SimId.ProcessOMDiktovanie:
					ProcessFinishProcessOMDiktovanie(message);
				break;
				}
			break;

			case Mc.PocetMiestVRade:
				ProcessPocetMiestVRade(message);
			break;

			case Mc.NoticeUvolnenieOm:
				ProcessNoticeUvolnenieOm(message);
			break;

			case Mc.Init:
				ProcessInit(message);
			break;

			case Mc.NoticeZaciatokObsluhyOm:
				ProcessNoticeZaciatokObsluhyOm(message);
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
		public new AgentObsluzneMiesto MyAgent
		{
			get
			{
				return (AgentObsluzneMiesto)base.MyAgent;
			}
		}
	}
}