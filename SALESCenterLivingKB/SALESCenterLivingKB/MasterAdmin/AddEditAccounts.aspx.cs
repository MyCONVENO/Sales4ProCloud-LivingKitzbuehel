using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SALESCenterLivingKB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SALESCenterLivingKB.MasterAdmin
{
    public partial class AddEditAccounts : System.Web.UI.Page
    {
        ApplicationUser existingUser = null;
        ApplicationDbContext context = null;
        List<SimpleRole> allroles = null;
                
        protected void Page_Load(object sender, EventArgs e)
        {
            context = new ApplicationDbContext();          

            allroles = (from r in context.Roles
                        select new SimpleRole() { Id = r.Id, Name = r.Name }).ToList();

            string productAction = Request.QueryString["UserAction"];

            if (productAction == "add")
            {
                LabelAddStatus.Text = "Benutzerrolle hinzugefügt!";
            }

            if (productAction == "save")
            {
                LabelAddStatus.Text = "Benutzer gespeichert!";
            }

            if (productAction == "remove")
            {
                LabelAddStatus.Text = "Benutzerrolle entfernt!";
            }

            string UserID = Request.QueryString["UserID"];



            if (!string.IsNullOrEmpty(UserID))
            {
                existingUser = context.Users.FirstOrDefault(p => p.Id == UserID);

                if (this.IsPostBack)
                    return;
                tbUserEmail.Text = existingUser.UserName;
                tbUserEmail.Enabled = false;
            }
            else
            {
                Button1.Enabled = false;
            }
        }

        string createNewUser(string userEmail, string password)
        {
            var manager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = new ApplicationUser() { UserName = userEmail, Email = userEmail, EmailConfirmed = true };

            IdentityResult result = manager.Create(user, password);

            existingUser = user;

            if (result.Succeeded)
                return "OK";

            return result.Errors.FirstOrDefault();
        }

        // The return type can be changed to IEnumerable, however to support
        // paging and sorting, the following parameters must be added:
        //     int maximumRows
        //     int startRowIndex
        //     out int totalRowCount
        //     string sortByExpression
        public IQueryable<SimpleRole> rollsGrid_GetData()
        {
            if (existingUser == null)
                return null;

            var roles = from r in allroles
                        join ur in existingUser.Roles
                        on r.Id equals ur.RoleId
                        select new SimpleRole() { Id = r.Id, Name = r.Name };


            return roles.AsQueryable();
        }

        public IQueryable<SimpleRole> GetRoles()
        {
            if (existingUser == null)
                return null;

            var roles = (from r in allroles
                         select new SimpleRole() { Id = r.Id, Name = r.Name }).ToList();

            foreach (var r in existingUser.Roles)
            {
                roles.Remove(roles.First(existingRole => existingRole.Id == r.RoleId));
            }

            return roles.AsQueryable();
        }

        // The id parameter name should match the DataKeyNames value set on the control
        public void rollsGrid_DeleteItem(string id)
        {
            var role = allroles.First(r => r.Id == id);

            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();

            var result = manager.RemoveFromRole(existingUser.Id, role.Name);


            //manager.SendEmail(existingUser.Id, "Benutzerrolle entfernt", "Sie wurden aus der Rolle <" + role.Name + "> entfernt.");

            string pageUrl = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.Count() - Request.Url.Query.Count());
            Response.Redirect(pageUrl + "?UserAction=remove&UserID=" + existingUser.Id);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var role = context.Roles.FirstOrDefault(dbRole => dbRole.Id == DropDownRoles.SelectedValue);

            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();

            var result = manager.AddToRole(existingUser.Id, role.Name);

            if (!result.Succeeded)
            {
                LabelAddStatus.Text = result.Errors.FirstOrDefault();
                return;
            }

            string pageUrl = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.Count() - Request.Url.Query.Count());
            Response.Redirect(pageUrl + "?UserAction=add&UserID=" + existingUser.Id);
        }

        protected void saveButton_Click(object sender, EventArgs e)
        {
            if (existingUser == null)
            {
                var result = createNewUser(tbUserEmail.Text, tbPassword.Text);

                if (result == "OK")
                {
                    string pageUrl = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.Count() - Request.Url.Query.Count());
                    Response.Redirect(pageUrl + "?UserAction=save&UserID=" + existingUser.Id);
                }
                else
                {
                    label1.Text = result;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(tbPassword.Text))
                {
                    var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                    var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
                    string token = manager.GeneratePasswordResetToken(existingUser.Id);
                    var result = manager.ResetPassword(existingUser.Id, token, tbPassword.Text);
                    if (result.Succeeded)
                    {
                        signInManager.SignIn(existingUser, isPersistent: false, rememberBrowser: false);
                        string pageUrl = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.Count() - Request.Url.Query.Count());
                        Response.Redirect(pageUrl + "?UserAction=save&UserID=" + existingUser.Id);
                    }
                }
            }
        }
                      
    }
}