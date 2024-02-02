namespace RhythmRecall.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reviewentity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TrackId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Title = c.String(),
                        Content = c.String(),
                        MyProperty = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tracks", t => t.TrackId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.TrackId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reviews", "UserId", "dbo.Users");
            DropForeignKey("dbo.Reviews", "TrackId", "dbo.Tracks");
            DropIndex("dbo.Reviews", new[] { "UserId" });
            DropIndex("dbo.Reviews", new[] { "TrackId" });
            DropTable("dbo.Reviews");
        }
    }
}
