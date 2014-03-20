namespace Jinete.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForeignKeyNightmares : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Checkouts", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Checkouts", "Desktop_DesktopId", c => c.Int());
            AddColumn("dbo.Checkouts", "Laptop_LaptopId", c => c.Int());
            AddColumn("dbo.Checkouts", "Monitor_MonitorId", c => c.Int());
            AddColumn("dbo.Checkouts", "Notebook_NotebookId", c => c.Int());
            AddColumn("dbo.Checkouts", "Printer_PrinterId", c => c.Int());
            AddColumn("dbo.Checkouts", "Tablet_TabletId", c => c.Int());
            AddColumn("dbo.Desktops", "ApplicationUser_Id", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Laptops", "ApplicationUser_Id", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Monitors", "ApplicationUser_Id", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Notebooks", "ApplicationUser_Id", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Printers", "ApplicationUser_Id", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Tablets", "ApplicationUser_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Checkouts", "ApplicationUser_Id");
            CreateIndex("dbo.Desktops", "ApplicationUser_Id");
            CreateIndex("dbo.Checkouts", "Desktop_DesktopId");
            CreateIndex("dbo.Desktops", "SaleId");
            CreateIndex("dbo.Laptops", "ApplicationUser_Id");
            CreateIndex("dbo.Checkouts", "Laptop_LaptopId");
            CreateIndex("dbo.Laptops", "SaleId");
            CreateIndex("dbo.Monitors", "ApplicationUser_Id");
            CreateIndex("dbo.Checkouts", "Monitor_MonitorId");
            CreateIndex("dbo.Monitors", "SaleId");
            CreateIndex("dbo.Notebooks", "ApplicationUser_Id");
            CreateIndex("dbo.Checkouts", "Notebook_NotebookId");
            CreateIndex("dbo.Notebooks", "SaleId");
            CreateIndex("dbo.Printers", "ApplicationUser_Id");
            CreateIndex("dbo.Checkouts", "Printer_PrinterId");
            CreateIndex("dbo.Printers", "SaleId");
            CreateIndex("dbo.Tablets", "ApplicationUser_Id");
            CreateIndex("dbo.Checkouts", "Tablet_TabletId");
            CreateIndex("dbo.Tablets", "SaleId");
            AddForeignKey("dbo.Checkouts", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Desktops", "ApplicationUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Checkouts", "Desktop_DesktopId", "dbo.Desktops", "DesktopId");
            AddForeignKey("dbo.Desktops", "SaleId", "dbo.Sales", "SaleId");
            AddForeignKey("dbo.Laptops", "ApplicationUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Checkouts", "Laptop_LaptopId", "dbo.Laptops", "LaptopId");
            AddForeignKey("dbo.Laptops", "SaleId", "dbo.Sales", "SaleId");
            AddForeignKey("dbo.Monitors", "ApplicationUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Checkouts", "Monitor_MonitorId", "dbo.Monitors", "MonitorId");
            AddForeignKey("dbo.Monitors", "SaleId", "dbo.Sales", "SaleId");
            Sql("UPDATE [dbo].[Notebooks] SET ApplicationUser_Id = (SELECT TOP 1 Id FROM [dbo].[AspNetUsers] A LEFT JOIN [dbo].[Notebooks] N on A.Id = N.ApplicationUser_Id WHERE N.ApplicationUser_Id is null)");
            AddForeignKey("dbo.Notebooks", "ApplicationUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Checkouts", "Notebook_NotebookId", "dbo.Notebooks", "NotebookId");
            AddForeignKey("dbo.Notebooks", "SaleId", "dbo.Sales", "SaleId");
            AddForeignKey("dbo.Printers", "ApplicationUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Checkouts", "Printer_PrinterId", "dbo.Printers", "PrinterId");
            AddForeignKey("dbo.Printers", "SaleId", "dbo.Sales", "SaleId");
            AddForeignKey("dbo.Tablets", "ApplicationUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Checkouts", "Tablet_TabletId", "dbo.Tablets", "TabletId");
            AddForeignKey("dbo.Tablets", "SaleId", "dbo.Sales", "SaleId");
            DropColumn("dbo.Checkouts", "ApplicationUserId");
            DropColumn("dbo.Desktops", "ApplicationUserId");
            DropColumn("dbo.Laptops", "ApplicationUserId");
            DropColumn("dbo.Monitors", "ApplicationUserId");
            DropColumn("dbo.Notebooks", "ApplicationUserId");
            DropColumn("dbo.Printers", "ApplicationUserId");
            DropColumn("dbo.Tablets", "ApplicationUserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tablets", "ApplicationUserId", c => c.String(nullable: false));
            AddColumn("dbo.Printers", "ApplicationUserId", c => c.String(nullable: false));
            AddColumn("dbo.Notebooks", "ApplicationUserId", c => c.String(nullable: false));
            AddColumn("dbo.Monitors", "ApplicationUserId", c => c.String(nullable: false));
            AddColumn("dbo.Laptops", "ApplicationUserId", c => c.String(nullable: false));
            AddColumn("dbo.Desktops", "ApplicationUserId", c => c.String(nullable: false));
            AddColumn("dbo.Checkouts", "ApplicationUserId", c => c.String());
            DropForeignKey("dbo.Tablets", "SaleId", "dbo.Sales");
            DropForeignKey("dbo.Checkouts", "Tablet_TabletId", "dbo.Tablets");
            DropForeignKey("dbo.Tablets", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Printers", "SaleId", "dbo.Sales");
            DropForeignKey("dbo.Checkouts", "Printer_PrinterId", "dbo.Printers");
            DropForeignKey("dbo.Printers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Notebooks", "SaleId", "dbo.Sales");
            DropForeignKey("dbo.Checkouts", "Notebook_NotebookId", "dbo.Notebooks");
            DropForeignKey("dbo.Notebooks", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Monitors", "SaleId", "dbo.Sales");
            DropForeignKey("dbo.Checkouts", "Monitor_MonitorId", "dbo.Monitors");
            DropForeignKey("dbo.Monitors", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Laptops", "SaleId", "dbo.Sales");
            DropForeignKey("dbo.Checkouts", "Laptop_LaptopId", "dbo.Laptops");
            DropForeignKey("dbo.Laptops", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Desktops", "SaleId", "dbo.Sales");
            DropForeignKey("dbo.Checkouts", "Desktop_DesktopId", "dbo.Desktops");
            DropForeignKey("dbo.Desktops", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Checkouts", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Tablets", new[] { "SaleId" });
            DropIndex("dbo.Checkouts", new[] { "Tablet_TabletId" });
            DropIndex("dbo.Tablets", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Printers", new[] { "SaleId" });
            DropIndex("dbo.Checkouts", new[] { "Printer_PrinterId" });
            DropIndex("dbo.Printers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Notebooks", new[] { "SaleId" });
            DropIndex("dbo.Checkouts", new[] { "Notebook_NotebookId" });
            DropIndex("dbo.Notebooks", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Monitors", new[] { "SaleId" });
            DropIndex("dbo.Checkouts", new[] { "Monitor_MonitorId" });
            DropIndex("dbo.Monitors", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Laptops", new[] { "SaleId" });
            DropIndex("dbo.Checkouts", new[] { "Laptop_LaptopId" });
            DropIndex("dbo.Laptops", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Desktops", new[] { "SaleId" });
            DropIndex("dbo.Checkouts", new[] { "Desktop_DesktopId" });
            DropIndex("dbo.Desktops", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Checkouts", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.Tablets", "ApplicationUser_Id");
            DropColumn("dbo.Printers", "ApplicationUser_Id");
            DropColumn("dbo.Notebooks", "ApplicationUser_Id");
            DropColumn("dbo.Monitors", "ApplicationUser_Id");
            DropColumn("dbo.Laptops", "ApplicationUser_Id");
            DropColumn("dbo.Desktops", "ApplicationUser_Id");
            DropColumn("dbo.Checkouts", "Tablet_TabletId");
            DropColumn("dbo.Checkouts", "Printer_PrinterId");
            DropColumn("dbo.Checkouts", "Notebook_NotebookId");
            DropColumn("dbo.Checkouts", "Monitor_MonitorId");
            DropColumn("dbo.Checkouts", "Laptop_LaptopId");
            DropColumn("dbo.Checkouts", "Desktop_DesktopId");
            DropColumn("dbo.Checkouts", "ApplicationUser_Id");
        }
    }
}
