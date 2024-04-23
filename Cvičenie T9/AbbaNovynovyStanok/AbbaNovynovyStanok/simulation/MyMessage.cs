using OSPABA;

namespace AbbaNovynovyStanok.simulation
{
	public class MyMessage : MessageForm
	{
		public double ZaciatokCakania { get; set; }
		public MyMessage(Simulation sim) :
			base(sim)
		{
		}

		public MyMessage(MyMessage original) :
			base(original)
		{
			// copy() is called in superclass
			ZaciatokCakania = original.ZaciatokCakania;
		}

		public override MessageForm CreateCopy()
		{
			return new MyMessage(this);
		}

		protected override void Copy(MessageForm message)
		{
			base.Copy(message);
			MyMessage original = (MyMessage)message;
			// Copy attributes
		}
	}
}