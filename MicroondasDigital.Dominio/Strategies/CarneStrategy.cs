using MicroondasDigital.Dominio.Entidades;
using MicroondasDigital.Dominio.Interfaces.Entidades;

namespace MicroondasDigital.Dominio.Strategies
{
    public class CarneStrategy : IProgramaAquecimento
    {
        public ProgramaAquecimento ObterPrograma()
        {
            return new ProgramaAquecimento(
                nome: "Carnes de boi",
                alimento: "Carne em pedaços ou fatias",
                tempo: 840,
                potencia: 4,
                caractere: "@",
                instrucoes: "Interrompa o processo na metade e vire o conteúdo com a parte de baixo " +
                            "para cima para o descongelamento uniforme."
            );
        }
    }
}
