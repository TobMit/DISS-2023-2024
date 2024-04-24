using Microsoft.VisualBasic;
using OSPABA;

namespace simulation
{
    public class MyMessage : MessageForm
    {
        public int ID { get; set; }
        public double VstupDoPredajne { get; set; }
        public double VstupDoRadyPredAutomatom { get; set; }
        public double VstupDoRadyPredObsluhov { get; set; }
        public double VstupDoRadyPredPokladnov { get; set; }

        public double TimeOfArrival { get; private set; }

        public Constants.TypZakaznika TypZakaznika { get; set; }
        public Constants.TypNarocnostiTovaru TypNarocnostiTovaru { get; private set; }
        public Constants.TypVelkostiNakladu TypVelkostiNakladu { get; private set; }
        public Constants.StavZakaznika StavZakaznika { get; set; }

        /// <summary>
        /// Jednoduchá správa
        /// </summary>
        /// <param name="sim">SimCore</param>
        public MyMessage(Simulation sim) :
            base(sim)
        {
            TimeOfArrival = 0;
            TypZakaznika = 0;
            SetTypNarocnostiTovaru(0);
            SetTypVelkostiNakladu(0);
            StavZakaznika = Constants.StavZakaznika.RadPredAutomatom;
            ID = 0;
        }

        /// <summary>
        /// Vytvorenie úplnej správy
        /// </summary>
        /// <param name="sim">SimCore</param>
        /// <param name="pTimeOfArrival">Čas príchodu</param>
        /// <param name="prTypZakaznika">Typ zákazníka</param>
        /// <param name="prTypNarocnostTovaru">Typ tovaru</param>
        /// <param name="prTypVelkostiNakladu">Typ velkosti nákladu</param>
        /// <param name="pId">ID zákazníka</param>
        public MyMessage(Simulation sim,
            double pTimeOfArrival,
            Constants.TypZakaznika pTypZakaznika,
            double prTypNarocnostTovaru,
            double prTypVelkostiNakladu,
            int pId) :
            base(sim)
        {
            TimeOfArrival = pTimeOfArrival;
            TypZakaznika = pTypZakaznika;
            SetTypNarocnostiTovaru(prTypNarocnostTovaru);
            SetTypVelkostiNakladu(prTypVelkostiNakladu);
            StavZakaznika = Constants.StavZakaznika.RadPredAutomatom;
            ID = pId;
        }

        public MyMessage(MyMessage original) :
            base(original)
        {
            // copy() is called in superclass
        }

        override public MessageForm CreateCopy()
        {
            return new MyMessage(this);
        }

        override protected void Copy(MessageForm message)
        {
            base.Copy(message);
            MyMessage original = (MyMessage)message;
            ID = original.ID;
            VstupDoPredajne = original.VstupDoPredajne;
            VstupDoRadyPredAutomatom = original.VstupDoRadyPredAutomatom;
            VstupDoRadyPredObsluhov = original.VstupDoRadyPredObsluhov;
            VstupDoRadyPredPokladnov = original.VstupDoRadyPredPokladnov;
            TimeOfArrival = original.TimeOfArrival;

            TypZakaznika = original.TypZakaznika;
            TypNarocnostiTovaru = original.TypNarocnostiTovaru;
            TypVelkostiNakladu = original.TypVelkostiNakladu;
            StavZakaznika = original.StavZakaznika;
            // Copy attributes
        }
        

        private void SetTypNarocnostiTovaru(double prTypNarocnostTovaru)
        {
            if (prTypNarocnostTovaru < 0.3)
            {
                TypNarocnostiTovaru = Constants.TypNarocnostiTovaru.Simple;
            }
            else if (prTypNarocnostTovaru < 0.7)
            {
                TypNarocnostiTovaru = Constants.TypNarocnostiTovaru.Normal;
            }
            else
            {
                TypNarocnostiTovaru = Constants.TypNarocnostiTovaru.Hard;
            }
        }

        private void SetTypVelkostiNakladu(double prTypVelkostiNakladu)
        {
            if (prTypVelkostiNakladu < 0.6)
            {
                TypVelkostiNakladu = Constants.TypVelkostiNakladu.Veľká;
            }
            else
            {
                TypVelkostiNakladu = Constants.TypVelkostiNakladu.Normálna;
            }
        }
    }
}