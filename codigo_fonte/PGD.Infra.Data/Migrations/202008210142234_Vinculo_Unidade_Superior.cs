namespace PGD.Infra.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Vinculo_Unidade_Superior : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Unidade", "IdUnidadeSuperior", c => c.Int());
            CreateIndex("dbo.OS_GrupoAtividade", "IdGrupoAtividadeOriginal");
            CreateIndex("dbo.Unidade", "IdUnidadeSuperior");
            AddForeignKey("dbo.OS_GrupoAtividade", "IdGrupoAtividadeOriginal", "dbo.GrupoAtividade", "IdGrupoAtividade");
            AddForeignKey("dbo.Unidade", "IdUnidadeSuperior", "dbo.Unidade", "IdUnidade");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Unidade", "IdUnidadeSuperior", "dbo.Unidade");
            DropForeignKey("dbo.OS_GrupoAtividade", "IdGrupoAtividadeOriginal", "dbo.GrupoAtividade");
            DropIndex("dbo.Unidade", new[] { "IdUnidadeSuperior" });
            DropIndex("dbo.OS_GrupoAtividade", new[] { "IdGrupoAtividadeOriginal" });
            DropColumn("dbo.Unidade", "IdUnidadeSuperior");
        }
    }
}
