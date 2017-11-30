namespace CustomActionBasedAuthenticationInASPNetMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserPasswordRestAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserPasswordResets",
                c => new
                    {
                        PasswordResetId = c.Long(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        PasswordResetToken = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        IsReset = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PasswordResetId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserPasswordResets");
        }
    }
}
