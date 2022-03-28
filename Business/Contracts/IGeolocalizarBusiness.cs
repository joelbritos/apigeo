using System;
using APIGEO.Domain;

namespace APIGEO.Business.Contracts
{
    public interface IGeolocalizarBusiness
    {
        int Localizar(PedidoDto pedido);

        void GuardarProcesado(string payload);

        OperacionDto VerificarGeo(int id);
    }
}