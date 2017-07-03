using SALESCenterLivingKB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SALESCenterLivingKB.MasterAdmin
{
    public partial class Users : System.Web.UI.Page
    {
        List<SimpleRole> allroles = null;
        ApplicationDbContext context = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            context = new ApplicationDbContext();

            allroles = (from r in context.Roles
                        select new SimpleRole() { Id = r.Id, Name = r.Name }).ToList();

            allroles.Add(new SimpleRole() { Id = "leer", Name = "alle" });

            if (!this.IsPostBack)
            {
                DropDownRoles.DataSource = allroles;
                DropDownRoles.DataBind();
                DropDownRoles.SelectedIndex = 0;
                FillUsers(0);
            }


        }

        // The id parameter name should match the DataKeyNames value set on the control
        public void usersGrid_DeleteItem(string id)
        {
            ApplicationDbContext appContext = new ApplicationDbContext();
            var user = appContext.Users.FirstOrDefault(u => u.Id == id);

            if (user != null)
            {
                appContext.Users.Remove(user);
                appContext.SaveChanges();
            }
        }

        // The return type can be changed to IEnumerable, however to support
        // paging and sorting, the following parameters must be added:
        //     int maximumRows
        //     int startRowIndex
        //     out int totalRowCount
        //     string sortByExpression       

        protected void addButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddEditAccounts.aspx");
        }

        void FillUsers(int pageIndex)
        {
            var users = context.Users;

            var us = users.Where(u => u.Id != "");
            if (DropDownRoles.SelectedValue != "leer")
            {
                us = us.Where(u => u.Roles.Any(r => r.RoleId == DropDownRoles.SelectedValue.ToString()));
            }

            if (!string.IsNullOrEmpty(tbSearchPattern.Text))
            {
                us = us.Where(u => u.Email.Contains(tbSearchPattern.Text));
            }

            usersGrid.DataSource = us.ToList();
            usersGrid.PageIndex = pageIndex;
            usersGrid.DataBind();
        }

        protected void filterButton_Click(object sender, EventArgs e)
        {
            FillUsers(0);
        }

        protected void usersGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "delete")
            {
                ApplicationDbContext appContext = new ApplicationDbContext();

                usersGrid.SelectedIndex = Convert.ToInt32(e.CommandArgument.ToString());

                var user = appContext.Users.FirstOrDefault(u => u.Id == usersGrid.SelectedValue.ToString());

                if (user != null)
                {
                    appContext.Users.Remove(user);
                    appContext.SaveChanges();
                }
            }
        }

        protected void usersGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            FillUsers(0);
        }

        protected void usersGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            FillUsers(e.NewPageIndex);
        }
    }
}