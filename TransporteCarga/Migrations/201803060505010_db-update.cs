namespace KleanKart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dbupdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Direccions", "orden", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Direccions", "orden");
        }
    }
}
