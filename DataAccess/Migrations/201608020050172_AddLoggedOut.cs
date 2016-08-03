namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLoggedOut : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "LoggedOut", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "LoggedOut");
        }
    }
}
