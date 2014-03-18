namespace Jinete.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropDownsSuck : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Desktops", "ApplicationUserId", c => c.String(nullable: false));
            AlterColumn("dbo.Laptops", "ApplicationUserId", c => c.String(nullable: false));
            AlterColumn("dbo.Monitors", "ApplicationUserId", c => c.String(nullable: false));
            AlterColumn("dbo.Notebooks", "ApplicationUserId", c => c.String(nullable: false));
            AlterColumn("dbo.Printers", "ApplicationUserId", c => c.String(nullable: false));
            AlterColumn("dbo.Tablets", "ApplicationUserId", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tablets", "ApplicationUserId", c => c.String());
            AlterColumn("dbo.Printers", "ApplicationUserId", c => c.String());
            AlterColumn("dbo.Notebooks", "ApplicationUserId", c => c.String());
            AlterColumn("dbo.Monitors", "ApplicationUserId", c => c.String());
            AlterColumn("dbo.Laptops", "ApplicationUserId", c => c.String());
            AlterColumn("dbo.Desktops", "ApplicationUserId", c => c.String());
        }
    }
}
