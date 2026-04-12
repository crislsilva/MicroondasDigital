using MicroondasDigital.Dominio.Entidades;
using MicroondasDigital.Dominio.Interfaces;

namespace MicroondasDigital.Dominio.Strategies
{
    public class LeiteStrategy : IProgramaAquecimento
    {
        public ProgramaAquecimento ObterPrograma()
        {
            // Implementação do método para retornar o programa de leite
            return new ProgramaAquecimento(
                nome: "Leite",
                alimento: "Leite",
                tempo: 300,
                potencia: 5,
                caractere: "#",
                instrucoes: "Cuidado com aquecimento de líquidos, o choque térmico aliado ao movimento do recipiente " +
                            "pode causar fervura imediata causando risco de queimaduras."
            );
        }
    }
}
