using SALESCenterLivingKB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SALESCenterLivingKB.Order
{
    public partial class NewCustomerOrders : System.Web.UI.Page
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
            GridViewShoppingCartItem.DataSource = entityModel.ShoppingCartItem.Where(s => s.ShoppingCartID == GridViewShoppingCart.SelectedValue.ToString()).ToList();
            GridViewShoppingCartItem.DataBind();
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

            if (GridViewClient.SelectedValue != null)
            {
                selectedValue = GridViewClient.SelectedValue.ToString();
            }

            return entityModel.ShoppingCart.Where(a => a.IsDeleted == false && a.ShoppingCartClientID == selectedValue && a.StatusID < 10 && (a.CustomerNumber == "-1" || a.CustomerID == "-1") && a.UserID != "michael@coelsch.de" && a.UserID != "email@webersasch.de").OrderBy(a => a.OrderDate);
        }

        // The id parameter name should match the DataKeyNames value set on the control
        public void GridViewShoppingCart_UpdateItem(string ShoppingCartID)
        {
            SALESCenterLivingKB.Models.ShoppingCart item = entityModel.ShoppingCart.FirstOrDefault(sp => sp.ShoppingCartID == ShoppingCartID);
            // Load the item here, e.g. item = MyDataLayer.Find(id);
            if (item == null)
            {
                // The item wasn't ShoppingCartID
                ModelState.AddModelError("", String.Format("Item with id {0} was not found", ShoppingCartID));
                return;
            }

            TryUpdateModel(item);

            if (item.CustomerNumber != "-1")
            {
                item.CustomerID = "1";
            }

            if (ModelState.IsValid)
            {
                entityModel.SaveChanges();
            }
        }
    }
}