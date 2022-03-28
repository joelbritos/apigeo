using System;

namespace APIGEO.Entities
{
    public class Pedido
    {
        public int Id { get; set; }
        public string Calle { get; set; }
        public int Numero { get; set; }
        public string Ciudad { get; set; }
        public int CodigoPostal { get; set; }
        public string Provincia { get; set; }
        public string Pais { get; set; }
        public int OperacionId { get; set; }
    }
}