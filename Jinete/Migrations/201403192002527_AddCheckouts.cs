namespace Jinete.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCheckouts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Checkouts", "EquipmentType", c => c.String());
            AddColumn("dbo.Checkouts", "EquipmentId", c => c.Int(nullable: false));
            AddColumn("dbo.Desktops", "isCheckedOut", c => c.Boolean(nullable: false));
            AddColumn("dbo.Laptops", "isCheckedOut", c => c.Boolean(nullable: false));
            AddColumn("dbo.Monitors", "isCheckedOut", c => c.Boolean(nullable: false));
            AddColumn("dbo.Notebooks", "isCheckedOut", c => c.Boolean(nullable: false));
            AddColumn("dbo.Printers", "isCheckedOut", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tablets", "isCheckedOut", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tablets", "isCheckedOut");
            DropColumn("dbo.Printers", "isCheckedOut");
            DropColumn("dbo.Notebooks", "isCheckedOut");
            DropColumn("dbo.Monitors", "isCheckedOut");
            DropColumn("dbo.Laptops", "isCheckedOut");
            DropColumn("dbo.Desktops", "isCheckedOut");
            DropColumn("dbo.Checkouts", "EquipmentId");
            DropColumn("dbo.Checkouts", "EquipmentType");
        }
    }
}
