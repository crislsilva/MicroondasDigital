using MicroondasDigital.Dominio.Entidades;
using MicroondasDigital.Dominio.Interfaces;

namespace MicroondasDigital.Dominio.Strategies
{
    public class FrangoStrategy : IProgramaAquecimento
    {
        public ProgramaAquecimento ObterPrograma()
        {
            return new ProgramaAquecimento(
                nome: "Frango",
                alimento: "Frango (qualquer corte)",
                tempo: 480,
                potencia: 7,
                caractere: "%",
                instrucoes: "Interrompa o processo na metade e vire o conteúdo com a parte de baixo " +
                            "para cima para o descongelamento uniforme."
            );
        }
    }
}
