namespace Jinete.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Jinete.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<Jinete.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        bool AddUserAndRole(ApplicationDbContext context)
        {
            IdentityResult ir;
            //uncomment the following lines to create new role types for users
            var rm = new RoleManager<IdentityRole>
               (new RoleStore<IdentityRole>(context));
            ir = rm.Create(new IdentityRole("Administrator"));
            ir = rm.Create(new IdentityRole("Manager"));
            ir = rm.Create(new IdentityRole("User"));
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));
            var user = new ApplicationUser()
            {
                UserName = "",
                Email = "",
                FirstName = "",
                LastName = "",
                Phone = "",
                Address = "",
                City = "",
                State = "",
                Zip = ""
            };
            ir = um.Create(user, "");
            if (ir.Succeeded == false)
                return ir.Succeeded;
            ir = um.AddToRole(user.Id, "Administrator");
            return ir.Succeeded;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            var result = AddUserAndRole(context);
        }
    }
}
