namespace Knjizara_ponovo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedingGenretable : DbMigration
    {
        public override void Up()
        {
            Sql(
                @"
INSERT INTO [dbo].[Genres] ([Name]) VALUES (N'Nauka')
INSERT INTO [dbo].[Genres] ([Name]) VALUES (N'Horor')
INSERT INTO [dbo].[Genres] ([Name]) VALUES (N'Komedija')
"
                );
        }
        
        public override void Down()
        {
        }
    }
}
