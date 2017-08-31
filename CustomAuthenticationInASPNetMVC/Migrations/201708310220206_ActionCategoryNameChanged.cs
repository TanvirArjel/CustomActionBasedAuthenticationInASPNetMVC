namespace CustomAuthenticationInASPNetMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActionCategoryNameChanged : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ActionCategories", "ActionCategoryName", c => c.String());
            DropColumn("dbo.ActionCategories", "ActionCategoryyName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ActionCategories", "ActionCategoryyName", c => c.String());
            DropColumn("dbo.ActionCategories", "ActionCategoryName");
        }
    }
}
