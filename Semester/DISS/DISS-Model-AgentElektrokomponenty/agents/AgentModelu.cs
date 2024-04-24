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
            // Setup component for the next replication
            var message = new MyMessage(MySim)
            {
                Addressee = this,
                Code = Mc.Init
            };
            MyManager.Notice(message);
        }

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new ManagerModelu(SimId.ManagerModelu, MySim, this);
			AddOwnMessage(Mc.VstupDoPredajne);
			AddOwnMessage(Mc.NoticePrichodZakaznika);
		}
		//meta! tag="end"
    }
}