namespace Jinete.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EquipmentRefactor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notebooks", "EquipmentName", c => c.String(nullable: false));
            AddColumn("dbo.Notebooks", "SerialNumber", c => c.String(nullable: false));
            AddColumn("dbo.AspNetUsers", "Phone", c => c.String());
            AddColumn("dbo.AspNetUsers", "Address", c => c.String());
            AddColumn("dbo.AspNetUsers", "City", c => c.String());
            AddColumn("dbo.AspNetUsers", "State", c => c.String(maxLength: 2));
            AddColumn("dbo.AspNetUsers", "Zip", c => c.String());
            DropColumn("dbo.Notebooks", "ComputerName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notebooks", "ComputerName", c => c.String(nullable: false));
            DropColumn("dbo.AspNetUsers", "Zip");
            DropColumn("dbo.AspNetUsers", "State");
            DropColumn("dbo.AspNetUsers", "City");
            DropColumn("dbo.AspNetUsers", "Address");
            DropColumn("dbo.AspNetUsers", "Phone");
            DropColumn("dbo.Notebooks", "SerialNumber");
            DropColumn("dbo.Notebooks", "EquipmentName");
        }
    }
}
