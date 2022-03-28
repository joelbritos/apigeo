using System;
using APIGEO.Domain;
using APIGEO.Entities;
using AutoMapper;

namespace APIGEO.Business.MappingProfiles
{
    public class OperacionProfile: Profile
    {
        public OperacionProfile()
        {
            CreateMap<OperacionDto, Operacion>();
            CreateMap<Operacion, OperacionDto>();
        }
    }
}