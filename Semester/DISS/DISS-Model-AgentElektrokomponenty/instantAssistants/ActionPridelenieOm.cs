using OSPABA;
using simulation;
using agents;
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

		override public void Execute(MessageForm message)
		{
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