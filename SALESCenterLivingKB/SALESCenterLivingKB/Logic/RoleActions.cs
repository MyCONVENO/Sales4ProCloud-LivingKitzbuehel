using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SALESCenterLivingKB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SALESCenterLivingKB.Logic
{
    public class RoleActions
    {
        internal void createAdmin()
        {
            // Access the application context and create result variables.
            ApplicationDbContext context = new ApplicationDbContext();
            IdentityResult IdRoleResult;
            IdentityResult IdUserResult;

            // Create a RoleStore object by using the ApplicationDbContext object. 
            // The RoleStore is only allowed to contain IdentityRole objects.
            var roleStore = new RoleStore<IdentityRole>(context);

            // Create a RoleManager object that is only allowed to contain IdentityRole objects.
            // When creating the RoleManager object, you pass in (as a parameter) a new RoleStore object. 
            var roleMgr = new RoleManager<IdentityRole>(roleStore);

            // Then, you create the "canEdit" role if it doesn't already exist.
            if (!roleMgr.RoleExists("Administrator"))
            {
                IdRoleResult = roleMgr.Create(new IdentityRole { Name = "Administrator" });
            }

            //if (!roleMgr.RoleExists("Contractor"))
            //{
            //    IdRoleResult = roleMgr.Create(new IdentityRole { Name = "Contractor" });
            //}

            // Create a UserManager object based on the UserStore object and the ApplicationDbContext  
            // object. Note that you can create new objects and use them as parameters in
            // a single line of code, rather than using multiple lines of code, as you did
            // for the RoleManager object.
            var userMgr = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var appUser = new ApplicationUser
            {
                UserName = "sascha.weber@myconveno.de",
                Email = "sascha.weber@myconveno.de",
                EmailConfirmed = true
            };
            IdUserResult = userMgr.Create(appUser, "Password");

            // If the new "canEdit" user was successfully created, 
            // add the "canEdit" user to the "canEdit" role. 
            if (IdUserResult.Succeeded)
            {
                IdUserResult = userMgr.AddToRole(appUser.Id, "Administrator");
            }
        }

        internal bool createUser(string UserAddress, string password)
        {
            // Access the application context and create result variables.
            ApplicationDbContext context = new ApplicationDbContext();

            var userMgr = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var appUser = new ApplicationUser
            {
                UserName = UserAddress,
                Email = UserAddress,
                EmailConfirmed = true
            };
            return userMgr.Create(appUser, password).Succeeded;

        }
    }
}