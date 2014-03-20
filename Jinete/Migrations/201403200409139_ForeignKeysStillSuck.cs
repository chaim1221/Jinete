namespace Jinete.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForeignKeysStillSuck : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Checkouts", "EquipmentId");
            DropColumn("dbo.Checkouts", "EquipmentType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Checkouts", "EquipmentType", c => c.String(nullable: false));
            AddColumn("dbo.Checkouts", "EquipmentId", c => c.Int(nullable: false));
        }
    }
}
