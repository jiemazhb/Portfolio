namespace BlogSystem.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatecommenttable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "like", c => c.Int(nullable: false));
            AddColumn("dbo.Comments", "dislike", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comments", "dislike");
            DropColumn("dbo.Comments", "like");
        }
    }
}
