using System;
using System.Threading;
using System.Threading.Tasks;
using APIGEO.Business;
using APIGEO.Business.Contracts;
using APIGEO.Repository;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace APIGEO.Services
{
    public class ConsumerService : BackgroundService
    {
        private readonly ConsumerConfig _config;
        private readonly IServiceScopeFactory _factory;
        private ILogger<ConsumerService> _logger;
        public ConsumerService(ConsumerConfig config, IServiceScopeFactory factory, ILogger<ConsumerService> logger)
        {
            _config = config;
            _factory = factory;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken cancelToken)
        {
            Task.Run(() => Start(cancelToken));
            return Task.CompletedTask;
        }

        private void Start(CancellationToken cancelToken)
        {
            try
            {
                CancellationTokenSource cTokenSource = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) =>
                {
                    e.Cancel = true;
                    cTokenSource.Cancel();
                };

                using (var consumer = new ConsumerBuilder<Ignore, string>(_config).Build())
                {
                    consumer.Subscribe("enProceso");
                    consumer.Subscribe("Procesados");

                    while (!cancelToken.IsCancellationRequested)
                    {
                        var cr = consumer.Consume(cTokenSource.Token);
                        if (cr.Topic == "Procesados" || cr.Topic == "enProceso")
                        {
                            using (var scope = _factory.CreateScope())
                            {
                                _logger.LogInformation("Procesando payload");
                                var service = scope.ServiceProvider.GetService<IGeolocalizarBusiness>();
                                service.GuardarProcesado(cr.Message.Value);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Mensaje de error: " + ex.Message);
            }
        }
    }
}