using System.Globalization;
using AutoMapper;
using ProEventos.Application.Dtos;
using ProEventos.Domain;

namespace ProEventos.Application.Helpers;

public class ProEventosProfile : Profile
{
    public ProEventosProfile()
    {
        CreateMap<Evento, EventoDto>()
            .ForMember(eventoDto => eventoDto.DataEvento, opt =>
                opt.MapFrom(evento => evento.DataEvento.Value.ToString(CultureInfo.InvariantCulture)))
            .ReverseMap();
        CreateMap<Lote, LoteDto>().ReverseMap();;
        CreateMap<RedeSocial, RedeSocialDto>().ReverseMap();
        CreateMap<Palestrante, PalestranteDto>().ReverseMap();
    }
}
