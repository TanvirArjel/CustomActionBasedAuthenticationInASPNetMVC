namespace CustomActionBasedAuthenticationInASPNetMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sectionActionAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Action",
                c => new
                    {
                        ActionId = c.Int(nullable: false, identity: true),
                        SectionId = c.Int(nullable: false),
                        ActionName = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ActionId)
                .ForeignKey("dbo.Sections", t => t.SectionId, cascadeDelete: true)
                .Index(t => t.SectionId);
            
            CreateTable(
                "dbo.Sections",
                c => new
                    {
                        SectionId = c.Int(nullable: false, identity: true),
                        SectionName = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.SectionId);
            
            CreateTable(
                "dbo.RoleAction",
                c => new
                    {
                        RoleId = c.Int(nullable: false),
                        ActionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoleId, t.ActionId })
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.Action", t => t.ActionId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.ActionId);
            
            CreateTable(
                "dbo.UserAction",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        ActionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.ActionId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Action", t => t.ActionId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ActionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Action", "SectionId", "dbo.Sections");
            DropForeignKey("dbo.UserAction", "ActionId", "dbo.Action");
            DropForeignKey("dbo.UserAction", "UserId", "dbo.Users");
            DropForeignKey("dbo.RoleAction", "ActionId", "dbo.Action");
            DropForeignKey("dbo.RoleAction", "RoleId", "dbo.Roles");
            DropIndex("dbo.UserAction", new[] { "ActionId" });
            DropIndex("dbo.UserAction", new[] { "UserId" });
            DropIndex("dbo.RoleAction", new[] { "ActionId" });
            DropIndex("dbo.RoleAction", new[] { "RoleId" });
            DropIndex("dbo.Action", new[] { "SectionId" });
            DropTable("dbo.UserAction");
            DropTable("dbo.RoleAction");
            DropTable("dbo.Sections");
            DropTable("dbo.Action");
        }
    }
}
