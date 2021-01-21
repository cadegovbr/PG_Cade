namespace PGD.Infra.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdicionandoColunaCodigoTabelaUnidade : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Unidade", "Codigo", c => c.String(nullable: false, maxLength: 100, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Unidade", "Codigo");
        }
    }
}
