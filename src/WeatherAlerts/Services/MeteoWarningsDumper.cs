using AutoMapper;
using MediatR;
using WeatherAlerts.Commands;
using WeatherAlerts.Entities;
using WeatherAlerts.Models;

namespace WeatherAlerts.Services;

internal class MeteoWarningsDumper : IMeteoWarningsDumper
{
    private readonly IMeteoWarningsService _service;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public MeteoWarningsDumper(IMeteoWarningsService service,
        IMapper mapper,
        IMediator mediator)
    {
        _service = service;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task LoadAndDump()
    {
        var result =  await _service.GetMeteoWarnings();

        var allItems = new List<MeteoWarning>();
        allItems.AddRange(result.Upcoming);
        allItems.AddRange(result.In12Hours);
        allItems.AddRange(result.NextDay);

        var dbItems = _mapper.Map<List<MeteoWarningEntity>>(allItems);
                
        await _mediator.Send(new DumpMeteoWarningsCommand(dbItems));
    }
}