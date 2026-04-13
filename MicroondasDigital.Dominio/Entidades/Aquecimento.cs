using System;

namespace MicroondasDigital.Dominio.Entidades
{
    public class Aquecimento
    {
        public int Tempo { get; }
        public int Potencia { get; }

        public Aquecimento(int tempoSegundos, int potencia)
        {
            if (potencia < 1 || potencia > 10)
                throw new ArgumentException("A potência deve estar entre 1 e 10.");

            Tempo = tempoSegundos;
            Potencia = potencia;    

        }
    }
}
