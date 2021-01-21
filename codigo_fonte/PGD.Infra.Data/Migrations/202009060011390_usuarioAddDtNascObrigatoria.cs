namespace PGD.Infra.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class usuarioAddDtNascObrigatoria : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Usuario", "DataNascimento", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Usuario", "DataNascimento", c => c.DateTime());
        }
    }
}
