using SALESCenterLivingKB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SALESCenterLivingKB.Order
{
    public partial class OpenOrders : System.Web.UI.Page
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
            GridViewShoppingCart.PageIndex = 0;
            GridViewShoppingCart.DataBind();
        }       

        protected void GridViewShoppingCart_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        // The return type can be changed to IEnumerable, however to support
        // paging and sorting, the following parameters must be added:
        //     int maximumRows
        //     int startRowIndex
        //     out int totalRowCount
        //     string sortByExpression
        public IQueryable<SALESCenterLivingKB.Models.ShoppingCart> GridViewShoppingCart_GetData()
        {

            string selectedValue = entityModel.Client.OrderBy(c => c.ClientID).FirstOrDefault(c => c.IsDeleted == false).ClientID;

            if(GridViewClient.SelectedValue != null)
            {
                selectedValue = GridViewClient.SelectedValue.ToString();
            }

            return entityModel.ShoppingCart.Where(a => a.IsDeleted == false && a.ShoppingCartClientID == selectedValue && a.StatusID == 10).OrderBy(a => a.OrderDate);
        }
    }
}