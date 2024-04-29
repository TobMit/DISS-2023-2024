using OSPABA;
using simulation;
using managers;

namespace agents
{
    //meta! id="1"
    public class AgentModelu : Agent
    {
        public AgentModelu(int id, Simulation mySim, Agent parent) :
            base(id, mySim, parent)
        {
            Init();
        }

        public override void PrepareReplication()
        {
            base.PrepareReplication();
            // Setup component for the next replicatio
            Constants.Log("SchedulerZatvorenieAutomatu", MySim.CurrentTime, null,"AgentModelu: PrepareReplication", Constants.LogType.AgentLog);
            var message = new MyMessage(MySim)
            {
                Addressee = MySim.FindAgent(SimId.AgentOkolia),
                Code = Mc.Init
            };
            MyManager.Notice(new MyMessage(message));
            message.Addressee = MySim.FindAgent(SimId.AgentPredajne);
            MyManager.Notice(new MyMessage(message));
        }

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new ManagerModelu(SimId.ManagerModelu, MySim, this);
			AddOwnMessage(Mc.NoticePrichodZakaznika);
			AddOwnMessage(Mc.NoticeOdchodZakaznika);
		}
		//meta! tag="end"
    }
}