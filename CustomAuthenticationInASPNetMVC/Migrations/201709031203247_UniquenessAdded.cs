namespace CustomAuthenticationInASPNetMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UniquenessAdded : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ActionCategories", "ActionCategoryName", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Roles", "RoleName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Users", "UserName", c => c.String(nullable: false, maxLength: 50));
            CreateIndex("dbo.ActionCategories", "ActionCategoryName", unique: true, name: "Ix_ActionCategoryNameUnique");
            CreateIndex("dbo.Roles", "RoleName", unique: true, name: "Ix_RoleNameUnique");
            CreateIndex("dbo.Users", "UserName", unique: true, name: "Ix_UserNameUnique");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Users", "Ix_UserNameUnique");
            DropIndex("dbo.Roles", "Ix_RoleNameUnique");
            DropIndex("dbo.ActionCategories", "Ix_ActionCategoryNameUnique");
            AlterColumn("dbo.Users", "UserName", c => c.String(nullable: false));
            AlterColumn("dbo.Roles", "RoleName", c => c.String(nullable: false));
            AlterColumn("dbo.ActionCategories", "ActionCategoryName", c => c.String(nullable: false));
        }
    }
}
