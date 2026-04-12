using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroondasDigital.Aplicacao.DTO
{
    public class ProgramaSelecionadoDto
    {
        public string Nome { get; set; }
        public int Tempo { get; set; }
        public int Potencia { get; set; }
        public string Caractere { get; set; }
        public string Instrucoes { get; set; }
    }
}
