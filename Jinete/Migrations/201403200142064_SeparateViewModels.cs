namespace Jinete.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeparateViewModels : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Checkouts", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Checkouts", new[] { "ApplicationUser_Id" });
            RenameColumn(table: "dbo.Desktops", name: "SaleId", newName: "Sale_SaleId");
            RenameColumn(table: "dbo.Laptops", name: "SaleId", newName: "Sale_SaleId");
            RenameColumn(table: "dbo.Monitors", name: "SaleId", newName: "Sale_SaleId");
            RenameColumn(table: "dbo.Notebooks", name: "SaleId", newName: "Sale_SaleId");
            RenameColumn(table: "dbo.Printers", name: "SaleId", newName: "Sale_SaleId");
            RenameColumn(table: "dbo.Tablets", name: "SaleId", newName: "Sale_SaleId");
            AlterColumn("dbo.Checkouts", "EquipmentType", c => c.String(nullable: false));
            AlterColumn("dbo.Checkouts", "ApplicationUser_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Checkouts", "ApplicationUser_Id");
            AddForeignKey("dbo.Checkouts", "ApplicationUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            DropColumn("dbo.Checkouts", "Username");
            DropColumn("dbo.Checkouts", "Discriminator");
            DropColumn("dbo.Notebooks", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notebooks", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Checkouts", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Checkouts", "Username", c => c.String());
            DropForeignKey("dbo.Checkouts", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Checkouts", new[] { "ApplicationUser_Id" });
            AlterColumn("dbo.Checkouts", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.Checkouts", "EquipmentType", c => c.String());
            RenameColumn(table: "dbo.Tablets", name: "Sale_SaleId", newName: "SaleId");
            RenameColumn(table: "dbo.Printers", name: "Sale_SaleId", newName: "SaleId");
            RenameColumn(table: "dbo.Notebooks", name: "Sale_SaleId", newName: "SaleId");
            RenameColumn(table: "dbo.Monitors", name: "Sale_SaleId", newName: "SaleId");
            RenameColumn(table: "dbo.Laptops", name: "Sale_SaleId", newName: "SaleId");
            RenameColumn(table: "dbo.Desktops", name: "Sale_SaleId", newName: "SaleId");
            CreateIndex("dbo.Checkouts", "ApplicationUser_Id");
            AddForeignKey("dbo.Checkouts", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
