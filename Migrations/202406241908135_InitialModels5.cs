namespace FakeCET.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialModels5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Polazniks", "Lozinka", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.Polazniks", "PotvrdiLozinku", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Polazniks", "PotvrdiLozinku");
            DropColumn("dbo.Polazniks", "Lozinka");
        }
    }
}
