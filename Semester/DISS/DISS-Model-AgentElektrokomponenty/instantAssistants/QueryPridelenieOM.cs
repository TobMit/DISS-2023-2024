using OSPABA;
using simulation;
using agents;
namespace instantAssistants
{
	//meta! id="71"
	public class QueryPridelenieOM : Query
	{
		public QueryPridelenieOM(int id, Simulation mySim, CommonAgent myAgent) :
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