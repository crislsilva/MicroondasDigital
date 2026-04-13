using MicroondasDigital.Aplicacao.Interfaces;
using MicroondasDigital.Dominio.Entidades;
using MicroondasDigital.Dominio.Interfaces.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroondasDigital.Aplicacao.Services
{
    public class ProgramaAquecimentoService : IProgramaAquecimentoService
    {
        private readonly IEnumerable<IProgramaAquecimento> _programas;
        
        public ProgramaAquecimentoService(IEnumerable<IProgramaAquecimento> programas)
        {
            _programas = programas;
        }

        public ProgramaAquecimento ObterPorNome(string nome)
        {
            var programa = _programas
                .Select(p => p.ObterPrograma())
                .FirstOrDefault(p => p.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase));

            return programa;
        }

        public List<ProgramaAquecimento> ObterTodos()
        {
            return _programas.Select(p => p.ObterPrograma()).ToList();
        }
    }
}
