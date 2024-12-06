namespace FakeCET.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Kurs",
                c => new
                    {
                        KursID = c.Int(nullable: false, identity: true),
                        Naziv = c.String(nullable: false, maxLength: 75),
                        Trajanje = c.Int(nullable: false),
                        Cena = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Opis = c.String(nullable: false),
                        OblastID = c.Int(),
                    })
                .PrimaryKey(t => t.KursID)
                .ForeignKey("dbo.Oblasts", t => t.OblastID)
                .Index(t => t.OblastID);
            
            CreateTable(
                "dbo.Oblasts",
                c => new
                    {
                        OblastID = c.Int(nullable: false, identity: true),
                        Naziv = c.String(nullable: false, maxLength: 55),
                    })
                .PrimaryKey(t => t.OblastID);
            
            CreateTable(
                "dbo.PredavacKurs",
                c => new
                    {
                        KursID = c.Int(nullable: false),
                        PredavacID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.KursID, t.PredavacID })
                .ForeignKey("dbo.Kurs", t => t.KursID, cascadeDelete: true)
                .ForeignKey("dbo.Predavacs", t => t.PredavacID, cascadeDelete: true)
                .Index(t => t.KursID)
                .Index(t => t.PredavacID);
            
            CreateTable(
                "dbo.Predavacs",
                c => new
                    {
                        PredavacID = c.Int(nullable: false, identity: true),
                        Ime = c.String(nullable: false, maxLength: 25),
                        Prezime = c.String(nullable: false, maxLength: 25),
                        KontaktTelefon = c.String(nullable: false, maxLength: 12),
                        MejlAdresa = c.String(nullable: false, maxLength: 100),
                        Zvanje = c.String(nullable: false, maxLength: 255),
                        OblastRada = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.PredavacID);
            
            CreateTable(
                "dbo.Termins",
                c => new
                    {
                        TerminID = c.Int(nullable: false, identity: true),
                        KursID = c.Int(nullable: false),
                        PredavacID = c.Int(nullable: false),
                        DatumPocetka = c.DateTime(nullable: false),
                        DatumZavrsetka = c.DateTime(nullable: false),
                        Status = c.Boolean(nullable: false),
                        MestoID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TerminID)
                .ForeignKey("dbo.MestoOdrzavanjas", t => t.MestoID, cascadeDelete: true)
                .ForeignKey("dbo.PredavacKurs", t => new { t.KursID, t.PredavacID }, cascadeDelete: true)
                .Index(t => new { t.KursID, t.PredavacID })
                .Index(t => t.MestoID);
            
            CreateTable(
                "dbo.MestoOdrzavanjas",
                c => new
                    {
                        MestoID = c.Int(nullable: false, identity: true),
                        Naziv = c.String(nullable: false, maxLength: 55),
                    })
                .PrimaryKey(t => t.MestoID);
            
            CreateTable(
                "dbo.PrijavaKurs",
                c => new
                    {
                        PrijavaID = c.Int(nullable: false, identity: true),
                        TerminID = c.Int(nullable: false),
                        PolaznikID = c.Int(nullable: false),
                        DatumPrijave = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PrijavaID, t.TerminID, t.PolaznikID })
                .ForeignKey("dbo.Polazniks", t => t.PolaznikID, cascadeDelete: true)
                .ForeignKey("dbo.Termins", t => t.TerminID, cascadeDelete: true)
                .Index(t => t.TerminID)
                .Index(t => t.PolaznikID);
            
            CreateTable(
                "dbo.Polazniks",
                c => new
                    {
                        PolaznikID = c.Int(nullable: false, identity: true),
                        Ime = c.String(nullable: false, maxLength: 25),
                        Prezime = c.String(nullable: false, maxLength: 25),
                        KontaktTelefon = c.String(nullable: false, maxLength: 12),
                        MejlAdresa = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.PolaznikID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.PrijavaKurs", "TerminID", "dbo.Termins");
            DropForeignKey("dbo.PrijavaKurs", "PolaznikID", "dbo.Polazniks");
            DropForeignKey("dbo.Termins", new[] { "KursID", "PredavacID" }, "dbo.PredavacKurs");
            DropForeignKey("dbo.Termins", "MestoID", "dbo.MestoOdrzavanjas");
            DropForeignKey("dbo.PredavacKurs", "PredavacID", "dbo.Predavacs");
            DropForeignKey("dbo.PredavacKurs", "KursID", "dbo.Kurs");
            DropForeignKey("dbo.Kurs", "OblastID", "dbo.Oblasts");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.PrijavaKurs", new[] { "PolaznikID" });
            DropIndex("dbo.PrijavaKurs", new[] { "TerminID" });
            DropIndex("dbo.Termins", new[] { "MestoID" });
            DropIndex("dbo.Termins", new[] { "KursID", "PredavacID" });
            DropIndex("dbo.PredavacKurs", new[] { "PredavacID" });
            DropIndex("dbo.PredavacKurs", new[] { "KursID" });
            DropIndex("dbo.Kurs", new[] { "OblastID" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Polazniks");
            DropTable("dbo.PrijavaKurs");
            DropTable("dbo.MestoOdrzavanjas");
            DropTable("dbo.Termins");
            DropTable("dbo.Predavacs");
            DropTable("dbo.PredavacKurs");
            DropTable("dbo.Oblasts");
            DropTable("dbo.Kurs");
        }
    }
}
