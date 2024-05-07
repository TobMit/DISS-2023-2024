using System.Text;
using agents;
using DISS_Model_AgentElektrokomponenty.Entity;
using OSPABA;
using DISS.Random;
using DISS.Random.Continous;
using DISS.Random.Other;
using managers;
using OSPStat;


namespace simulation
{
    public class MySimulation : Simulation
    {
        public bool BehZavislosti { get; set; }
        public bool SlowDown { get; set; }
        public bool Break { get; set; }

        public bool ZvysenyTok { get; set; }

        private double Tok { get => ZvysenyTok ? 1.30 : 1.0; }

        public int PocetObsluznychMiest { private get; set; }

        public int PocetObsluhyOnline
        {
            get => PocetObsluznychMiest / 3;
        }

        public int PocetObsluhyOstatne
        {
            get => PocetObsluznychMiest - PocetObsluhyOnline;
        }

        public int PocetPokladni { get; set; }

        public List<Person> Persons;

        private DataStructure _eventData;

        // RNG
        public RNGPickPokladna RndPickPokladna;
        public RNGPickPokladna RndPickReklamacia;
        public Exponential RndPrichodZakaznikaBasic;
        public Exponential RndPrichodZakaznikaZmluvny;
        public Exponential RndPrichodZakaznikaOnline;
        public Exponential RndPrichodZakaznikaReklamacia;
        public Uniform RndTypNarocnostTovaru;
        public Uniform RndTypVelkostiNakladu;
        public Uniform RndTrvanieAutomatu;
        public Uniform RndTrvanieDiktovania;
        public Triangular RndTrvanieOnlinePripravaTovaru;
        public Empiric RndTrvaniePripravaSimple;
        public Uniform RndTrvaniePripravaNormal;
        public Empiric RndTrvaniePripravaHard;
        public DISS.Random.Discrete.Empiric RndTrvaniePladba;
        public Uniform RndTrvanieVyzdvyhnutieVelkehoTovaru;
        public Triangular RndTrvanieReklamacie;

        // štatistiky
        public int CelkovyPocetZakaznikov { get; set; }
        public Stat StatCasStravenyPredAutomatom;
        public Stat StatPriemernyCasVObchode;
        public WStat StatPriemernaDlzkaRaduPredAutomatom;
        public WStat StatVyuzitieAutomatu;
        public List<WStat> ListStatVytazenieObsluhOnline;
        public List<WStat> ListStatVytazenieObsluhOstane;
        public WStat StatPriemernaDlzkaRaduPredObsluhouBasic;
        public WStat StatPriemernaDlzkaRaduPredObsluhouZmluvny;
        public WStat StatPriemernaDlzkaRaduPredObsluhouOnline;
        public List<WStat> ListStatPriemerneDlzkyRadovPredPokladnami;
        public List<WStat> ListStatPriemerneVytazeniePokladni;
        public int PocetObsluzenychZakaznikov { get; set; }
        public int PocetVyhodenychZakaznikov { get; set; }

        // Globálne štatistiky
        private Stat _globPriemernyCasVObchode;
        private Stat _globCasStravenyPredAutomatom;
        private Stat _globPriemernaDlzkaRadu;
        private Stat _globPriemernyOdchodPoslednehoZakaznika;
        private Stat _globPriemernyPocetZakaznikov;
        private Stat _globPriemernyPocetObsluzenychZakaznikov;
        private Stat _globPriemerneVytazenieAutomatu;
        private Stat _globPriemernaDlzkaRaduPredObsluhouBasic;
        private Stat _globPriemernaDlzkaRaduPredObsluhouZmluvny;
        private Stat _globPriemernaDlzkaRaduPredObsluhouOnline;
        private List<Stat> _globPriemerneDlzkyRadovPredPokladnami;
        private List<Stat> _globPriemerneVytazeniePokladni;
        private List<Stat> _globPriemerneVytaznieObsluhyOnline;
        private List<Stat> _globPriemerneVytaznieObsluhyOstatne;

        public MySimulation(int pocetObsluznychMiest, int pocetPokladni)
        {
            PocetObsluznychMiest = pocetObsluznychMiest;
            PocetPokladni = pocetPokladni;
            ListStatVytazenieObsluhOnline = new();
            for (int i = 0; i < PocetObsluhyOnline; i++)
            {
                ListStatVytazenieObsluhOnline.Add(new(this));
            }

            ListStatVytazenieObsluhOstane = new();
            for (int i = 0; i < PocetObsluhyOstatne; i++)
            {
                ListStatVytazenieObsluhOstane.Add(new(this));
            }

            StatPriemernaDlzkaRaduPredObsluhouBasic = new(this);
            StatPriemernaDlzkaRaduPredObsluhouZmluvny = new(this);
            StatPriemernaDlzkaRaduPredObsluhouOnline = new(this);
            ListStatPriemerneDlzkyRadovPredPokladnami = new();
            ListStatPriemerneVytazeniePokladni = new();
            for (int i = 0; i < PocetPokladni; i++)
            {
                ListStatPriemerneDlzkyRadovPredPokladnami.Add(new(this));
                ListStatPriemerneVytazeniePokladni.Add(new(this));
            }

            Init();
        }

        protected override void PrepareSimulation()
        {
            base.PrepareSimulation();
            // Init Other
            Persons = new();

            RndPickPokladna = new(ExtendedRandom<double>.NextSeed(), PocetPokladni);
            RndPickReklamacia = new(ExtendedRandom<double>.NextSeed(), Constants.POCET_PRACOVNIKOV_REKLAMACIE);
            // (60*60) / 30 lebo je to 30 zákazníkov za hodinu ale systém beží v sekundách tak preto ten prepočet
            RndPrichodZakaznikaBasic = new(((60.0 * 60.0) / (15.0 * Tok)), ExtendedRandom<double>.NextSeed());
            RndPrichodZakaznikaZmluvny = new(((60.0 * 60.0) / (5.0 * Tok)), ExtendedRandom<double>.NextSeed());
            RndPrichodZakaznikaOnline = new(((60.0 * 60.0) / (10.0 * Tok)), ExtendedRandom<double>.NextSeed());
            RndPrichodZakaznikaReklamacia = new(((60.0 * 60.0) / (10.0 * Tok)), ExtendedRandom<double>.NextSeed());
            RndTypNarocnostTovaru = new(0, 1.0, ExtendedRandom<double>.NextSeed());
            RndTypVelkostiNakladu = new(0, 1.0, ExtendedRandom<double>.NextSeed());
            RndTrvanieAutomatu = new(30.0, 120.0, ExtendedRandom<double>.NextSeed());
            RndTrvanieDiktovania = new(60.0, 900.0, ExtendedRandom<double>.NextSeed());
            RndTrvanieOnlinePripravaTovaru = new(60.0, 120.0, 480.0, ExtendedRandom<double>.NextSeed());
            List<EmpiricBase<double>.EmpiricDataWithSeed<double>> listSimple = new();
            listSimple.Add(new(120.0, 300.0, 0.6, ExtendedRandom<double>.NextSeed()));
            listSimple.Add(new(300.0, 540.0, 0.4, ExtendedRandom<double>.NextSeed()));
            RndTrvaniePripravaSimple = new(listSimple, ExtendedRandom<double>.NextSeed());
            RndTrvaniePripravaNormal = new(540.0, 660.0, ExtendedRandom<double>.NextSeed());
            List<EmpiricBase<double>.EmpiricDataWithSeed<double>> listHard = new();
            listHard.Add(new(660.0, 720.0, 0.1, ExtendedRandom<double>.NextSeed()));
            listHard.Add(new(720.0, 1_200.0, 0.6, ExtendedRandom<double>.NextSeed()));
            listHard.Add(new(1_200.0, 1_500.0, 0.3, ExtendedRandom<double>.NextSeed()));
            RndTrvaniePripravaHard = new(listHard, ExtendedRandom<double>.NextSeed());
            List<EmpiricBase<int>.EmpiricDataWithSeed<int>> listPladba = new();
            listPladba.Add(new(180, 480, 0.4, ExtendedRandom<double>.NextSeed()));
            listPladba.Add(new(180, 360, 0.6, ExtendedRandom<double>.NextSeed()));
            RndTrvaniePladba = new(listPladba, ExtendedRandom<double>.NextSeed());
            RndTrvanieVyzdvyhnutieVelkehoTovaru = new(30.0, 70.0, ExtendedRandom<double>.NextSeed());
            RndTrvanieReklamacie = new(5, 8, 10);

            // štatistiky
            StatCasStravenyPredAutomatom = new();
            StatPriemernyCasVObchode = new();
            StatPriemernaDlzkaRaduPredAutomatom = new(this);
            StatVyuzitieAutomatu = new(this);

            // Create global statistcis
            _globPriemernyPocetZakaznikov = new();
            _globCasStravenyPredAutomatom = new();
            _globPriemernaDlzkaRadu = new();
            _globPriemerneVytazenieAutomatu = new();
            _globPriemerneVytaznieObsluhyOnline = new();
            for (int i = 0; i < PocetObsluhyOnline; i++)
            {
                _globPriemerneVytaznieObsluhyOnline.Add(new());
            }

            _globPriemerneVytaznieObsluhyOstatne = new();
            for (int i = 0; i < PocetObsluhyOstatne; i++)
            {
                _globPriemerneVytaznieObsluhyOstatne.Add(new());
            }

            _globPriemernaDlzkaRaduPredObsluhouBasic = new();
            _globPriemernaDlzkaRaduPredObsluhouZmluvny = new();
            _globPriemernaDlzkaRaduPredObsluhouOnline = new();
            _globPriemerneDlzkyRadovPredPokladnami = new();
            _globPriemerneVytazeniePokladni = new();
            for (int i = 0; i < PocetPokladni; i++)
            {
                _globPriemerneDlzkyRadovPredPokladnami.Add(new());
                _globPriemerneVytazeniePokladni.Add(new());
            }

            _globPriemernyPocetObsluzenychZakaznikov = new();
            _globPriemernyCasVObchode = new();
            _globPriemernyOdchodPoslednehoZakaznika = new();
        }

        protected override void PrepareReplication()
        {
            base.PrepareReplication();
            // Reset entities, queues, local statistics, etc...
            Persons.Clear();

            CelkovyPocetZakaznikov = 0;
            StatCasStravenyPredAutomatom.Clear();
            StatPriemernyCasVObchode.Clear();
            StatPriemernaDlzkaRaduPredAutomatom.Clear();
            StatVyuzitieAutomatu.Clear();
            ListStatVytazenieObsluhOnline.ForEach(stat => stat.Clear());
            ListStatVytazenieObsluhOstane.ForEach(stat => stat.Clear());
            StatPriemernaDlzkaRaduPredObsluhouBasic.Clear();
            StatPriemernaDlzkaRaduPredObsluhouZmluvny.Clear();
            StatPriemernaDlzkaRaduPredObsluhouOnline.Clear();
            ListStatPriemerneDlzkyRadovPredPokladnami.ForEach(stat => stat.Clear());
            ListStatPriemerneVytazeniePokladni.ForEach(stat => stat.Clear());
            PocetObsluzenychZakaznikov = 0;
            PocetVyhodenychZakaznikov = 0;
        }

        protected override void ReplicationFinished()
        {
            // Collect local statistics into global, update UI, etc...
            _globPriemernyPocetZakaznikov.AddSample(CelkovyPocetZakaznikov);
            _globCasStravenyPredAutomatom.AddSample(StatCasStravenyPredAutomatom.Mean());
            StatPriemernaDlzkaRaduPredAutomatom.updateAfterReplication();
            _globPriemernaDlzkaRadu.AddSample(StatPriemernaDlzkaRaduPredAutomatom.Mean());
            _globPriemerneVytazenieAutomatu.AddSample(StatVyuzitieAutomatu.Mean());
            for (int i = 0; i < PocetObsluhyOnline; i++)
            {
                _globPriemerneVytaznieObsluhyOnline[i].AddSample(ListStatVytazenieObsluhOnline[i].Mean());
            }

            for (int i = 0; i < PocetObsluhyOstatne; i++)
            {
                _globPriemerneVytaznieObsluhyOstatne[i].AddSample(ListStatVytazenieObsluhOstane[i].Mean());
            }

            StatPriemernaDlzkaRaduPredObsluhouBasic.updateAfterReplication();
            _globPriemernaDlzkaRaduPredObsluhouBasic.AddSample(StatPriemernaDlzkaRaduPredObsluhouBasic.Mean());
            StatPriemernaDlzkaRaduPredObsluhouZmluvny.updateAfterReplication();
            _globPriemernaDlzkaRaduPredObsluhouZmluvny.AddSample(StatPriemernaDlzkaRaduPredObsluhouZmluvny.Mean());
            StatPriemernaDlzkaRaduPredObsluhouOnline.updateAfterReplication();
            _globPriemernaDlzkaRaduPredObsluhouOnline.AddSample(StatPriemernaDlzkaRaduPredObsluhouOnline.Mean());
            for (int i = 0; i < PocetPokladni; i++)
            {
                ListStatPriemerneDlzkyRadovPredPokladnami[i].updateAfterReplication();
                _globPriemerneDlzkyRadovPredPokladnami[i]
                    .AddSample(ListStatPriemerneDlzkyRadovPredPokladnami[i].Mean());
                _globPriemerneVytazeniePokladni[i].AddSample(ListStatPriemerneVytazeniePokladni[i].Mean());
            }

            _globPriemernyPocetObsluzenychZakaznikov.AddSample(PocetObsluzenychZakaznikov);
            _globPriemernyCasVObchode.AddSample(StatPriemernyCasVObchode.Mean());
            _globPriemernyOdchodPoslednehoZakaznika.AddSample(CurrentTime);
            base.ReplicationFinished();
        }

        protected override void SimulationFinished()
        {
            // Display simulation results
            base.SimulationFinished();
            var confidencePriemernyPocetZakaznikov = _globPriemernyPocetZakaznikov.ConfidenceInterval95;
            Console.WriteLine($"Priemerný počet zákazníkov: {_globPriemernyPocetZakaznikov.Mean()} / [{confidencePriemernyPocetZakaznikov[0]:0.000} - {confidencePriemernyPocetZakaznikov[1]:0.000}]");
            var confidenceCasStravenyPredAutomatom = _globCasStravenyPredAutomatom.ConfidenceInterval95;
            Console.WriteLine(
                $"Čas strávený pred automatom: {Double.Round(_globCasStravenyPredAutomatom.Mean(), 4)}s / {TimeSpan.FromSeconds(_globCasStravenyPredAutomatom.Mean()).ToString(@"hh\:mm\:ss")} - [{TimeSpan.FromSeconds(confidenceCasStravenyPredAutomatom[0]).ToString(@"hh\:mm\:ss")} - {TimeSpan.FromSeconds(confidenceCasStravenyPredAutomatom[1]).ToString(@"hh\:mm\:ss")}]");
            var confidencePriemernaDlzkaRadu = _globPriemernaDlzkaRadu.ConfidenceInterval95;
            Console.WriteLine(
                $"Priemerná dĺžka radu pred automatom: {Double.Round(_globPriemernaDlzkaRadu.Mean(), 4)} - [{confidencePriemernaDlzkaRadu[0]:0.000} - {confidencePriemernaDlzkaRadu[1]:0.000}]");
            var confidencePriemerneVytazenieAutomatu = _globPriemerneVytazenieAutomatu.ConfidenceInterval95;
            Console.WriteLine(
                $"Priemerne vyťaženie automatu: {Double.Round(_globPriemerneVytazenieAutomatu.Mean(), 4) * 100:0.00}% - [{confidencePriemerneVytazenieAutomatu[0] * 100:0.00}% - {confidencePriemerneVytazenieAutomatu[1] * 100:0.00}%]");
            StringBuilder sbPriemerneVytazenieObsluhyOnline = new();
            foreach (var stat in _globPriemerneVytaznieObsluhyOnline)
            {
                var confidencePriemerneVytazenieObsluhyOnline = stat.ConfidenceInterval95;
                sbPriemerneVytazenieObsluhyOnline.Append($"([{Double.Round(stat.Mean(), 4) * 100:0.00}%] - [{confidencePriemerneVytazenieObsluhyOnline[0] * 100:0.00}% - {confidencePriemerneVytazenieObsluhyOnline[1] * 100:0.00}%]), ");
            }

            Console.WriteLine(
                $"Priemerne vyťaženie obsluhy online: {sbPriemerneVytazenieObsluhyOnline.Remove(sbPriemerneVytazenieObsluhyOnline.Length - 2, 2)}");
            StringBuilder sbPriemerneVytazenieObsluhyOstatne = new();
            foreach (var stat in _globPriemerneVytaznieObsluhyOstatne)
            {
                var confidencePriemerneVytazenieObsluhyOstatne = stat.ConfidenceInterval95;
                sbPriemerneVytazenieObsluhyOstatne.Append($"([{Double.Round(stat.Mean(), 4) * 100:0.00}%] - [{confidencePriemerneVytazenieObsluhyOstatne[0] * 100:0.00}% - {confidencePriemerneVytazenieObsluhyOstatne[1] * 100:0.00}%]), ");
            }

            Console.WriteLine(
                $"Priemerne vyťaženie obsluhy ostatne: {sbPriemerneVytazenieObsluhyOstatne.Remove(sbPriemerneVytazenieObsluhyOstatne.Length - 2, 2)}");
            
            var confidencePriemernaDlzkaRaduPredObsluhouBasic = _globPriemernaDlzkaRaduPredObsluhouBasic.ConfidenceInterval95;
            var confidencePriemernaDlzkaRaduPredObsluhouZmluvny = _globPriemernaDlzkaRaduPredObsluhouZmluvny.ConfidenceInterval95;
            var confidencePriemernaDlzkaRaduPredObsluhouOnline = _globPriemernaDlzkaRaduPredObsluhouOnline.ConfidenceInterval95;
            Console.WriteLine(
                $"Priemerná dĺžka radu pred obsluhou basic/zmluvný/online: ({Double.Round(_globPriemernaDlzkaRaduPredObsluhouBasic.Mean(), 4)} - [{confidencePriemernaDlzkaRaduPredObsluhouBasic[0]:0.000} - {confidencePriemernaDlzkaRaduPredObsluhouBasic[1]:0.000}]) / ({Double.Round(_globPriemernaDlzkaRaduPredObsluhouZmluvny.Mean(), 4)} - [{confidencePriemernaDlzkaRaduPredObsluhouZmluvny[0]:0.000} - {confidencePriemernaDlzkaRaduPredObsluhouZmluvny[1]:0.000}]) / ({Double.Round(_globPriemernaDlzkaRaduPredObsluhouOnline.Mean(), 4)} - [{confidencePriemernaDlzkaRaduPredObsluhouOnline[0]:0.000} - {confidencePriemernaDlzkaRaduPredObsluhouOnline[1]:0.000}])");
            StringBuilder sbPriemernaDlzkaRadu = new();
            StringBuilder sbPriemerneVytazeniePokladne = new();
            for (int i = 0; i < PocetPokladni; i++)
            {
                var confidencePriemernaDlzkaRaduPokladne = _globPriemerneDlzkyRadovPredPokladnami[i].ConfidenceInterval95;
                sbPriemernaDlzkaRadu.Append($"([{Double.Round(_globPriemerneDlzkyRadovPredPokladnami[i].Mean(), 4)}] - [{confidencePriemernaDlzkaRaduPokladne[0]:0.000} - {confidencePriemernaDlzkaRaduPokladne[1]:0.000}]), ");
                var confidencePriemerneVytazeniePokladne = _globPriemerneVytazeniePokladni[i].ConfidenceInterval95;
                sbPriemerneVytazeniePokladne.Append(
                    $"([{Double.Round(_globPriemerneVytazeniePokladni[i].Mean(), 4) * 100:0.00}%] - [{confidencePriemerneVytazeniePokladne[0] * 100:0.00}% - {confidencePriemerneVytazeniePokladne[1] * 100:0.00}%]), ");
            }

            Console.WriteLine(
                $"Priemerne dĺžky radov pred pokladňami: {sbPriemernaDlzkaRadu.Remove(sbPriemernaDlzkaRadu.Length - 2, 2)}");
            Console.WriteLine(
                $"Priemerne vyťaženie pokladni: {sbPriemerneVytazeniePokladne.Remove(sbPriemerneVytazeniePokladne.Length - 2, 2)}");
            var confidencePriemernyPocetObsluzenychZakaznikov = _globPriemernyPocetObsluzenychZakaznikov.ConfidenceInterval95;
            Console.WriteLine(
                $"Priemerný počet obslužných zákazníkov: {Double.Round(_globPriemernyPocetObsluzenychZakaznikov.Mean(), 4)} - [{confidencePriemernyPocetObsluzenychZakaznikov[0]:0.000} - {confidencePriemernyPocetObsluzenychZakaznikov[1]:0.000}]");
            var confidencePriemernyCasVObchode = _globPriemernyCasVObchode.ConfidenceInterval95;
            Console.WriteLine(
                $"Priemerný čas v obchode: {Double.Round(_globPriemernyCasVObchode.Mean(), 4)}s / {TimeSpan.FromSeconds(_globPriemernyCasVObchode.Mean()).ToString(@"hh\:mm\:ss")} - [{TimeSpan.FromSeconds(confidencePriemernyCasVObchode[0]).ToString(@"hh\:mm\:ss")} - {TimeSpan.FromSeconds(confidencePriemernyCasVObchode[1]).ToString(@"hh\:mm\:ss")}]");
            var confidencePriemernyOdchodPoslednehoZakaznika = _globPriemernyOdchodPoslednehoZakaznika.ConfidenceInterval95;
            Console.WriteLine(
                $"Priemerný odchod posledného zákazníka: {Double.Round(_globPriemernyOdchodPoslednehoZakaznika.Mean(), 4)} / {TimeSpan.FromSeconds(Constants.START_DAY + _globPriemernyOdchodPoslednehoZakaznika.Mean()).ToString(@"hh\:mm\:ss")} - [{TimeSpan.FromSeconds(Constants.START_DAY + confidencePriemernyOdchodPoslednehoZakaznika[0]).ToString(@"hh\:mm\:ss")} - {TimeSpan.FromSeconds(Constants.START_DAY + confidencePriemernyOdchodPoslednehoZakaznika[1]).ToString(@"hh\:mm\:ss")}]");
        }

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			AgentModelu = new AgentModelu(SimId.AgentModelu, this, null);
			AgentOkolia = new AgentOkolia(SimId.AgentOkolia, this, AgentModelu);
			AgentPredajne = new AgentPredajne(SimId.AgentPredajne, this, AgentModelu);
			AgentPokladni = new AgentPokladni(SimId.AgentPokladni, this, AgentPredajne);
			AgentReklamacia = new AgentReklamacia(SimId.AgentReklamacia, this, AgentPredajne);
			AgentAutomatu = new AgentAutomatu(SimId.AgentAutomatu, this, AgentPredajne);
			AgentObsluzneMiesto = new AgentObsluzneMiesto(SimId.AgentObsluzneMiesto, this, AgentPredajne);
		}
		public AgentModelu AgentModelu
		{ get; set; }
		public AgentOkolia AgentOkolia
		{ get; set; }
		public AgentPredajne AgentPredajne
		{ get; set; }
		public AgentPokladni AgentPokladni
		{ get; set; }
		public AgentReklamacia AgentReklamacia
		{ get; set; }
		public AgentAutomatu AgentAutomatu
		{ get; set; }
		public AgentObsluzneMiesto AgentObsluzneMiesto
		{ get; set; }
		//meta! tag="end"


        public DataStructure GetUIData(MySimulation sim)
        {
            if (BehZavislosti)
            {
                if (_eventData is null)
                {
                    _eventData = new();
                }

                _eventData.AktuaReplikacia = sim.CurrentReplication.ToString();
                if (_globPriemernaDlzkaRadu.SampleSize > 0)
                {
                    _eventData.BehZavislostiPriemernyPocetZakaznikovPredAutomatom =
                        Double.Round(_globPriemernaDlzkaRadu.Mean(), 3);
                }
                else
                {
                    _eventData.BehZavislostiPriemernyPocetZakaznikovPredAutomatom = 0;
                }

                return _eventData;
            }

            if (_eventData is null)
            {
                _eventData = new();
            }
            _eventData.NewData = true;

            // update sim času aj keď nie sú nové dáta
            if (_eventData.ShallowUpdate)
            {
                _eventData.SimulationTime =
                    $"{TimeSpan.FromSeconds(Constants.START_DAY + sim.CurrentTime).ToString(@"hh\:mm\:ss")} / {TimeSpan.FromSeconds(Constants.START_DAY + Constants.END_SIMULATION_TIME).ToString(@"hh\:mm\:ss")}";
            }

            if (_eventData.NewData)
            {
                _eventData.ShallowUpdate = SlowDown;
                if (_eventData.ShallowUpdate)
                {
                    _eventData.People = Persons.Where(o => o.StavZakaznika != Constants.StavZakaznika.OdišielZPredajne)
                        .Select(person => person.ToString())
                        .ToList();
                    _eventData.People.Insert(0,
                        "ID\t  | Typ Zákazníka \t | Typ naroč. tovar\t | Typ veľko. nákl. \t | Stav zákazníka");

                    _eventData.RadaPredAutomatom = $"{CelkovyPocetZakaznikov} / {PocetObsluzenychZakaznikov}";
                    _eventData.AutomatObsah = ((ManagerAutomatu)AgentAutomatu.MyManager).GuiToString();
                    _eventData.AutomatObsadeny = ((ManagerAutomatu)AgentAutomatu.MyManager).Obsluhuje;
                    var confidenceRadaPredObsluznymMiestomOnline = new Double[] {0, 0};
                    if (StatPriemernaDlzkaRaduPredObsluhouOnline.SampleSize > 1)
                    {
                        confidenceRadaPredObsluznymMiestomOnline =
                            StatPriemernaDlzkaRaduPredObsluhouOnline.ConfidenceInterval95;
                    }
                    _eventData.RadaPredObsluznimiMiestamiOnline =
                        $"{((ManagerObsluzneMiesto)AgentObsluzneMiesto.MyManager).RadaPredObsluznymMiestom.CountOnline}/{((ManagerObsluzneMiesto)AgentObsluzneMiesto.MyManager).RadaPredObsluznymMiestom.Count}/{Constants.RADA_PRED_OBSLUZNYM_MIESTOM + 1} - ({StatPriemernaDlzkaRaduPredObsluhouOnline.Mean():0.00} - [{confidenceRadaPredObsluznymMiestomOnline[0]:0.00} - {confidenceRadaPredObsluznymMiestomOnline[1]:0.00}])";
                    var confidenceRadaPredObsluznymMiestomBasic = new double[] { 0, 0 };
                    if (StatPriemernaDlzkaRaduPredObsluhouBasic.SampleSize > 1)
                    {
                        confidenceRadaPredObsluznymMiestomBasic =
                            StatPriemernaDlzkaRaduPredObsluhouBasic.ConfidenceInterval95;
                    }
                    _eventData.RadaPredObsluznimiMiestamiBasic =
                        $"{((ManagerObsluzneMiesto)AgentObsluzneMiesto.MyManager).RadaPredObsluznymMiestom.CountBasic}/{((ManagerObsluzneMiesto)AgentObsluzneMiesto.MyManager).RadaPredObsluznymMiestom.Count}/{Constants.RADA_PRED_OBSLUZNYM_MIESTOM + 1} - ({StatPriemernaDlzkaRaduPredObsluhouBasic.Mean():0.00} - [{confidenceRadaPredObsluznymMiestomBasic[0]:0.00} - {confidenceRadaPredObsluznymMiestomBasic[1]:0.00}])";
                    var confidenceRadaPredObsluznymMiestomZmluvny = new double[] { 0, 0 };
                    if (StatPriemernaDlzkaRaduPredObsluhouZmluvny.SampleSize > 1)
                    {
                        confidenceRadaPredObsluznymMiestomZmluvny =
                            StatPriemernaDlzkaRaduPredObsluhouZmluvny.ConfidenceInterval95;
                    }
                    _eventData.RadaPredObsluznimiMiestamiZmluvny =
                        $"{((ManagerObsluzneMiesto)AgentObsluzneMiesto.MyManager).RadaPredObsluznymMiestom.CountOstatne - ((ManagerObsluzneMiesto)AgentObsluzneMiesto.MyManager).RadaPredObsluznymMiestom.CountBasic}/{((ManagerObsluzneMiesto)AgentObsluzneMiesto.MyManager).RadaPredObsluznymMiestom.Count}/{Constants.RADA_PRED_OBSLUZNYM_MIESTOM + 1} - ({StatPriemernaDlzkaRaduPredObsluhouZmluvny.Mean():0.00} - [{confidenceRadaPredObsluznymMiestomZmluvny[0]:0.00} - {confidenceRadaPredObsluznymMiestomZmluvny[1]:0.00}])";
                    _eventData.ObsluzneMiestos = ((ManagerObsluzneMiesto)AgentObsluzneMiesto.MyManager).GetInfoNaUI();
                    _eventData.Pokladne = ((ManagerPokladni)AgentPokladni.MyManager).GetInfoNaUI();
                    _eventData.Reklamacia = ((ManagerReklamacia)AgentReklamacia.MyManager).ListReklamacia;
                }

                _eventData.AktuaReplikacia = sim.CurrentReplication.ToString();

                if (_globPriemernyCasVObchode.SampleSize > 1)
                {
                    var confidencePriemernyCasVObchode = _globPriemernyCasVObchode.ConfidenceInterval95;
                    _eventData.PriemernyCasVObhchode =
                        $"{Double.Round(_globPriemernyCasVObchode.Mean(), 3)}s / {TimeSpan.FromSeconds(_globPriemernyCasVObchode.Mean()).ToString(@"hh\:mm\:ss")} - [{TimeSpan.FromSeconds(confidencePriemernyCasVObchode[0]).ToString(@"hh\:mm\:ss")} - {TimeSpan.FromSeconds(confidencePriemernyCasVObchode[1]).ToString(@"hh\:mm\:ss")}]";
                }
                else
                {
                    _eventData.PriemernyCasVObhchode = "-/- / -:-:- - [-:-:- - -:-:-]";
                }

                if (_globCasStravenyPredAutomatom.SampleSize > 1)
                {
                    var confidenceCasStravenyPredAutomatom = _globCasStravenyPredAutomatom.ConfidenceInterval95;
                    _eventData.PriemernyCasPredAutomatom =
                        $"{Double.Round(_globCasStravenyPredAutomatom.Mean(), 3)}s / {TimeSpan.FromSeconds(_globCasStravenyPredAutomatom.Mean()).ToString(@"hh\:mm\:ss")} - [{TimeSpan.FromSeconds(confidenceCasStravenyPredAutomatom[0]).ToString(@"hh\:mm\:ss")} - {TimeSpan.FromSeconds(confidenceCasStravenyPredAutomatom[1]).ToString(@"hh\:mm\:ss")}]";
                }
                else
                {
                    _eventData.PriemernyCasPredAutomatom = "-/- / -:-:- - [-:-:- - -:-:-]";
                }

                if (_globPriemernaDlzkaRadu.SampleSize > 1)
                {
                    var confidencePriemernaDlzkaRadu = _globPriemernaDlzkaRadu.ConfidenceInterval95;
                    _eventData.PriemernaDlzkaraduPredAutomatom =
                        $"{Double.Round(_globPriemernaDlzkaRadu.Mean(), 3)} - [{confidencePriemernaDlzkaRadu[0]:0.000} - {confidencePriemernaDlzkaRadu[1]:0.000}]";
                }
                else
                {
                    _eventData.PriemernaDlzkaraduPredAutomatom = "-/- - [-:-:- - -:-:-]";
                }

                if (_globPriemernyOdchodPoslednehoZakaznika.SampleSize > 1)
                {
                    var confidencePriemernyOdchodPoslednehoZakaznika = _globPriemernyOdchodPoslednehoZakaznika.ConfidenceInterval95;
                    _eventData.PriemernyOdchodPoslednehoZakaznika =
                        $"{Double.Round(_globPriemernyOdchodPoslednehoZakaznika.Mean(), 3)} / {TimeSpan.FromSeconds(Constants.START_DAY + _globPriemernyOdchodPoslednehoZakaznika.Mean()).ToString(@"hh\:mm\:ss")} - [{TimeSpan.FromSeconds(Constants.START_DAY + confidencePriemernyOdchodPoslednehoZakaznika[0]).ToString(@"hh\:mm\:ss")} - {TimeSpan.FromSeconds(Constants.START_DAY + confidencePriemernyOdchodPoslednehoZakaznika[1]).ToString(@"hh\:mm\:ss")}]";
                }
                else
                {
                    _eventData.PriemernyOdchodPoslednehoZakaznika = "-/- / -:-:- - [-:-:- - -:-:-]";
                }

                if (_globPriemernyPocetZakaznikov.SampleSize > 1)
                {
                    var confidencePriemernyPocetZakaznikov = _globPriemernyPocetZakaznikov.ConfidenceInterval95;
                    _eventData.PriemernyPocetZakaznikov =
                        $"{Double.Round(_globPriemernyPocetZakaznikov.Mean(), 3)} - [{confidencePriemernyPocetZakaznikov[0]:0.000} - {confidencePriemernyPocetZakaznikov[1]:0.000}]";
                }
                else
                {
                    _eventData.PriemernyPocetZakaznikov = "-/- - [-:-:- - -:-:-]";
                }

                if (_globPriemernyPocetObsluzenychZakaznikov.SampleSize > 1)
                {
                    var confidencePriemernyPocetObsluzenychZakaznikov = _globPriemernyPocetObsluzenychZakaznikov.ConfidenceInterval95;
                    _eventData.PriemernyPocetObsluzenychZakaznikov =
                        $"{Double.Round(_globPriemernyPocetObsluzenychZakaznikov.Mean(), 3)} - [{confidencePriemernyPocetObsluzenychZakaznikov[0]:0.000} - {confidencePriemernyPocetObsluzenychZakaznikov[1]:0.000}]";
                }
                else
                {
                    _eventData.PriemernyPocetObsluzenychZakaznikov = "-/- - [-:-:- - -:-:-]";
                }

                if (_globPriemerneVytazenieAutomatu.SampleSize > 1)
                {
                    var confidencePriemerneVytazenieAutomatu = _globPriemerneVytazenieAutomatu.ConfidenceInterval95;
                    _eventData.PriemerneVytazenieAutomatu =
                        $"{Double.Round(_globPriemerneVytazenieAutomatu.Mean(), 4) * 100:0.00}% - [{confidencePriemerneVytazenieAutomatu[0] * 100:0.00}% - {confidencePriemerneVytazenieAutomatu[1] * 100:0.00}%]";
                }
                else
                {
                    _eventData.PriemerneVytazenieAutomatu = "-/- - [-:-:- - -:-:-]";
                }

                if (_globPriemernaDlzkaRaduPredObsluhouBasic.SampleSize > 1 &&
                    _globPriemernaDlzkaRaduPredObsluhouZmluvny.SampleSize > 1 &&
                    _globPriemernaDlzkaRaduPredObsluhouOnline.SampleSize > 1)
                {
                    var confidencePriemernaDlzkaRaduPredObsluhouBasic =
                        _globPriemernaDlzkaRaduPredObsluhouBasic.ConfidenceInterval95;
                    var confidencePriemernaDlzkaRaduPredObsluhouZmluvny =
                        _globPriemernaDlzkaRaduPredObsluhouZmluvny.ConfidenceInterval95;
                    var confidencePriemernaDlzkaRaduPredObsluhouOnline =
                        _globPriemernaDlzkaRaduPredObsluhouOnline.ConfidenceInterval95;
                    _eventData.PriemerneDlzkyRadovPredObsluhov =
                        $"({Double.Round(_globPriemernaDlzkaRaduPredObsluhouBasic.Mean(), 3)} - [{confidencePriemernaDlzkaRaduPredObsluhouBasic[0]:0.000} - {confidencePriemernaDlzkaRaduPredObsluhouBasic[1]:0.000}]), ({Double.Round(_globPriemernaDlzkaRaduPredObsluhouZmluvny.Mean(), 3)} - [{confidencePriemernaDlzkaRaduPredObsluhouZmluvny[0]:0.000} - {confidencePriemernaDlzkaRaduPredObsluhouZmluvny[1]:0.000}]), ({Double.Round(_globPriemernaDlzkaRaduPredObsluhouOnline.Mean(), 3)} - [{confidencePriemernaDlzkaRaduPredObsluhouOnline[0]:0.000} - {confidencePriemernaDlzkaRaduPredObsluhouOnline[1]:0.000}])";
                }
                else
                {
                    _eventData.PriemerneDlzkyRadovPredObsluhov = "[-/-], [-/-], [-/-]";
                }

                StringBuilder sbPriemernaDlkaRadu = new();
                StringBuilder sbPriemerneVytazeniePokladni = new();
                for (int i = 0; i < PocetPokladni; i++)
                {
                    if (_globPriemerneDlzkyRadovPredPokladnami[i].SampleSize <= 1)
                    {
                        sbPriemernaDlkaRadu.Append("[-/-], ");
                        sbPriemerneVytazeniePokladni.Append("[-/-], ");
                    }
                    else
                    {
                        var confidencePriemernaDlzkaRaduPokladne = _globPriemerneDlzkyRadovPredPokladnami[i].ConfidenceInterval95;
                        sbPriemernaDlkaRadu.Append(
                            $"({Double.Round(_globPriemerneDlzkyRadovPredPokladnami[i].Mean(), 3):0.000} - [{confidencePriemernaDlzkaRaduPokladne[0]:0.000} - {confidencePriemernaDlzkaRaduPokladne[1]:0.000}]), ");
                        var confidencePriemerneVytazeniePokladne = _globPriemerneVytazeniePokladni[i].ConfidenceInterval95;
                        sbPriemerneVytazeniePokladni.Append(
                            $"({Double.Round(_globPriemerneVytazeniePokladni[i].Mean(), 4) * 100:0.00}% - [{confidencePriemerneVytazeniePokladne[0] * 100:0.00}% - {confidencePriemerneVytazeniePokladne[1] * 100:0.000}%]), ");
                    }
                }

                _eventData.PriemerneDlzkyRadovPredPokladnami =
                    sbPriemernaDlkaRadu.Remove(sbPriemernaDlkaRadu.Length - 2, 2).ToString();
                _eventData.PriemerneVytazeniePokladni = sbPriemerneVytazeniePokladni
                    .Remove(sbPriemerneVytazeniePokladni.Length - 2, 2).ToString();
                StringBuilder sbPriemerneVytazenieObsluhyOnline = new();
                for (int i = 0; i < ((ManagerObsluzneMiesto)AgentObsluzneMiesto.MyManager).ListObsluhaOnline.Count; i++)
                {
                    if (_globPriemerneVytaznieObsluhyOnline[i].SampleSize <= 1)
                    {
                        sbPriemerneVytazenieObsluhyOnline.Append("[-/-], ");
                    }
                    else
                    {
                        var confidencePriemerneObsluhyOnline = _globPriemerneVytaznieObsluhyOnline[i].ConfidenceInterval95;
                        sbPriemerneVytazenieObsluhyOnline.Append(
                            $"({Double.Round(_globPriemerneVytaznieObsluhyOnline[i].Mean(), 4) * 100:0.00}% - [{confidencePriemerneObsluhyOnline[0] * 100:0.00}% - {confidencePriemerneObsluhyOnline[1] * 100:0.000}%]), ");
                    }
                }

                _eventData.PriemerneVytazenieObsluhyOnline = sbPriemerneVytazenieObsluhyOnline
                    .Remove(sbPriemerneVytazenieObsluhyOnline.Length - 2, 2).ToString();
                StringBuilder sbPriemerneVytazenieObsluhyOstatne = new();
                for (int i = 0; i < ((ManagerObsluzneMiesto)AgentObsluzneMiesto.MyManager).ListObsluhaOstatne.Count; i++)
                {
                    if (_globPriemerneVytaznieObsluhyOstatne[i].SampleSize <= 1)
                    {
                        sbPriemerneVytazenieObsluhyOstatne.Append("[-/-], ");
                    }
                    else
                    {
                        var confidencePriemerneObsluhyOstatne = _globPriemerneVytaznieObsluhyOstatne[i].ConfidenceInterval95;
                        sbPriemerneVytazenieObsluhyOstatne.Append(
                            $"[" +
                            $"({Double.Round(_globPriemerneVytaznieObsluhyOstatne[i].Mean(), 4) * 100:0.00}% - [{confidencePriemerneObsluhyOstatne[0] * 100:0.00}% - {confidencePriemerneObsluhyOstatne[1] * 100:0.000}%]), ");
                    }
                }

                _eventData.PriemerneVytazenieObsluhyOstatne = sbPriemerneVytazenieObsluhyOstatne
                    .Remove(sbPriemerneVytazenieObsluhyOstatne.Length - 2, 2).ToString();
            }

            return _eventData;
        }
    }
}