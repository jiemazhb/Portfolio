namespace BlogSystem.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DecemberByDanny : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "NickName", c => c.String(maxLength: 50, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "NickName");
        }
    }
}
