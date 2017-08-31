namespace CustomAuthenticationInASPNetMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SecurityChanges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RoleAction", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.RoleAction", "ActionId", "dbo.Action");
            DropForeignKey("dbo.UserAction", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserAction", "ActionId", "dbo.Action");
            DropForeignKey("dbo.Action", "SectionId", "dbo.Sections");
            DropIndex("dbo.Action", new[] { "SectionId" });
            CreateTable(
                "dbo.ActionCategories",
                c => new
                    {
                        ActionCategoryId = c.Int(nullable: false, identity: true),
                        ActionCategoryyName = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ActionCategoryId);


            DropTable("dbo.Action");
            CreateTable(
                "dbo.Action",
                c => new
                    {
                        ActionId = c.Int(nullable: false, identity: true),
                        ActionCategoryId = c.Int(nullable: false),
                        ActionName = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ActionId)
                .ForeignKey("dbo.ActionCategories", t => t.ActionCategoryId, cascadeDelete: true)
                .Index(t => t.ActionCategoryId);
            
            CreateTable(
                "dbo.RoleActionCategory",
                c => new
                    {
                        RoleId = c.Int(nullable: false),
                        ActionCategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoleId, t.ActionCategoryId })
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.ActionCategories", t => t.ActionCategoryId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.ActionCategoryId);

            DropTable("dbo.RoleAction");
            CreateTable(
                "dbo.RoleAction",
                c => new
                    {
                        RoleId = c.Int(nullable: false),
                        ActionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoleId, t.ActionId })
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.Action", t => t.ActionId, cascadeDelete: true);
            
            CreateTable(
                "dbo.UserActionCategory",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        ActionCategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.ActionCategoryId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.ActionCategories", t => t.ActionCategoryId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ActionCategoryId);

            
            DropTable("dbo.UserAction");
            CreateTable(
                "dbo.UserAction",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        ActionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.ActionId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Action", t => t.ActionId, cascadeDelete: true);
            
            
            DropTable("dbo.Sections");
            
            
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserAction",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        ActionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.ActionId });
            
            CreateTable(
                "dbo.RoleAction",
                c => new
                    {
                        RoleId = c.Int(nullable: false),
                        ActionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoleId, t.ActionId });
            
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
                "dbo.Action",
                c => new
                    {
                        ActionId = c.Int(nullable: false, identity: true),
                        SectionId = c.Int(nullable: false),
                        ActionName = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ActionId);
            
            DropForeignKey("dbo.UserAction", "ActionId", "dbo.Action");
            DropForeignKey("dbo.UserAction", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserActionCategory", "ActionCategoryId", "dbo.ActionCategories");
            DropForeignKey("dbo.UserActionCategory", "UserId", "dbo.Users");
            DropForeignKey("dbo.RoleAction", "ActionId", "dbo.Action");
            DropForeignKey("dbo.RoleAction", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.RoleActionCategory", "ActionCategoryId", "dbo.ActionCategories");
            DropForeignKey("dbo.RoleActionCategory", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Action", "ActionCategoryId", "dbo.ActionCategories");
            DropIndex("dbo.UserActionCategory", new[] { "ActionCategoryId" });
            DropIndex("dbo.UserActionCategory", new[] { "UserId" });
            DropIndex("dbo.RoleActionCategory", new[] { "ActionCategoryId" });
            DropIndex("dbo.RoleActionCategory", new[] { "RoleId" });
            DropIndex("dbo.Action", new[] { "ActionCategoryId" });
            DropTable("dbo.UserAction");
            DropTable("dbo.UserActionCategory");
            DropTable("dbo.RoleAction");
            DropTable("dbo.RoleActionCategory");
            DropTable("dbo.Action");
            DropTable("dbo.ActionCategories");
            CreateIndex("dbo.Action", "SectionId");
            AddForeignKey("dbo.Action", "SectionId", "dbo.Sections", "SectionId", cascadeDelete: true);
            AddForeignKey("dbo.UserAction", "ActionId", "dbo.Action", "ActionId", cascadeDelete: true);
            AddForeignKey("dbo.UserAction", "UserId", "dbo.Users", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.RoleAction", "ActionId", "dbo.Action", "ActionId", cascadeDelete: true);
            AddForeignKey("dbo.RoleAction", "RoleId", "dbo.Roles", "RoleId", cascadeDelete: true);
        }
    }
}
