namespace BlogSystem.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatecommenttableagain : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "likes", c => c.Int(nullable: false));
            AddColumn("dbo.Comments", "dislikes", c => c.Int(nullable: false));
            DropColumn("dbo.Comments", "like");
            DropColumn("dbo.Comments", "dislike");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Comments", "dislike", c => c.Int(nullable: false));
            AddColumn("dbo.Comments", "like", c => c.Int(nullable: false));
            DropColumn("dbo.Comments", "dislikes");
            DropColumn("dbo.Comments", "likes");
        }
    }
}
