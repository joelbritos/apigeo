using System;
using APIGEO.Domain;
using APIGEO.Entities;
using AutoMapper;

namespace APIGEO.Business.MappingProfiles
{
    public class PedidoProfile: Profile
    {
        public PedidoProfile()
        {
            CreateMap<PedidoDto, Pedido>();
            CreateMap<Pedido, PedidoDto>();
        }
    }
}