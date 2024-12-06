namespace FakeCET.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialModels4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Predavacs", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Predavacs", new[] { "UserId" });
            DropColumn("dbo.Predavacs", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Predavacs", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Predavacs", "UserId");
            AddForeignKey("dbo.Predavacs", "UserId", "dbo.AspNetUsers", "Id");
        }
    }
}
