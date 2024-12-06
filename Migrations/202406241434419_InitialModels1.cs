namespace FakeCET.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialModels1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Predavacs", "Odobrenje", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Predavacs", "Odobrenje");
        }
    }
}
