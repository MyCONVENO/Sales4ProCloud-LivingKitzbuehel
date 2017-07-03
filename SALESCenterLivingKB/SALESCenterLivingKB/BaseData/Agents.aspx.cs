using SALESCenterLivingKB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SALESCenterLivingKB.BaseData
{
    public partial class Agents : System.Web.UI.Page
    {
        ServerEntities serverModel;
        protected void Page_Load(object sender, EventArgs e)
        {
            serverModel = new ServerEntities();
        }

        // The return type can be changed to IEnumerable, however to support
        // paging and sorting, the following parameters must be added:
        //     int maximumRows
        //     int startRowIndex
        //     out int totalRowCount
        //     string sortByExpression
        public IQueryable<SALESCenterLivingKB.Models.Agent> agentsGrid_GetData()
        {
            return serverModel.Agent;
        }

        // The id parameter name should match the DataKeyNames value set on the control
        public void agentsGrid_DeleteItem(int id)
        {

        }
    }
}