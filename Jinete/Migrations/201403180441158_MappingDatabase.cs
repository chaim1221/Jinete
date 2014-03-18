namespace Jinete.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MappingDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Checkouts",
                c => new
                    {
                        CheckoutId = c.Int(nullable: false, identity: true),
                        ApplicationUserId = c.String(),
                        dtCheckedOut = c.DateTime(nullable: false),
                        dtReturned = c.DateTime(),
                        checkedIn = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CheckoutId);
            
            CreateTable(
                "dbo.Desktops",
                c => new
                    {
                        DesktopId = c.Int(nullable: false, identity: true),
                        MonitorId = c.Int(nullable: false),
                        EquipmentName = c.String(nullable: false),
                        SerialNumber = c.String(nullable: false),
                        PurchasePrice = c.Double(nullable: false),
                        Discarded = c.DateTime(),
                        LostOrStolen = c.DateTime(),
                        ApplicationUserId = c.String(),
                        SaleId = c.Int(),
                    })
                .PrimaryKey(t => t.DesktopId);
            
            CreateTable(
                "dbo.Laptops",
                c => new
                    {
                        LaptopId = c.Int(nullable: false, identity: true),
                        EquipmentName = c.String(nullable: false),
                        SerialNumber = c.String(nullable: false),
                        PurchasePrice = c.Double(nullable: false),
                        Discarded = c.DateTime(),
                        LostOrStolen = c.DateTime(),
                        ApplicationUserId = c.String(),
                        SaleId = c.Int(),
                    })
                .PrimaryKey(t => t.LaptopId);
            
            CreateTable(
                "dbo.Monitors",
                c => new
                    {
                        MonitorId = c.Int(nullable: false, identity: true),
                        EquipmentName = c.String(nullable: false),
                        SerialNumber = c.String(nullable: false),
                        PurchasePrice = c.Double(nullable: false),
                        Discarded = c.DateTime(),
                        LostOrStolen = c.DateTime(),
                        ApplicationUserId = c.String(),
                        SaleId = c.Int(),
                    })
                .PrimaryKey(t => t.MonitorId);
            
            CreateTable(
                "dbo.Printers",
                c => new
                    {
                        PrinterId = c.Int(nullable: false, identity: true),
                        EquipmentName = c.String(nullable: false),
                        SerialNumber = c.String(nullable: false),
                        PurchasePrice = c.Double(nullable: false),
                        Discarded = c.DateTime(),
                        LostOrStolen = c.DateTime(),
                        ApplicationUserId = c.String(),
                        SaleId = c.Int(),
                    })
                .PrimaryKey(t => t.PrinterId);
            
            CreateTable(
                "dbo.Sales",
                c => new
                    {
                        SaleId = c.Int(nullable: false, identity: true),
                        dtSold = c.DateTime(nullable: false),
                        SalePrice = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.SaleId);
            
            CreateTable(
                "dbo.Tablets",
                c => new
                    {
                        TabletId = c.Int(nullable: false, identity: true),
                        EquipmentName = c.String(nullable: false),
                        SerialNumber = c.String(nullable: false),
                        PurchasePrice = c.Double(nullable: false),
                        Discarded = c.DateTime(),
                        LostOrStolen = c.DateTime(),
                        ApplicationUserId = c.String(),
                        SaleId = c.Int(),
                    })
                .PrimaryKey(t => t.TabletId);
            
            AddColumn("dbo.Notebooks", "PurchasePrice", c => c.Double(nullable: false));
            AddColumn("dbo.Notebooks", "Discarded", c => c.DateTime());
            AddColumn("dbo.Notebooks", "LostOrStolen", c => c.DateTime());
            AddColumn("dbo.Notebooks", "ApplicationUserId", c => c.String());
            AddColumn("dbo.Notebooks", "SaleId", c => c.Int());
            AddColumn("dbo.Notebooks", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Notebooks", "dtCheckedOut");
            DropColumn("dbo.Notebooks", "dtReturned");
            DropColumn("dbo.Notebooks", "checkedIn");
            DropColumn("dbo.Notebooks", "PersonFirstName");
            DropColumn("dbo.Notebooks", "PersonLastName");
            DropColumn("dbo.Notebooks", "UserID");
            DropColumn("dbo.Notebooks", "Phone");
            DropColumn("dbo.Notebooks", "Address");
            DropColumn("dbo.Notebooks", "City");
            DropColumn("dbo.Notebooks", "State");
            DropColumn("dbo.Notebooks", "Zip");
            DropColumn("dbo.Notebooks", "Email");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notebooks", "Email", c => c.String(nullable: false));
            AddColumn("dbo.Notebooks", "Zip", c => c.String(nullable: false));
            AddColumn("dbo.Notebooks", "State", c => c.String(nullable: false, maxLength: 2));
            AddColumn("dbo.Notebooks", "City", c => c.String(nullable: false));
            AddColumn("dbo.Notebooks", "Address", c => c.String(nullable: false));
            AddColumn("dbo.Notebooks", "Phone", c => c.String(nullable: false));
            AddColumn("dbo.Notebooks", "UserID", c => c.String(nullable: false));
            AddColumn("dbo.Notebooks", "PersonLastName", c => c.String(nullable: false));
            AddColumn("dbo.Notebooks", "PersonFirstName", c => c.String(nullable: false));
            AddColumn("dbo.Notebooks", "checkedIn", c => c.Boolean(nullable: false));
            AddColumn("dbo.Notebooks", "dtReturned", c => c.DateTime());
            AddColumn("dbo.Notebooks", "dtCheckedOut", c => c.DateTime(nullable: false));
            DropColumn("dbo.Notebooks", "Discriminator");
            DropColumn("dbo.Notebooks", "SaleId");
            DropColumn("dbo.Notebooks", "ApplicationUserId");
            DropColumn("dbo.Notebooks", "LostOrStolen");
            DropColumn("dbo.Notebooks", "Discarded");
            DropColumn("dbo.Notebooks", "PurchasePrice");
            DropTable("dbo.Tablets");
            DropTable("dbo.Sales");
            DropTable("dbo.Printers");
            DropTable("dbo.Monitors");
            DropTable("dbo.Laptops");
            DropTable("dbo.Desktops");
            DropTable("dbo.Checkouts");
        }
    }
}
