using System.Text;
using MicroondasDigital.Aplicacao.Interfaces;
using MicroondasDigital.Dominio.Entidades;

namespace MicroondasDigital.Aplicacao.Services
{
    public class AquecimentoService : IAquecimentoService
    {
        private int _tempoRestante;
        private int _potencia;
        private bool _estaAquecendo;
        private bool _estaPausado;
        private readonly StringBuilder _status = new StringBuilder();

        #region Métodos Públicos
        public void Iniciar(int? tempoSegundos, int? potencia)
        {
            if (!_estaAquecendo && !_estaPausado)
            {
                Reset();
                var aquecimento = new Aquecimento(tempoSegundos ?? 30, potencia ?? 10);
                
                _tempoRestante = aquecimento.Tempo;
                _potencia = aquecimento.Potencia;
                _estaAquecendo = true;
                _estaPausado = false;
            }
            else
            {
                if (!_estaPausado)
                    _tempoRestante += 30;
            }
        }
        
        public void Continuar()
        {
            if (_estaPausado)
                _estaPausado = false;
        }

        public string ObterTempoFormatado()
        {
            int minutos = _tempoRestante / 60;
            int segundos = _tempoRestante % 60;

            return minutos > 0 ? $"{minutos}:{segundos:D2}" : segundos.ToString();
        }

        public string PausarOuCancelar()
        {
            if (_estaAquecendo && !_estaPausado)
            {
                _estaPausado = true;
                return "pausado";
            }
            else
            {
                Reset();
                return "cancelado";
            }
        }

        public string TimerTick()
        {
            if (!_estaAquecendo || _estaPausado)
                return _status.ToString();

            if (_tempoRestante <= 0)
            {
                _estaAquecendo = false;
                _status.Append("Aquecimento concluído");
                return _status.ToString();
            }

            for (int i = 0; i < _potencia; i++)
            {
                _status.Append(".");
            }

            _status.Append(" ");
            _tempoRestante--;

            return _status.ToString();
        }
        #endregion

        #region Métodos Privados
        private void Reset()
        {
            _tempoRestante = 0;
            _potencia = 0;
            _estaAquecendo = false;
            _estaPausado = false;
            _status.Clear();
        }
        #endregion
    }
}
