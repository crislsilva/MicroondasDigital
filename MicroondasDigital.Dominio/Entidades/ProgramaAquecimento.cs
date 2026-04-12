namespace MicroondasDigital.Dominio.Entidades
{
    public class ProgramaAquecimento
    {
        public string Nome { get; private set; }
        public string Alimento { get; private set; }
        public int Tempo { get; private set; }
        public int Potencia { get; private set; }
        public string CaractereAquecimento { get; private set; }
        public string Instrucoes { get; private set; }

        public ProgramaAquecimento(
            string nome, 
            string alimento, 
            int tempo, 
            int potencia, 
            string caractere, 
            string instrucoes)
        {
            Nome = nome;
            Alimento = alimento;
            Tempo = tempo;
            Potencia = potencia;
            CaractereAquecimento = caractere;
            Instrucoes = instrucoes;
        }

    }
}
