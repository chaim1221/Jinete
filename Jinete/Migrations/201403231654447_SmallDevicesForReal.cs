namespace Jinete.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SmallDevicesForReal : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cameras",
                c => new
                    {
                        CameraId = c.Int(nullable: false, identity: true),
                        EquipmentName = c.String(nullable: false),
                        SerialNumber = c.String(nullable: false),
                        PurchasePrice = c.Double(nullable: false),
                        Discarded = c.DateTime(),
                        LostOrStolen = c.DateTime(),
                        isCheckedOut = c.Boolean(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Sale_SaleId = c.Int(),
                    })
                .PrimaryKey(t => t.CameraId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Sales", t => t.Sale_SaleId)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Sale_SaleId);
            
            CreateTable(
                "dbo.MobilePhones",
                c => new
                    {
                        MobilePhoneId = c.Int(nullable: false, identity: true),
                        EquipmentName = c.String(nullable: false),
                        SerialNumber = c.String(nullable: false),
                        PurchasePrice = c.Double(nullable: false),
                        Discarded = c.DateTime(),
                        LostOrStolen = c.DateTime(),
                        isCheckedOut = c.Boolean(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Sale_SaleId = c.Int(),
                    })
                .PrimaryKey(t => t.MobilePhoneId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Sales", t => t.Sale_SaleId)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Sale_SaleId);
            
            CreateTable(
                "dbo.Telephones",
                c => new
                    {
                        TelephoneId = c.Int(nullable: false, identity: true),
                        EquipmentName = c.String(nullable: false),
                        SerialNumber = c.String(nullable: false),
                        PurchasePrice = c.Double(nullable: false),
                        Discarded = c.DateTime(),
                        LostOrStolen = c.DateTime(),
                        isCheckedOut = c.Boolean(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Sale_SaleId = c.Int(),
                    })
                .PrimaryKey(t => t.TelephoneId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Sales", t => t.Sale_SaleId)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Sale_SaleId);
            
            AddColumn("dbo.Checkouts", "Camera_CameraId", c => c.Int());
            AddColumn("dbo.Checkouts", "MobilePhone_MobilePhoneId", c => c.Int());
            AddColumn("dbo.Checkouts", "Telephone_TelephoneId", c => c.Int());
            CreateIndex("dbo.Checkouts", "Camera_CameraId");
            CreateIndex("dbo.Checkouts", "MobilePhone_MobilePhoneId");
            CreateIndex("dbo.Checkouts", "Telephone_TelephoneId");
            AddForeignKey("dbo.Checkouts", "Camera_CameraId", "dbo.Cameras", "CameraId");
            AddForeignKey("dbo.Checkouts", "MobilePhone_MobilePhoneId", "dbo.MobilePhones", "MobilePhoneId");
            AddForeignKey("dbo.Checkouts", "Telephone_TelephoneId", "dbo.Telephones", "TelephoneId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Telephones", "Sale_SaleId", "dbo.Sales");
            DropForeignKey("dbo.Checkouts", "Telephone_TelephoneId", "dbo.Telephones");
            DropForeignKey("dbo.Telephones", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.MobilePhones", "Sale_SaleId", "dbo.Sales");
            DropForeignKey("dbo.Checkouts", "MobilePhone_MobilePhoneId", "dbo.MobilePhones");
            DropForeignKey("dbo.MobilePhones", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Cameras", "Sale_SaleId", "dbo.Sales");
            DropForeignKey("dbo.Checkouts", "Camera_CameraId", "dbo.Cameras");
            DropForeignKey("dbo.Cameras", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Telephones", new[] { "Sale_SaleId" });
            DropIndex("dbo.Checkouts", new[] { "Telephone_TelephoneId" });
            DropIndex("dbo.Telephones", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.MobilePhones", new[] { "Sale_SaleId" });
            DropIndex("dbo.Checkouts", new[] { "MobilePhone_MobilePhoneId" });
            DropIndex("dbo.MobilePhones", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Cameras", new[] { "Sale_SaleId" });
            DropIndex("dbo.Checkouts", new[] { "Camera_CameraId" });
            DropIndex("dbo.Cameras", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.Checkouts", "Telephone_TelephoneId");
            DropColumn("dbo.Checkouts", "MobilePhone_MobilePhoneId");
            DropColumn("dbo.Checkouts", "Camera_CameraId");
            DropTable("dbo.Telephones");
            DropTable("dbo.MobilePhones");
            DropTable("dbo.Cameras");
        }
    }
}
