using MicroondasDigital.Aplicacao.Services;
using MicroondasDigital.Dominio.Entidades;
using MicroondasDigital.Dominio.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroondasDigital.Testes.Services
{
    [TestFixture]
    public class ProgramaAquecimentoServiceTests
    {
        private Mock<IProgramaAquecimento> _programaMock;
        private ProgramaAquecimento _programaAquecimento;
        private ProgramaAquecimentoService _service;

        [SetUp]
        public void SetUp()
        {
            _programaMock = new Mock<IProgramaAquecimento>();
            _programaAquecimento = new ProgramaAquecimento(
                "Alimento 1",
                "Alimento 1",
                300,
                10,
                null,
                null
            );
            _programaMock.Setup(p => p.ObterPrograma()).Returns(_programaAquecimento);

            var programas = new List<IProgramaAquecimento> { _programaMock.Object };
            _service = new ProgramaAquecimentoService(programas);
        }

        [Test]
        public void ObterPorNome_ComNomeValido_RetornaProgramaAquecimento()
        {
            // Arrange
            var nome = "Alimento 1";

            // Act
            var resultado = _service.ObterPorNome(nome);

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual(nome, resultado.Nome);
            Assert.AreEqual(300, resultado.Tempo);
            Assert.AreEqual(10, resultado.Potencia);
        }

        [Test]
        public void ObterPorNome_ComNomeInvalido_LancaException()
        {
            // Arrange
            var nomeInvalido = "Programa Inexistente";

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => _service.ObterPorNome(nomeInvalido));
            Assert.That(exception.Message, Does.Contain($"Programa {nomeInvalido} não encontrado"));
        }

        [Test]
        public void ObterTodos_RetornaTodosProgramas()
        {
            // Arrange
            var programaMock2 = new Mock<IProgramaAquecimento>();
            var programa2 = new ProgramaAquecimento(
                "Alimento 2",
                "Alimento 2",
                600,
                8,
                null,
                null
            );
            programaMock2.Setup(p => p.ObterPrograma()).Returns(programa2);

            var programas = new List<IProgramaAquecimento> { _programaMock.Object, programaMock2.Object };
            var service = new ProgramaAquecimentoService(programas);

            // Act
            var resultado = service.ObterTodos();

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual(2, resultado.Count());
            Assert.That(resultado, Has.Some.Matches<ProgramaAquecimento>(p => p.Nome == "Alimento 1"));
            Assert.That(resultado, Has.Some.Matches<ProgramaAquecimento>(p => p.Nome == "Alimento 2"));
        }

        [Test]
        public void ObterTodos_SemProgramas_RetornaListaVazia()
        {
            // Arrange
            var programas = new List<IProgramaAquecimento>();
            var service = new ProgramaAquecimentoService(programas);

            // Act
            var resultado = service.ObterTodos();

            // Assert
            Assert.IsNotNull(resultado);
            Assert.IsEmpty(resultado);
        }
    }
}