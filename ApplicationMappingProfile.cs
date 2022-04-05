using AutoMapper;
using LMS.Data;
using LMS.Dtos;

namespace LMS
{
    public class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile()
        {

            CreateMap<Course, CreateCourseDto>()
                .ForMember(dest => dest.Author,
                c => c.MapFrom(src => src.Author.FirstName + " " + src.Author.LastName));
        }
    }
}
