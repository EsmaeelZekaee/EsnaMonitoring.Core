
namespace EsnaMonitoring.Services.AutoMapper
{
    using System;

    using global::AutoMapper;
    using global::AutoMapper.Configuration;

    using EsnaData.Entities;
    using EsnaMonitoring.Services.Models;

    public class EsnaMapperProfile : Profile
    {
        public EsnaMapperProfile()
        {
            CreateMap<Configuration, ConfigurationModel>();
            CreateMap<ConfigurationModel, Configuration>()
                .ForMember(x => x.Id, expression => expression.Ignore())
                .ForMember(x => x.CreatedOnUtc, expression => expression.Ignore());
        }
    }
}
