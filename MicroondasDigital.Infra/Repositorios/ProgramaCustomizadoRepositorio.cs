using MicroondasDigital.Dominio.Entidades;
using MicroondasDigital.Dominio.Interfaces.Repositorios;
using MicroondasDigital.Infra.Data;
using System.Collections.Generic;
using System.Linq;

namespace MicroondasDigital.Infra.Repositorios
{
    public class ProgramaCustomizadoRepositorio : IProgramaCustomizadoRepositorio
    {
        private readonly MicroondasContext _context;

        public ProgramaCustomizadoRepositorio(MicroondasContext context)
        {
            _context = context;
        }

        public bool ExisteCaractere(string caractere)
        {
            return _context.ProgramasCustomizados.Any(p => p.Caractere == caractere);
        }

        public void Inserir(ProgramaCustomizado programa)
        {
            _context.ProgramasCustomizados.Add(programa);
            _context.SaveChanges();
        }

        public IEnumerable<ProgramaCustomizado> ObterTodos()
        {
            return _context.ProgramasCustomizados.ToList();
        }
    }
}
