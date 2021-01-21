namespace PGD.Infra.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modelagemUsuarioPerfil : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RecursosHumanos", "IdUnidade", "dbo.Unidade");
            DropIndex("dbo.RecursosHumanos", new[] { "IdUnidade" });
            CreateTable(
                "dbo.UsuarioPerfilUnidade",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        IdUsuario = c.Int(nullable: false),
                        IdPerfil = c.Int(nullable: false),
                        IdUnidade = c.Int(nullable: false),
                        Excluido = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Perfil", t => t.IdPerfil)
                .ForeignKey("dbo.Unidade", t => t.IdUnidade)
                .ForeignKey("dbo.Usuario", t => t.IdUsuario)
                .Index(t => t.IdUsuario)
                .Index(t => t.IdPerfil)
                .Index(t => t.IdUnidade);
            
            CreateTable(
                "dbo.Perfil",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Unidade", "Sigla", c => c.String(maxLength: 25, unicode: false));
            AddColumn("dbo.Unidade", "Excluido", c => c.Boolean());
            AddColumn("dbo.Usuario", "DataNascimento", c => c.DateTime());
            AlterColumn("dbo.Unidade", "Nome", c => c.String(nullable: false, maxLength: 200, unicode: false));
            AlterColumn("dbo.Unidade", "Codigo", c => c.String(nullable: false, maxLength: 100, unicode: false));
            AlterColumn("dbo.Usuario", "Matricula", c => c.String(nullable: false, maxLength: 100, unicode: false));
            AlterColumn("dbo.Usuario", "Cpf", c => c.String(nullable: false, maxLength: 11, fixedLength: true, unicode: false));
            AlterColumn("dbo.Usuario", "Nome", c => c.String(nullable: false, maxLength: 250, unicode: false));
            AlterColumn("dbo.Usuario", "Email", c => c.String(nullable: false, maxLength: 100, unicode: false));
            AlterColumn("dbo.Usuario", "Inativo", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Usuario", "Cpf", name: "IDX_CPF");
            DropColumn("dbo.Usuario", "Unidade");
            DropColumn("dbo.Usuario", "NomeUnidade");
            DropColumn("dbo.Usuario", "Administrador");
            DropTable("dbo.RecursosHumanos");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.RecursosHumanos",
                c => new
                    {
                        IdRecursosHumanos = c.Int(nullable: false, identity: true),
                        IdUnidade = c.Int(),
                        IdPerfil = c.Int(),
                        Perfil = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdRecursosHumanos);
            
            AddColumn("dbo.Usuario", "Administrador", c => c.Boolean());
            AddColumn("dbo.Usuario", "NomeUnidade", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.Usuario", "Unidade", c => c.Int());
            DropForeignKey("dbo.UsuarioPerfilUnidade", "IdUsuario", "dbo.Usuario");
            DropForeignKey("dbo.UsuarioPerfilUnidade", "IdUnidade", "dbo.Unidade");
            DropForeignKey("dbo.UsuarioPerfilUnidade", "IdPerfil", "dbo.Perfil");
            DropIndex("dbo.Usuario", "IDX_CPF");
            DropIndex("dbo.UsuarioPerfilUnidade", new[] { "IdUnidade" });
            DropIndex("dbo.UsuarioPerfilUnidade", new[] { "IdPerfil" });
            DropIndex("dbo.UsuarioPerfilUnidade", new[] { "IdUsuario" });
            AlterColumn("dbo.Usuario", "Inativo", c => c.Boolean());
            AlterColumn("dbo.Usuario", "Email", c => c.String(maxLength: 100, unicode: false));
            AlterColumn("dbo.Usuario", "Nome", c => c.String(maxLength: 100, unicode: false));
            AlterColumn("dbo.Usuario", "Cpf", c => c.String(maxLength: 100, unicode: false));
            AlterColumn("dbo.Usuario", "Matricula", c => c.String(maxLength: 100, unicode: false));
            AlterColumn("dbo.Unidade", "Codigo", c => c.String(maxLength: 100, unicode: false));
            AlterColumn("dbo.Unidade", "Nome", c => c.String(maxLength: 100, unicode: false));
            DropColumn("dbo.Usuario", "DataNascimento");
            DropColumn("dbo.Unidade", "Excluido");
            DropColumn("dbo.Unidade", "Sigla");
            DropTable("dbo.Perfil");
            DropTable("dbo.UsuarioPerfilUnidade");
            CreateIndex("dbo.RecursosHumanos", "IdUnidade");
            AddForeignKey("dbo.RecursosHumanos", "IdUnidade", "dbo.Unidade", "IdUnidade");
        }
    }
}
