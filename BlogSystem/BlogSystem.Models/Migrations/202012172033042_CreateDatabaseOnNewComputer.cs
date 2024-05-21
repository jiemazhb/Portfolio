namespace BlogSystem.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDatabaseOnNewComputer : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Articles", "CategoryId", "dbo.BlogCategories");
            DropIndex("dbo.Articles", new[] { "CategoryId" });
            DropColumn("dbo.Articles", "CategoryId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Articles", "CategoryId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Articles", "CategoryId");
            AddForeignKey("dbo.Articles", "CategoryId", "dbo.BlogCategories", "Id");
        }
    }
}
