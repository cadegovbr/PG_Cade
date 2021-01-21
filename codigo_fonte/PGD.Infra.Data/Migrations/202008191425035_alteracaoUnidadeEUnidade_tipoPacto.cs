namespace PGD.Infra.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alteracaoUnidadeEUnidade_tipoPacto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Unidade", "Hierarquia", c => c.String(maxLength: 50, unicode: false));
            CreateIndex("dbo.Unidade_TipoPacto", "IdUnidade");
            AddForeignKey("dbo.Unidade_TipoPacto", "IdUnidade", "dbo.Unidade", "IdUnidade");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Unidade_TipoPacto", "IdUnidade", "dbo.Unidade");
            DropIndex("dbo.Unidade_TipoPacto", new[] { "IdUnidade" });
            DropColumn("dbo.Unidade", "Hierarquia");
        }
    }
}
