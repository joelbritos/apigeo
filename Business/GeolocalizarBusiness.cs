using System;
using System.Linq;
using APIGEO.Business.Contracts;
using APIGEO.Domain;
using APIGEO.Entities;
using APIGEO.Repository;
// using APIGEO.Repository;
using AutoMapper;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace APIGEO.Business
{
    public class GeolocalizarBusiness : IGeolocalizarBusiness
    {
        private readonly APIGEOContext _context;
        private readonly IMapper _mapper;
        private readonly ProducerConfig _config;

        public GeolocalizarBusiness(IMapper mapper, APIGEOContext context, ProducerConfig config)
        {
            _context = context;
            _mapper = mapper;
            _config = config;
        }

        public int Localizar(PedidoDto pedido)
        {
            Pedido entity = _mapper.Map<Pedido>(pedido);
            _context.Pedidos.Add(entity);
            _context.SaveChanges();

            // Publicar en KafkaQueue
            EmitirMensaje("procesar", entity);

            return entity.Id;
        }

        private void EmitirMensaje(string topic, Pedido payload)
        {
            var payloadReady = JsonConvert.SerializeObject(payload);

            using (var producer = new ProducerBuilder<Null, string>(_config).Build())
            {
                producer.Produce(topic, new Message<Null, string> { Value = payloadReady });
                producer.Flush();
            }
        }

        public void GuardarProcesado(string payload)
        {
            OperacionDto operacion = JsonConvert.DeserializeObject<OperacionDto>(payload);
            
            Operacion entity = new Operacion() 
            { 
                OperacionId = operacion.Id,
                Latitud = operacion.Latitud,
                Longitud = operacion.Longitud,
                Estado = operacion.Estado,
            };

            if (operacion == null)
            {
                throw new Exception("No se pudo convertir");
            }

            _context.Operacion.Add(entity);
            _context.SaveChanges();
        }

        public OperacionDto VerificarGeo(int id)
        {
            var entity = _context.Operacion.FirstOrDefault(x => x.Id == id);
            
            return _mapper.Map<OperacionDto>(entity);
        }
    }
}