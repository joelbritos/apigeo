using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIGEO.Business.Contracts;
using APIGEO.Domain;
// using APIGEO.Business.Contracts;
// using APIGEO.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace APIGEO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeolocalizarController : ControllerBase
    {
        private readonly ILogger<GeolocalizarController> _logger;
        private readonly IGeolocalizarBusiness _geolocalizar;

        public GeolocalizarController(ILogger<GeolocalizarController> logger, IGeolocalizarBusiness geolocalizar)
        {
            _logger = logger;
            _geolocalizar = geolocalizar;
        }

        [HttpPost("geolocalizar")]
        public async Task<ActionResult<OperacionDto>> Post([FromBody] PedidoDto pedido)
        {
            if (pedido == null)
            {
                return NoContent();
            }

            try
            {
                ResponseDto response = new ResponseDto();
                var localizarTask = Task.Run(() => {
                    response.Id = _geolocalizar.Localizar(pedido);
                });

                await Task.WhenAll(localizarTask);
                return Accepted(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("geocodoficar/{id}")]
        public async Task<ActionResult<OperacionDto>> Get(int id)
        {
            try
            {
                OperacionDto res = new OperacionDto();
                var traerDatosTask = Task.Run(() => {
                    res = _geolocalizar.VerificarGeo(id);
                });

                await Task.WhenAll(traerDatosTask);

                if(res == null)
                {
                    return NotFound();
                }

                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }


    }
}
