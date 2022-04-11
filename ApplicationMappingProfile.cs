using AutoMapper;
using LMS.Data;
using LMS.Dtos;

namespace LMS
{
    public class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile()
        {

            CreateMap<CreateCourseDto, Course>();
            CreateMap<User, UserDto>();
                
        }
    }
}
