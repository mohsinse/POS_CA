using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.Model;
using POS.DTO;

namespace POS.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Define the mapping from UserDTO to User
            CreateMap<UserDTO, User>()
                .ForMember(dest => dest.Password, opt => opt.Ignore()); // Ignore Password for security reasons

            // Define the mapping from User to UserDTO
            CreateMap<User, UserDTO>();
        }
    }
}
