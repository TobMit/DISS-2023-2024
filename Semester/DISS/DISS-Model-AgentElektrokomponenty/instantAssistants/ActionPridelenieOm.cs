using OSPABA;
using simulation;
using agents;
using DISS_Model_AgentElektrokomponenty.Entity;
using managers;
using Action = OSPABA.Action;

namespace instantAssistants
{
	//meta! id="110"
	public class ActionPridelenieOm : Action
	{
		public ActionPridelenieOm(int id, Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
		}
		
		/// <summary>
		/// Vráti voľne obslužné miesto pre online zákazníkov
		/// </summary>
		/// <returns>Ak je volne vráti obslužné miesto inak null</returns>
		public ObsluzneMiesto? GetVolneOnline()
		{
			var tmp = ((ManagerObsluzneMiesto)MyAgent.MyManager).ListObsluhaOnline;
			return ((ManagerObsluzneMiesto)MyAgent.MyManager).ListObsluhaOnline.FirstOrDefault(miesto => !miesto.Obsadena);
		}

		/// <summary>
		/// Vráti voľne obslužné miesto pre ostatných zákazníkov
		/// </summary>
		/// <returns>Ak je volne vráti obslužné miesto inak null</returns>
		public ObsluzneMiesto? GetVolneOstatne()
		{
			return ((ManagerObsluzneMiesto)MyAgent.MyManager).ListObsluhaOstatne.FirstOrDefault(miesto => !miesto.Obsadena);
		}

		public override void Execute(MessageForm message)
		{
			var sprava = (MyMessage)message;
			Constants.Log("ActionPridelenieOm: Execute", Constants.LogType.InstantAssistantLog);
			ObsluzneMiesto? obsluzneMiesto;
			if (sprava.Zakaznik.TypZakaznika == Constants.TypZakaznika.Online)
			{
				obsluzneMiesto = GetVolneOnline();
			}
			else
			{
				obsluzneMiesto = GetVolneOstatne();
			}
			sprava.ObsluzneMiesto = obsluzneMiesto;
		}
		public new AgentObsluzneMiesto MyAgent
		{
			get
			{
				return (AgentObsluzneMiesto)base.MyAgent;
			}
		}
	}
}