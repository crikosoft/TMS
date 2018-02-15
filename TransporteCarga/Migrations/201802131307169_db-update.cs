namespace KleanKart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dbupdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Pagoes", "fechaPagoProg", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Pagoes", "fechaPagoProg", c => c.DateTime(nullable: false));
        }
    }
}
