namespace MicroondasDigital.Api.DTO
{
    public class CriarProgramaRequest
    {
        public string Nome { get; set; }
        public string Alimento { get; set; }
        public int Tempo { get; set; }
        public int Potencia { get; set; }
        public string Caractere { get; set; }
        public string Instrucoes { get; set; }  
    }
}