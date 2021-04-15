namespace Cubes.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20210415 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        IdCategorie = c.Int(nullable: false, identity: true),
                        Libelle = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.IdCategorie);
            
            CreateTable(
                "dbo.Ressources",
                c => new
                    {
                        IdRessource = c.Int(nullable: false, identity: true),
                        Nom = c.String(nullable: false, maxLength: 50),
                        IdCategorie = c.Int(nullable: false),
                        User = c.String(),
                        Titre = c.String(maxLength: 50),
                        IsPrivate = c.Boolean(nullable: false),
                        IsValid = c.Boolean(nullable: false),
                        IsPublish = c.Boolean(nullable: false),
                        IsFavorite = c.Boolean(nullable: false),
                        IsOperate = c.Boolean(nullable: false),
                        DateCreation = c.DateTime(),
                        DatePublication = c.DateTime(),
                        Fichier = c.String(maxLength: 200),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.IdRessource)
                .ForeignKey("dbo.Categories", t => t.IdCategorie, cascadeDelete: true)
                .Index(t => t.IdCategorie);
            
            CreateTable(
                "dbo.Commentaires",
                c => new
                    {
                        IdCommentaire = c.Int(nullable: false, identity: true),
                        Text = c.String(nullable: false),
                        DateCommentaire = c.DateTime(),
                        IdUser = c.Int(nullable: false),
                        IdRessource = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdCommentaire)
                .ForeignKey("dbo.Ressources", t => t.IdRessource, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.IdUser, cascadeDelete: true)
                .Index(t => t.IdUser)
                .Index(t => t.IdRessource);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        IdUser = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 100),
                        Nom = c.String(nullable: false, maxLength: 50),
                        Prenom = c.String(nullable: false, maxLength: 50),
                        Telephone = c.String(),
                        DateNaissance = c.DateTime(nullable: false),
                        DateInscription = c.DateTime(),
                        IsActivated = c.Boolean(nullable: false),
                        DateBannissement = c.DateTime(),
                        DateFinBannissement = c.DateTime(),
                        Photo = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.IdUser)
                .Index(t => t.Email, unique: true, name: "Index_Email");
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        IdMessage = c.Int(nullable: false, identity: true),
                        Text = c.String(nullable: false),
                        DateEnvoie = c.DateTime(),
                        IdUser = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdMessage)
                .ForeignKey("dbo.Users", t => t.IdUser, cascadeDelete: true)
                .Index(t => t.IdUser);
            
            CreateTable(
                "dbo.Replies",
                c => new
                    {
                        IdReply = c.Int(nullable: false, identity: true),
                        Text = c.String(nullable: false),
                        DateReponse = c.DateTime(),
                        IdUser = c.Int(nullable: true),
                        IdMessage = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdReply)
                .ForeignKey("dbo.Messages", t => t.IdMessage, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.IdUser, cascadeDelete: false)
                .Index(t => t.IdUser)
                .Index(t => t.IdMessage);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "IdUser", "dbo.Users");
            DropForeignKey("dbo.Replies", "IdUser", "dbo.Users");
            DropForeignKey("dbo.Replies", "IdMessage", "dbo.Messages");
            DropForeignKey("dbo.Commentaires", "IdUser", "dbo.Users");
            DropForeignKey("dbo.Commentaires", "IdRessource", "dbo.Ressources");
            DropForeignKey("dbo.Ressources", "IdCategorie", "dbo.Categories");
            DropIndex("dbo.Replies", new[] { "IdMessage" });
            DropIndex("dbo.Replies", new[] { "IdUser" });
            DropIndex("dbo.Messages", new[] { "IdUser" });
            DropIndex("dbo.Users", "Index_Email");
            DropIndex("dbo.Commentaires", new[] { "IdRessource" });
            DropIndex("dbo.Commentaires", new[] { "IdUser" });
            DropIndex("dbo.Ressources", new[] { "IdCategorie" });
            DropTable("dbo.Replies");
            DropTable("dbo.Messages");
            DropTable("dbo.Users");
            DropTable("dbo.Commentaires");
            DropTable("dbo.Ressources");
            DropTable("dbo.Categories");
        }
    }
}
