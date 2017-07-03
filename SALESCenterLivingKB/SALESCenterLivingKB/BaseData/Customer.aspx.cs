using SALESCenterLivingKB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SALESCenterLivingKB.BaseData
{
    public partial class Customer : System.Web.UI.Page
    {
        ServerEntities entityModel = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            entityModel = new ServerEntities();
            entityModel.Configuration.LazyLoadingEnabled = false;
        }

        // The return type can be changed to IEnumerable, however to support
        // paging and sorting, the following parameters must be added:
        //     int maximumRows
        //     int startRowIndex
        //     out int totalRowCount
        //     string sortByExpression
        public IQueryable<SALESCenterLivingKB.Models.Client> GridViewClient_GetData()
        {
            return entityModel.Client.Where(c => c.IsDeleted == false).OrderBy(c => c.ClientID);
        }

        protected void GridViewClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillCustomer(0);
        }


        void FillCustomer(int pageIndex)
        {

            string selectedClientID = GridViewClient.SelectedValue.ToString();
            string Filter = tbFilter.Text;

            var dbcustomers = entityModel.Customer.Where(a => a.IsDeleted == false && a.CustomerClientID == selectedClientID);

            if (!string.IsNullOrEmpty(Filter))
            {
                dbcustomers = dbcustomers.Where(c => c.CustomerName1.Contains(Filter) || c.CustomerName2.Contains(Filter) || c.CustomerName3.Contains(Filter) || c.CustomerStreet.Contains(Filter) || c.CustomerCity.Contains(Filter) || c.CustomerNumber.StartsWith(Filter));
            }

            var customers = dbcustomers.OrderBy(a => a.CustomerNumber).ToList();
            GridViewCustomer.DataSource = customers;
            GridViewCustomer.PageIndex = pageIndex;
            GridViewCustomer.DataBind();
            GridViewCustomer.SelectedIndex = 0;

            //if (customers.Any())
            //{
            //    FillArticle(modelle.First().ModelID);
            //}
            //else
            //{
            //    FillArticle("");
            //}
        }

        protected void GridViewCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void FilterCommand_Click(object sender, EventArgs e)
        {
            FillCustomer(0);
        }

        protected void GridViewCustomer_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            FillCustomer(e.NewPageIndex);
        }
    }
}