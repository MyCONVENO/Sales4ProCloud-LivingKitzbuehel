using SALESCenterLivingKB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SALESCenterLivingKB.Admin
{
    public partial class Users : System.Web.UI.Page
    {
        ServerEntities serverModel = new ServerEntities();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public IQueryable<User> usersGrid_GetData()
        {
            return serverModel.User;
        }        
    }
}