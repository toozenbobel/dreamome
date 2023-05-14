using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using WeatherAlerts.Entities;
using WeatherAlerts.Models;

namespace WeatherAlerts;

[ExcludeFromCodeCoverage]
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<MeteoWarning, MeteoWarningEntity>()
            .ReverseMap();
    }
}