using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SALESCenterLivingKB.Models
{
    public partial class Agent
    {
        public string Summary { get { return AgentName1 + "(" + AgentNumber + "), " + Client.ClientName; } }
        public string Mandant
        {
            get
            {
                //if (AgentClientID == "1")
                return "Living Kitzbühel";
                //if (AgentClientID == "2")
                //    return "SOLIDUS";

                return "";
            }
        }
    }
}