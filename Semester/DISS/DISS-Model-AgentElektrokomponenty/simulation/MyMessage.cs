using DISS_Model_AgentElektrokomponenty.Entity;
using Microsoft.VisualBasic;
using OSPABA;

namespace simulation
{
    public class MyMessage : MessageForm
    {
        public Person Zakaznik { get; set; }

        public int PocetLudiVOM { get; set; }
        /// <summary>
        /// Jednoduchá správa
        /// </summary>
        /// <param name="sim">SimCore</param>
        public MyMessage(Simulation sim, Person? pZakaznik = null) :
            base(sim)
        {
            if (pZakaznik is null)
            {
                Zakaznik = new();
            }
            else
            {
                Zakaznik = pZakaznik;
            }

            PocetLudiVOM = 0;
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
            // Copy attributes
            Zakaznik = original.Zakaznik;
            PocetLudiVOM = original.PocetLudiVOM;   
        }
    }
}