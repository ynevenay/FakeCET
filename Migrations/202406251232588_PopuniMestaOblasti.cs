namespace FakeCET.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopuniMestaOblasti : DbMigration
    {
        public override void Up()
        {
            //oblasti
            Sql("INSERT INTO Oblasts (Naziv) VALUES ('Grafika, CAD')");
            Sql("INSERT INTO Oblasts (Naziv) VALUES ('Web dizajn')");
            Sql("INSERT INTO Oblasts (Naziv) VALUES ('MCSA SQL 2016')");
            Sql("INSERT INTO Oblasts (Naziv) VALUES ('Programerski')");
            Sql("INSERT INTO Oblasts (Naziv) VALUES ('Office')");
            Sql("INSERT INTO Oblasts (Naziv) VALUES ('Hardver, Linux, IT')");
            Sql("INSERT INTO Oblasts (Naziv) VALUES ('Za decu i ucenike')");
            Sql("INSERT INTO Oblasts (Naziv) VALUES ('MCSA Web Applications')");
            Sql("INSERT INTO Oblasts (Naziv) VALUES ('MCSA Windows Server')");
            Sql("INSERT INTO Oblasts (Naziv) VALUES ('Microsoft Official Courses on demand')");

            //mesta
            Sql("INSERT INTO MestoOdrzavanjas (Naziv) VALUES ('Online - Zoom app')");
            Sql("INSERT INTO MestoOdrzavanjas (Naziv) VALUES ('RAF 7 - Knez Mihailova 6/6')");
            Sql("INSERT INTO MestoOdrzavanjas (Naziv) VALUES ('RAF 5 - Knez Mihailova 6/6')");
            Sql("INSERT INTO MestoOdrzavanjas (Naziv) VALUES ('RAF 6 - Knez Mihailova 6/6')");
            Sql("INSERT INTO MestoOdrzavanjas (Naziv) VALUES ('RAF 20 - Knez Mihailova 6/3')");
            Sql("INSERT INTO MestoOdrzavanjas (Naziv) VALUES ('CET 1 - Trg republike 5/5')");
            //updateDB
        }

        public override void Down()
        {
        }
    }
}
