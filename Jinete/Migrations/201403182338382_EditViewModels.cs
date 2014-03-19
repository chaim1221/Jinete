namespace Jinete.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditViewModels : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Checkouts", "Username", c => c.String());
            AddColumn("dbo.Checkouts", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Checkouts", "checkedIn");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Checkouts", "checkedIn", c => c.Boolean(nullable: false));
            DropColumn("dbo.Checkouts", "Discriminator");
            DropColumn("dbo.Checkouts", "Username");
        }
    }
}
