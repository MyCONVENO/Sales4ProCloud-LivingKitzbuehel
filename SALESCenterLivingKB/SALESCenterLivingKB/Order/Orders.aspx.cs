using SALESCenterLivingKB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SALESCenterLivingKB.Order
{
    public partial class Orders : System.Web.UI.Page
    {
        ServerEntities entityModel = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            entityModel = new ServerEntities();
            entityModel.Configuration.LazyLoadingEnabled = false;
        }

        protected void GridViewClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewShoppingCart.PageIndex = 0;
            GridViewShoppingCart.DataBind();
        }

        protected void GridViewShoppingCart_SelectedIndexChanged(object sender, EventArgs e)
        {
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
            GridViewShoppingCartItem.DataBind();

            var status = Convert.ToInt32(ddOrderStatus.SelectedValue);

            IQueryable<ShoppingCart> orders = entityModel.ShoppingCart.Where(a => a.IsDeleted == false && a.StatusID == status && a.UserID != "michael@coelsch.de" && a.UserID != "email@webersasch.de");

            if (!string.IsNullOrEmpty(tbFilter.Text))
            {
                orders = orders.Where(r => r.OrderNumber.Contains(tbFilter.Text) || r.CustomerNumber == tbFilter.Text || r.CustomerName1.Contains(tbFilter.Text) || r.CustomerName2.Contains(tbFilter.Text) || r.AssociationMemberNumber.Contains(tbFilter.Text));
            }


            return orders.OrderByDescending(a => a.OrderDate);
        }

        protected void FilterButton_Click(object sender, EventArgs e)
        {
            GridViewShoppingCart.DataBind();
        }

        protected void tranferOrderButton_Click(object sender, EventArgs e)
        {
            string cartID = GridViewShoppingCart.SelectedValue.ToString();

            entityModel.Database.ExecuteSqlCommand("UPDATE ShoppingCart SET OrderNumber = CAST(CAST(OrderNumber AS bigint) + 1 AS nvarchar(50)), StatusID = 0 WHERE ShoppingCartID = N'" + cartID + "'");

            Response.Redirect("Orders");
        }

        protected void reCalcButton_Click(object sender, EventArgs e)
        {
            string cartID = GridViewShoppingCart.SelectedValue.ToString();

            entityModel.Database.ExecuteSqlCommand("UPDATE ShoppingCartItem SET    BuyingPrice = Price.BuyingPrice, SalesPrice = Price.SalesPrice FROM  Sizerun INNER JOIN Price ON Sizerun.SizerunID = Price.PriceSizerunID INNER JOIN ShoppingCartItem ON Sizerun.EAN01 = ShoppingCartItem.EAN01 WHERE(ShoppingCartItem.ShoppingCartID = N'" + cartID + "') AND(ShoppingCartItem.IsDeleted = 0) AND(Sizerun.IsDeleted = 0) AND(Price.IsDeleted = 0)");

            GridViewShoppingCartItem.DataBind();
        }

        // The id parameter name should match the DataKeyNames value set on the control
        public void GridViewShoppingCartItem_UpdateItem(string ShoppingCartItemID)
        {
            SALESCenterLivingKB.Models.ShoppingCartItem item = entityModel.ShoppingCartItem.FirstOrDefault(s => s.ShoppingCartItemID == ShoppingCartItemID);
            // Load the item here, e.g. item = MyDataLayer.Find(id);
            if (item == null)
            {
                // The item wasn't found
                ModelState.AddModelError("", String.Format("Item with id {0} was not found", ShoppingCartItemID));
                return;
            }
            TryUpdateModel(item);
            if (ModelState.IsValid)
            {
                entityModel.SaveChanges();
            }
        }

        // The return type can be changed to IEnumerable, however to support
        // paging and sorting, the following parameters must be added:
        //     int maximumRows
        //     int startRowIndex
        //     out int totalRowCount
        //     string sortByExpression
        public IQueryable<SALESCenterLivingKB.Models.ShoppingCartItem> GridViewShoppingCartItem_GetData()
        {
            if (GridViewShoppingCart.SelectedValue == null)
            {
                editOrder.Visible = false;
                return null;
            }

            ShoppingCart cart = entityModel.ShoppingCart.FirstOrDefault(s => s.ShoppingCartID == GridViewShoppingCart.SelectedValue.ToString());

            editOrder.Visible = cart != null && cart.StatusID != 10;

            return entityModel.ShoppingCartItem.Where(s => s.ShoppingCartID == GridViewShoppingCart.SelectedValue.ToString() && s.IsDeleted == false);
        }
    }
}