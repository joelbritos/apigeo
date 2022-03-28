using System;
using APIGEO.Business.Contracts;
using APIGEO.Domain;
using NUnit.Framework;

namespace APIGEO.Business.Tests
{
    [TestFixture]
    public class GeolocalizarTest
    {
        private readonly IGeolocalizarBusiness _geolocalizar;
        public GeolocalizarTest(IGeolocalizarBusiness geolocalizar)
        {
            _geolocalizar = geolocalizar;
        }
        [Test]
        public void CanBeProcessed_GeolocalizarB()
        {
            //Arrange
            var direccion = new PedidoDto();
            //Act
            var result = _geolocalizar.Localizar(direccion);
            //Assert
            Assert.AreSame(new OperacionDto(), result);
        }
    }
}