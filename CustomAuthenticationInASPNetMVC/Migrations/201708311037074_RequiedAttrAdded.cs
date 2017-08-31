namespace CustomAuthenticationInASPNetMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequiedAttrAdded : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ActionCategories", "ActionCategoryName", c => c.String(nullable: false));
            AlterColumn("dbo.ActionCategories", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.Action", "ActionName", c => c.String(nullable: false));
            AlterColumn("dbo.Action", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.Roles", "RoleName", c => c.String(nullable: false));
            AlterColumn("dbo.Roles", "Description", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Roles", "Description", c => c.String());
            AlterColumn("dbo.Roles", "RoleName", c => c.String());
            AlterColumn("dbo.Action", "Description", c => c.String());
            AlterColumn("dbo.Action", "ActionName", c => c.String());
            AlterColumn("dbo.ActionCategories", "Description", c => c.String());
            AlterColumn("dbo.ActionCategories", "ActionCategoryName", c => c.String());
        }
    }
}
