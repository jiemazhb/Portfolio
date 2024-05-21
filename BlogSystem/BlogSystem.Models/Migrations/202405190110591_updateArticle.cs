namespace BlogSystem.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateArticle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "category", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Articles", "category");
        }
    }
}
