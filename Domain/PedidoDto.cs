using System;

namespace APIGEO.Domain
{
    public class PedidoDto
    {
        public string Calle { get; set; }
        public int Numero { get; set; }
        public string Ciudad { get; set; }
        public int CodigoPostal { get; set; }
        public string Provincia { get; set; }
        public string Pais { get; set; }
    }
}