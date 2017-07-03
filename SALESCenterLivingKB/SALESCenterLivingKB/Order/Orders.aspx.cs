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
            GridViewShoppingCartItem.DataSource = entityModel.ShoppingCartItem.Where(s => s.ShoppingCartID == GridViewShoppingCart.SelectedValue.ToString() && s.IsDeleted == false).ToList();
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
            var status = Convert.ToInt32(ddOrderStatus.SelectedValue);

            return entityModel.ShoppingCart.Where(a => a.IsDeleted == false && a.StatusID == status && a.UserID != "michael@coelsch.de" && a.UserID != "email@webersasch.de").OrderByDescending(a => a.OrderDate);
        }

        protected void FilterButton_Click(object sender, EventArgs e)
        {
            GridViewShoppingCart.DataBind();
        }
    }
}