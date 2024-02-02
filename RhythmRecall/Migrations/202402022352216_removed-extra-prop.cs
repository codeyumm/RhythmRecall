namespace RhythmRecall.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedextraprop : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Reviews", "MyProperty");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reviews", "MyProperty", c => c.Int(nullable: false));
        }
    }
}
