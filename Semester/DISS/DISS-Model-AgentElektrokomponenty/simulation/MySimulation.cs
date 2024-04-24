using System.Security.AccessControl;
using OSPABA;
using agents;
using DISS_HelperClasses.Statistic;
using DISS_Model_AgentElektrokomponenty.Entity;
using DISS.Random;
using DISS.Random.Continous;
using DISS.Random.Other;
using OSPStat;


namespace simulation
{
    public class MySimulation : Simulation
    {
        public bool BehZavislosti { get; set; }

        private int _pocetObsluznychMiest;
        private int _pocetPokladni;

        public List<Person> Persons;

        // RNG
        //public RNGPickPokladna RndPickPokladna; todo reslove
        public Exponential RndPrichodZakaznikaBasic;
        public Exponential RndPrichodZakaznikaZmluvny;
        public Exponential RndPrichodZakaznikaOnline;
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

        public int CelkovyPocetZakaznikov { get; set; }
        
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

        public MySimulation()
        {
            Init();
        }

        protected override void PrepareSimulation()
        {
            base.PrepareSimulation();
            // Init Other
            Persons = new();
            
            //RndPickPokladna = new(ExtendedRandom<double>.NextSeed(), _pocetPokladni);
            // (60*60) / 30 lebo je to 30 zákazníkov za hodinu ale systém beží v sekundách tak preto ten prepočet
            RndPrichodZakaznikaBasic = new(((60.0 * 60.0) / 15.0), ExtendedRandom<double>.NextSeed());
            RndPrichodZakaznikaZmluvny = new(((60.0 * 60.0) / 5.0), ExtendedRandom<double>.NextSeed());
            RndPrichodZakaznikaOnline = new(((60.0 * 60.0) / 10.0), ExtendedRandom<double>.NextSeed());
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
            
            // Create global statistcis
            _globPriemernyPocetZakaznikov = new();
        }

        protected override void PrepareReplication()
        {
            base.PrepareReplication();
            // Reset entities, queues, local statistics, etc...
            Persons.Clear();
            
            CelkovyPocetZakaznikov = 0;
            
        }

        protected override void ReplicationFinished()
        {
            // Collect local statistics into global, update UI, etc...
            _globPriemernyPocetZakaznikov.AddSample(CelkovyPocetZakaznikov);
            base.ReplicationFinished();
        }

        protected override void SimulationFinished()
        {
            // Display simulation results
            base.SimulationFinished();
            Console.WriteLine($"Priemerný počet zákazníkov: {_globPriemernyPocetZakaznikov.Mean()}");
        }

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			AgentModelu = new AgentModelu(SimId.AgentModelu, this, null);
			AgentOkolia = new AgentOkolia(SimId.AgentOkolia, this, AgentModelu);
			AgentPredajne = new AgentPredajne(SimId.AgentPredajne, this, AgentModelu);
			AgentPokladni = new AgentPokladni(SimId.AgentPokladni, this, AgentPredajne);
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
		public AgentAutomatu AgentAutomatu
		{ get; set; }
		public AgentObsluzneMiesto AgentObsluzneMiesto
		{ get; set; }
		//meta! tag="end"
    }
}