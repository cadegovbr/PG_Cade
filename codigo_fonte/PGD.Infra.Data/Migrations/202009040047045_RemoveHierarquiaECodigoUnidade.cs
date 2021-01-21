namespace PGD.Infra.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveHierarquiaECodigoUnidade : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Unidade", "Codigo");
            DropColumn("dbo.Unidade", "Hierarquia");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Unidade", "Hierarquia", c => c.String(maxLength: 50, unicode: false));
            AddColumn("dbo.Unidade", "Codigo", c => c.String(nullable: false, maxLength: 100, unicode: false));
        }
    }
}
