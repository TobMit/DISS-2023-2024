using OSPABA;
using simulation;
using agents;
namespace instantAssistants
{
	//meta! id="69"
	public class QueryPridelPokladnu : Query
	{
		public QueryPridelPokladnu(int id, Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
		}

		override public void Execute(MessageForm message)
		{
		}
		public new AgentPokladni MyAgent
		{
			get
			{
				return (AgentPokladni)base.MyAgent;
			}
		}
	}
}