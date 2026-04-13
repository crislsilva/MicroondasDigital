namespace MicroondasDigital.Infra.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inicial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProgramasCustomizados",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 50),
                        Alimento = c.String(nullable: false, maxLength: 100),
                        Tempo = c.Int(nullable: false),
                        Potencia = c.Int(nullable: false),
                        Caractere = c.String(nullable: false, maxLength: 1),
                        Instrucoes = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ProgramasCustomizados");
        }
    }
}
