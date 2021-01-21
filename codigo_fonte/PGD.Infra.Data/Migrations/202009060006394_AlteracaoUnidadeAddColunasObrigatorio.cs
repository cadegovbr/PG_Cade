namespace PGD.Infra.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlteracaoUnidadeAddColunasObrigatorio : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Unidade", "Sigla", c => c.String(nullable: false, maxLength: 25, unicode: false));
            AlterColumn("dbo.Unidade", "Excluido", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Unidade", "Excluido", c => c.Boolean());
            AlterColumn("dbo.Unidade", "Sigla", c => c.String(maxLength: 25, unicode: false));
        }
    }
}
