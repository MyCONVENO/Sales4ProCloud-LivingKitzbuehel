using SALESCenterLivingKB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SALESCenterLivingKB.BaseData
{
    public partial class Seasons : System.Web.UI.Page
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
        public IQueryable<SALESCenterLivingKB.Models.Season> SeasonGrid_GetData()
        {
            return entityModel.Season.Where(o => o.IsDeleted == false);
        }

        // The id parameter name should match the DataKeyNames value set on the control
        public void SeasonGrid_UpdateItem(string SeasonID)
        {
            SALESCenterLivingKB.Models.Season item = entityModel.Season.FirstOrDefault(sp => sp.SeasonID == SeasonID);
            // Load the item here, e.g. item = MyDataLayer.Find(id);
            if (item == null)
            {
                // The item wasn't found
                ModelState.AddModelError("", String.Format("Item with id {0} was not found", SeasonID));
                return;
            }
            item.SyncDateTime = DateTime.Now;
            TryUpdateModel(item);
            if (ModelState.IsValid)
            {
                entityModel.SaveChanges();
            }
        }

        protected void SeasonGrid_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int index = SeasonGrid.EditIndex;
            GridViewRow row = SeasonGrid.Rows[index];

            TextBox startdate = (TextBox)row.FindControl("tbStartDate");
            TextBox enddate = (TextBox)row.FindControl("tbEndDate");

            // Add the updated values to the NewValues dictionary. Use the
            // parameter names declared in the parameterized update query 
            // string for the key names.
            e.NewValues["DeliveryDateStart"] = Convert.ToDateTime(startdate.Text);
            e.NewValues["DeliveryDateEnd"] = Convert.ToDateTime(enddate.Text);
        }
    }
}