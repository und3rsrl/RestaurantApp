namespace RestaurantApp.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Categories : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CategoriesModels",
                c => new
                    {
                        Identifier = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Identifier);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CategoriesModels");
        }
    }
}
