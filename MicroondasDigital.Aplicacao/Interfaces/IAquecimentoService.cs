using MicroondasDigital.Aplicacao.DTO;

namespace MicroondasDigital.Aplicacao.Interfaces
{
    public interface IAquecimentoService
    {
        void Iniciar(int? tempoSegundos, int? potencia);
        string PausarOuCancelar();
        void Continuar();
        string TimerTick();
        string ObterTempoFormatado();
        ProgramaSelecionadoDto SelecionarPrograma(string nomePrograma);
    }
}
