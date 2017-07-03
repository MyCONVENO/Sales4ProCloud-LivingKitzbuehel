using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using SALESCenterLivingKB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SALESCenterLivingKB.Admin
{
    public partial class EditUser : System.Web.UI.Page
    {
        ServerEntities entityModel = null;
        User existingUser = null;

        List<Agent> allAgents = null;
        List<Pricelist> allPricelists = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            entityModel = new ServerEntities();

            allAgents = entityModel.Agent.OrderBy(a => a.AgentName1).ToList();
            allPricelists = entityModel.Pricelist.OrderBy(p => p.PricelistName).ToList();

            string productAction = Request.QueryString["AgentAction"];

            if (productAction == "save")
            {
                LabelAddStatus.Text = "Änderungen gespeichert!";
            }

            if (productAction == "add")
            {
                LabelAddStatus.Text = "Gebiet hinzugefügt!";
            }

            if (productAction == "addPL")
            {
                LabelAddStatus.Text = "Preisliste hinzugefügt!";
            }

            if (productAction == "remove")
            {
                LabelAddStatus.Text = "Gebiet entfernt!";
            }

            if (productAction == "removePL")
            {
                LabelAddStatus.Text = "Preisliste entfernt!";
            }

            string UserID = Request.QueryString["UserID"];

            if (!string.IsNullOrEmpty(UserID))
            {
                existingUser = entityModel.User.FirstOrDefault(p => p.UserID == UserID);

                if (string.IsNullOrEmpty(tbName1.Text))
                {
                    labelUserName2.Text = existingUser.UserID;
                    tbCity.Text = existingUser.City;
                    tbFax.Text = existingUser.Fax;
                    tbName1.Text = existingUser.Name1;
                    tbName2.Text = existingUser.Name2;
                    tbPhone.Text = existingUser.Phone;
                    tbStreet.Text = existingUser.Street;
                    tbZIP.Text = existingUser.ZIP;
                    tbEMail.Text = existingUser.Email;
                    tbConfirm.Text = existingUser.ConfirmEmail;
                    tbMobil.Text = existingUser.Mobile;
                }
            }
        }


        public IQueryable<SALESCenterLivingKB.Models.UserAgents> GridView_Agents_GetData()
        {
            if (existingUser == null)
                return null;

            return existingUser.UserAgents.Where(ua => ua.IsDeleted == false).OrderBy(a => a.Summary).AsQueryable();
        }

        public IQueryable<SALESCenterLivingKB.Models.Agent> GetAgents()
        {
            if (existingUser == null)
                return null;


            foreach (var a in existingUser.UserAgents.Where(ua => ua.IsDeleted == false).Select(ua => ua.Agent))
            {
                allAgents.Remove(allAgents.FirstOrDefault(existingAgent => existingAgent.AgentID == a.AgentID));
            }

            return allAgents.AsQueryable();
        }

        public void GridView_Agents_DeleteItem(string UserAgentsID)
        {
            var ua = entityModel.UserAgents.FirstOrDefault(f => f.UserAgentsID == UserAgentsID);

            if (ua != null)
            {
                ua.IsDeleted = true;
                ua.SyncDateTime = DateTime.Now;
            }

            entityModel.SaveChanges();

            string pageUrl = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.Count() - Request.Url.Query.Count());
            Response.Redirect(pageUrl + "?AgentAction=remove&UserID=" + existingUser.UserID);
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            var agent = entityModel.Agent.FirstOrDefault(a => a.AgentID == DropDownListAgent.SelectedValue);

            UserAgents newUserAgent = existingUser.UserAgents.FirstOrDefault(f => f.AgentID == agent.AgentID);

            if (newUserAgent == null)
            {
                newUserAgent = new UserAgents();
                newUserAgent.UserID = existingUser.UserID;
                newUserAgent.UserAgentsID = Guid.NewGuid().ToString();
                newUserAgent.UserAgentClientID = agent.AgentClientID;
                newUserAgent.IsDeleted = false;
                newUserAgent.SyncDateTime = DateTime.Now;
                newUserAgent.AgentID = agent.AgentID;
                entityModel.UserAgents.Add(newUserAgent);
            }
            else
            {
                newUserAgent.IsDeleted = false;
                newUserAgent.SyncDateTime = DateTime.Now;
            }
            entityModel.SaveChanges();

            string pageUrl = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.Count() - Request.Url.Query.Count());
            Response.Redirect(pageUrl + "?AgentAction=add&UserID=" + existingUser.UserID);
        }

        protected void saveButton_Click(object sender, EventArgs e)
        {
            if (existingUser == null)
                return;

            existingUser.UserID = labelUserName2.Text;
            existingUser.City = tbCity.Text;
            existingUser.Fax = tbFax.Text;
            existingUser.Name1 = tbName1.Text;
            existingUser.Name2 = tbName2.Text;
            existingUser.Phone = tbPhone.Text;
            existingUser.Street = tbStreet.Text;
            existingUser.ZIP = tbZIP.Text;
            existingUser.ConfirmEmail = tbConfirm.Text;
            existingUser.Email = tbEMail.Text;
            existingUser.SyncDateTime = DateTime.Now;
            existingUser.Mobile = tbMobil.Text;

            entityModel.SaveChanges();

            string pageUrl = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.Count() - Request.Url.Query.Count());
            Response.Redirect(pageUrl + "?AgentAction=save&UserID=" + existingUser.UserID);
        }

        // The return type can be changed to IEnumerable, however to support
        // paging and sorting, the following parameters must be added:
        //     int maximumRows
        //     int startRowIndex
        //     out int totalRowCount
        //     string sortByExpression
        public IQueryable<SALESCenterLivingKB.Models.UserPriceList> GridView1_GetData()
        {
            if (existingUser == null)
                return null;

            return existingUser.UserPriceList.Where(a => a.IsDeleted == false).OrderBy(a => a.Pricelist.PricelistName).AsQueryable();
        }

        // The id parameter name should match the DataKeyNames value set on the control
        public void GridView1_DeleteItem(int id)
        {
            var upl = entityModel.UserPriceList.FirstOrDefault(f => f.ID == id);

            if (upl != null)
            {
                upl.IsDeleted = true;
                upl.SyncDateTime = DateTime.Now;
            }

            entityModel.SaveChanges();

            string pageUrl = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.Count() - Request.Url.Query.Count());
            Response.Redirect(pageUrl + "?AgentAction=removePL&UserID=" + existingUser.UserID);
        }

        protected void addPricelist_Click(object sender, EventArgs e)
        {
            var pricelist = entityModel.Pricelist.FirstOrDefault(a => a.PricelistID == DropDownListPricelist.SelectedValue);

            UserPriceList newUserPriceList = existingUser.UserPriceList.FirstOrDefault(f => f.PricelistID == pricelist.PricelistID);

            if (newUserPriceList == null)
            {
                newUserPriceList = new UserPriceList();
                newUserPriceList.UserID = existingUser.UserID;
                newUserPriceList.UserPricelistClientID = pricelist.PricelistClientID;
                newUserPriceList.PricelistID = pricelist.PricelistID;
                newUserPriceList.SyncDateTime = DateTime.Now;
                newUserPriceList.IsDeleted = false;
                entityModel.UserPriceList.Add(newUserPriceList);
            }
            else
            {
                newUserPriceList.SyncDateTime = DateTime.Now;
                newUserPriceList.IsDeleted = false;
            }
            entityModel.SaveChanges();

            string pageUrl = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.Count() - Request.Url.Query.Count());
            Response.Redirect(pageUrl + "?AgentAction=addPL&UserID=" + existingUser.UserID);
        }

        public IQueryable<SALESCenterLivingKB.Models.Pricelist> GetPricelists()
        {
            if (existingUser == null)
                return null;


            foreach (var a in existingUser.UserPriceList.Where(pl => pl.IsDeleted == false).Select(ua => ua.Pricelist))
            {
                allPricelists.Remove(allPricelists.FirstOrDefault(existingPl => existingPl.PricelistID == a.PricelistID));
            }

            return allPricelists.AsQueryable();
        }
    }
}