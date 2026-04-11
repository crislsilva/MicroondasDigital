using NUnit.Framework;
using MicroondasDigital.Aplicacao.Services;

namespace MicroondasDigital.Testes.Services
{
    public class MicroondasDigitalServiceTestes
    {
        private AquecimentoService _service;

        [SetUp]
        public void Setup()
        {
            _service = new AquecimentoService();
        }

        [Test]
        public void IniciarAquecimento_DeveConfigurarTempoEPotencia()
        {
            _service.Iniciar(60, 5);
            Assert.AreEqual("1:00", _service.ObterTempoFormatado());
        }

        [Test]
        public void IniciarAquecimento_SemParametros_DeveUsarValoresPadrao()
        {
            _service.Iniciar(null, null);

            var tempo = _service.ObterTempoFormatado();
            Assert.AreEqual("30", tempo);
        }
        [Test]
        public void IniciarAquecimento_QuandoEmAquecimento_DeveAdicionar30Segundos()
        {
            _service.Iniciar(30, 5);
            _service.Iniciar(30, 5);

            var tempo = _service.ObterTempoFormatado();

            Assert.AreEqual("1:00", tempo);
        }

        [Test]
        public void PausarOuCancelarAquecimento_QuandoEmAquecimento_DevePausar()
        {
            _service.Iniciar(10, 5);
            _service.PausarOuCancelar();

            var antes = _service.TimerTick();
            var depois = _service.TimerTick();

            Assert.AreEqual(antes, depois);
        }

        [Test]
        public void PausarOuCancelarAquecimento_QuandoPausado_DeveCancelar()
        {
            _service.Iniciar(10, 5);
            _service.PausarOuCancelar();
            _service.PausarOuCancelar();
            var tempo = _service.ObterTempoFormatado();
            Assert.AreEqual("0", tempo);
        }

        [Test]
        public void ContinuarAquecimento_QuandoPausado_DeveContinuar()
        {
            _service.Iniciar(10, 5);
            _service.PausarOuCancelar();
            _service.Continuar();
            var antes = _service.TimerTick();
            var depois = _service.TimerTick();
            Assert.AreNotEqual(antes, depois);
        }
        [Test]
        public void TimerTick_QuandoTempoChegarAZero_DeveFinalizarAquecimento()
        {
            _service.Iniciar(1, 2);
            _service.TimerTick();

            var resultado = _service.TimerTick();

            Assert.IsTrue(resultado.Contains("Aquecimento concluído"));
        }

    }
}
