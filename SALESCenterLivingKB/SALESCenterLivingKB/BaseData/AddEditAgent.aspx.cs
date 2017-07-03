using SALESCenterLivingKB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SALESCenterLivingKB.BaseData
{
    public partial class AddEditAgent : System.Web.UI.Page
    {
        ServerEntities entityModel = null;
        Agent existingAgent = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            entityModel = new ServerEntities();

            string productAction = Request.QueryString["AgentAction"];

            if (productAction == "save")
            {
                LabelAddStatus.Text = "Änderungen gespeichert!";
            }           


            string AgentID = Request.QueryString["AgentID"];

            if (!string.IsNullOrEmpty(AgentID))
            {
                existingAgent = entityModel.Agent.FirstOrDefault(p => p.AgentID == AgentID);

                if (!this.IsPostBack)
                {
                    tbCity.Text = existingAgent.AgentCity;
                    tbFax.Text = existingAgent.AgentFax;
                    tbName1.Text = existingAgent.AgentName1;
                    tbName2.Text = existingAgent.AgentName2;
                    tbPhone.Text = existingAgent.AgentPhone;
                    tbStreet.Text = existingAgent.AgentStreet;
                    tbZIP.Text = existingAgent.AgentZIP;
                    tbEMail.Text = existingAgent.AgentEMail;
                    tbConfirm.Text = existingAgent.AgentConfirmEmail;
                }
            }
        }

        protected void saveButton_Click(object sender, EventArgs e)
        {
            if (existingAgent == null)
                return;

          
            existingAgent.AgentCity = tbCity.Text;
            existingAgent.AgentFax = tbFax.Text;
            existingAgent.AgentName1 = tbName1.Text;
            existingAgent.AgentName2 = tbName2.Text;
            existingAgent.AgentPhone = tbPhone.Text;
            existingAgent.AgentStreet = tbStreet.Text;
            existingAgent.AgentZIP = tbZIP.Text;
            existingAgent.AgentConfirmEmail = tbConfirm.Text;
            existingAgent.AgentEMail = tbEMail.Text;
            existingAgent.SyncDateTime = DateTime.Now;
            entityModel.SaveChanges();
           
            Response.Redirect("Agents.aspx");
        }

        protected void RemoveAgentButton_Click(object sender, EventArgs e)
        {
            var uas = entityModel.UserAgents.Where(usera => usera.AgentID == existingAgent.AgentID).ToList();

            foreach(var us in uas)
            {
                entityModel.UserAgents.Remove(us);
            }

            entityModel.Agent.Remove(existingAgent);
            entityModel.SaveChanges();
            Response.Redirect("Agents.aspx");
        }
    }
}