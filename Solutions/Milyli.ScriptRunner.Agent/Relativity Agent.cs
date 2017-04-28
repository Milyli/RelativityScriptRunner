using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kCura.Agent;
using kCura.Relativity.Client;
using Relativity.API;

namespace Milyli.ScriptRunner.Agent
{
    [kCura.Agent.CustomAttributes.Name("Agent")]
    [System.Runtime.InteropServices.Guid("a3e50474-a27d-486b-b46b-c0fb6b40d3c5")]
    public class RelativityAgent : kCura.Agent.AgentBase
    {
        public override void Execute()
        {
            //Get the current Agent artifactID
            Int32 agentArtifactID = this.AgentID;
            //Get a dbContext for the EDDS database
            Relativity.API.IDBContext eddsDBContext = this.Helper.GetDBContext(-1);

            try
            {
                


            }
            catch (System.Exception ex)
            {
                //Your Agent caught an exception
                this.RaiseError(ex.Message, ex.Message);
            }
        }

        /**
		 * Returns the name of agent
		 */
        public override string Name
        {
            get
            {
                return "Agent";
            }
        }
    }
}
