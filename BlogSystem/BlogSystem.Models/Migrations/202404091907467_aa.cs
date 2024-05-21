namespace BlogSystem.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aa : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "PassWord", c => c.String(nullable: false, maxLength: 300, unicode: false));
            AlterColumn("dbo.Users", "ImagePath", c => c.String(nullable: false, maxLength: 300, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "ImagePath", c => c.String(nullable: false, maxLength: 500, unicode: false));
            AlterColumn("dbo.Users", "PassWord", c => c.String(nullable: false, maxLength: 30, unicode: false));
        }
    }
}
