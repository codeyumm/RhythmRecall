namespace RhythmRecall.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedTrackTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tracks", "AlbumArt", c => c.String());
            AlterColumn("dbo.Tracks", "ReleaseDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tracks", "ReleaseDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Tracks", "AlbumArt");
        }
    }
}
