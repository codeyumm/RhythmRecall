namespace RhythmRecall.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TrackLIst : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TrackLists",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        TrackId = c.Int(nullable: false),
                        ListenLater = c.Int(nullable: false),
                        Discovered = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Tracks", t => t.TrackId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.TrackId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TrackLists", "UserId", "dbo.Users");
            DropForeignKey("dbo.TrackLists", "TrackId", "dbo.Tracks");
            DropIndex("dbo.TrackLists", new[] { "TrackId" });
            DropIndex("dbo.TrackLists", new[] { "UserId" });
            DropTable("dbo.Users");
            DropTable("dbo.TrackLists");
        }
    }
}
