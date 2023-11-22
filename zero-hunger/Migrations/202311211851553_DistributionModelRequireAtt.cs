namespace zero_hunger.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DistributionModelRequireAtt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Distributions", "Area", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Distributions", "Area", c => c.String());
        }
    }
}
