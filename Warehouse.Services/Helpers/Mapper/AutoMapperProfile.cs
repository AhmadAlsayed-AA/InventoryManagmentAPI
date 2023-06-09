﻿using System;
using AutoMapper;
using Warehouse.Data.UserModels;
using Warehouse.Data.ProductModels;

namespace Warehouse.Services.Helpers.Mapper
{
    public class AutoMapperProfile : Profile
    {
		public AutoMapperProfile()
		{
            CreateMap<RegisterRequest, User>();

            CreateMap<UpdateRequest, User>()
    .ForMember(dest => dest.Name, opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.Name)))
    .ForMember(dest => dest.Email, opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.Email)))
    .ForMember(dest => dest.PhoneNumber, opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.PhoneNumber)))
    .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<UserResponse, User>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    prop != null
                ));
            CreateMap<User, UserResponse>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    prop != null
                ));
            CreateMap<User, UpdateRequest>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    prop != null

                ));
            CreateMap<Product, Product>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    {
                        // ignore null & empty string properties
                        if (prop == null) return false;
                        if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                        return true;
                    }
                ));
        }
        
	}
}

