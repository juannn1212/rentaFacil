using AutoMapper;
using BookingService.Domain.Entities;
using BookingService.Application.DTOs;

namespace BookingService.Application.Mappings
{
    public class ReportProfile : Profile
    {
        public ReportProfile()
        {
            CreateMap<DailyReport, ReportDto>();
        }
    }
}