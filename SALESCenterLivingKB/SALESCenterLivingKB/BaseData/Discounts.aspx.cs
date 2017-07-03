using SALESCenterLivingKB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SALESCenterLivingKB.BaseData
{
    public partial class Discounts : System.Web.UI.Page
    {

        ServerEntities entityModel = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            entityModel = new ServerEntities();
        }

        // The return type can be changed to IEnumerable, however to support
        // paging and sorting, the following parameters must be added:
        //     int maximumRows
        //     int startRowIndex
        //     out int totalRowCount
        //     string sortByExpression
        public IQueryable<SALESCenterLivingKB.Models.SpecialDiscount> SpecialDiscountGrid_GetData()
        {
            return entityModel.SpecialDiscount.OrderBy(o => o.SpecialDiscountClientID);
        }

        // The id parameter name should match the DataKeyNames value set on the control
        public void SpecialDiscountGrid_UpdateItem(string SpecialDiscountID)
        {
            SALESCenterLivingKB.Models.SpecialDiscount item = entityModel.SpecialDiscount.FirstOrDefault(sp => sp.SpecialDiscountID == SpecialDiscountID);
            // Load the item here, e.g. item = MyDataLayer.Find(id);
            if (item == null)
            {
                // The item wasn't found
                ModelState.AddModelError("", String.Format("Item with id {0} was not found", SpecialDiscountID));
                return;
            }
            item.SyncDateTime = DateTime.Now;
            TryUpdateModel(item);
            if (ModelState.IsValid)
            {
                entityModel.SaveChanges();
            }
        }

        protected void SpecialDiscountGrid_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int index = SpecialDiscountGrid.EditIndex;
            GridViewRow row = SpecialDiscountGrid.Rows[index];

            TextBox startdate = (TextBox)row.FindControl("tbStartDate");
            TextBox enddate = (TextBox)row.FindControl("tbEndDate");

            // Add the updated values to the NewValues dictionary. Use the
            // parameter names declared in the parameterized update query 
            // string for the key names.
            e.NewValues["StartDate"] = Convert.ToDateTime(startdate.Text);
            e.NewValues["EndDate"] = Convert.ToDateTime(enddate.Text);
        }
    }
}