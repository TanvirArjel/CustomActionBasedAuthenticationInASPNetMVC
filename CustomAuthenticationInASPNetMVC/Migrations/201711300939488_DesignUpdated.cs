namespace CustomAuthenticationInASPNetMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DesignUpdated : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Action", newName: "ControllerActions");
            DropForeignKey("dbo.RoleActionCategory", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.RoleActionCategory", "ActionCategoryId", "dbo.ActionCategories");
            DropForeignKey("dbo.UserActionCategory", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserActionCategory", "ActionCategoryId", "dbo.ActionCategories");
            DropIndex("dbo.RoleActionCategory", new[] { "RoleId" });
            DropIndex("dbo.RoleActionCategory", new[] { "ActionCategoryId" });
            DropIndex("dbo.UserActionCategory", new[] { "UserId" });
            DropIndex("dbo.UserActionCategory", new[] { "ActionCategoryId" });
            DropTable("dbo.RoleActionCategory");
            DropTable("dbo.UserActionCategory");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserActionCategory",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        ActionCategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.ActionCategoryId });
            
            CreateTable(
                "dbo.RoleActionCategory",
                c => new
                    {
                        RoleId = c.Int(nullable: false),
                        ActionCategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoleId, t.ActionCategoryId });
            
            CreateIndex("dbo.UserActionCategory", "ActionCategoryId");
            CreateIndex("dbo.UserActionCategory", "UserId");
            CreateIndex("dbo.RoleActionCategory", "ActionCategoryId");
            CreateIndex("dbo.RoleActionCategory", "RoleId");
            AddForeignKey("dbo.UserActionCategory", "ActionCategoryId", "dbo.ActionCategories", "ActionCategoryId", cascadeDelete: true);
            AddForeignKey("dbo.UserActionCategory", "UserId", "dbo.Users", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.RoleActionCategory", "ActionCategoryId", "dbo.ActionCategories", "ActionCategoryId", cascadeDelete: true);
            AddForeignKey("dbo.RoleActionCategory", "RoleId", "dbo.Roles", "RoleId", cascadeDelete: true);
            RenameTable(name: "dbo.ControllerActions", newName: "Action");
        }
    }
}
