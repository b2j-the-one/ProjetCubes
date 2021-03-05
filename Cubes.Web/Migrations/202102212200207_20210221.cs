namespace Cubes.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20210221 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        IdUser = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 100),
                        Nom = c.String(nullable: false, maxLength: 50),
                        Prenom = c.String(nullable: false, maxLength: 50),
                        DateNaissance = c.DateTime(nullable: false),
                        IsActivated = c.Boolean(nullable: false),
                        Photo = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.IdUser)
                .Index(t => t.Email, unique: true, name: "Index_Email");
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Users", "Index_Email");
            DropTable("dbo.Users");
        }
    }
}
