using System;

namespace APIGEO.Entities
{
    public class Operacion
    {
        public int Id { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string Estado { get; set; }
        public int OperacionId {get; set;}
    }
}