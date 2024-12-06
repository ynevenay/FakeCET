namespace FakeCET.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialModels3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Predavacs", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Predavacs", "UserId");
            AddForeignKey("dbo.Predavacs", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Predavacs", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Predavacs", new[] { "UserId" });
            DropColumn("dbo.Predavacs", "UserId");
        }
    }
}
