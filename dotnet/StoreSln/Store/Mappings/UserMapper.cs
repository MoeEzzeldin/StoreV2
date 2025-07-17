using System;
using System.Collections.Generic;
using Store.Models.DTOs;
using Store.Models.Entities;
using AutoMapper;

namespace Store.Mappings
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            // Entity to safe return model
            CreateMap<User, ReturnUser>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.UserRole));

            // Registration model to entity (for creating new users)
            CreateMap<RegisterUser, User>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => src.Role))
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // Set manually after hashing
                .ForMember(dest => dest.Salt, opt => opt.Ignore()) // Set manually
                .ForMember(dest => dest.Reviews, opt => opt.Ignore());

            // No mapping needed for LoginUser - it's just for input validation
            // No mapping needed for LoginResponse - it's constructed manually
        }
    }
}