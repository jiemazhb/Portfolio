namespace BlogSystem.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateArticleTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "LikeUserInfo", c => c.String());
            AddColumn("dbo.Articles", "DislikeUserInfo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Articles", "DislikeUserInfo");
            DropColumn("dbo.Articles", "LikeUserInfo");
        }
    }
}
