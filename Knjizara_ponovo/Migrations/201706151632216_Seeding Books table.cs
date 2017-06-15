namespace Knjizara_ponovo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedingBookstable : DbMigration
    {
        public override void Up()
        {
            Sql(
                @"
INSERT INTO [dbo].[Books] ([Name], [Price], [GenreId], [isDeleted]) VALUES (N'Harry Potter', 666, 3, 0);
INSERT INTO [dbo].[Books] ([Name], [Price], [GenreId], [isDeleted]) VALUES (N'Game of Thrones', 523, 1, 0);
INSERT INTO [dbo].[Books] ([Name], [Price], [GenreId], [isDeleted]) VALUES (N'Isijavanje', 740, 2, 0);
"
                );
        }
        
        public override void Down()
        {
        }
    }
}
