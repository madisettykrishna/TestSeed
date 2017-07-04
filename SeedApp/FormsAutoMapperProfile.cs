using System;
using AutoMapper;

namespace SeedApp.Forms
{
	public class FormsAutoMapperProfile : Profile
	{
		public FormsAutoMapperProfile()
		{
		}

		public void Configure()
		{
			// Mapping goes here, something like this.
			//CreateMap<MenuItemDto, MenuItem>()
			//            .ForMember(dest => dest.ServerCreatedOnUtc, opt => opt.MapFrom(src => src.CreatedOnUtc))
			//            .ForMember(dest => dest.ServerModifiedOnUtc, opt => opt.MapFrom(src => src.ModifiedOnUtc))
			//            .ReverseMap();
		}
	}
}
