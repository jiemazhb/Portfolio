namespace BlogSystem.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newestVersion : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "NickName", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "NickName", c => c.String(maxLength: 50, unicode: false));
        }
    }
}
