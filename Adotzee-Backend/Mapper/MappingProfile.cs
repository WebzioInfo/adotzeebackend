using Adotzee_Backend.DTOs;
using Adotzee_Backend.DTOs.AddonDTOs;
using Adotzee_Backend.DTOs.CollegeDTOs;
using Adotzee_Backend.DTOs.CourseDTOs;
using Adotzee_Backend.Helpers;
using Adotzee_Backend.Models;
using AutoMapper;

namespace Adotzee_Backend.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Course, CourseResponseDTO>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
                .ForMember(dest => dest.Stream, opt => opt.MapFrom(src => src.Stream.ToString()));

            CreateMap<CourseCreateDTO, Course>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => Enum.Parse<CourseType>(src.Type)))
                .ForMember(dest => dest.Stream, opt => opt.MapFrom(src => Enum.Parse<StreamType>(src.Stream)));

            CreateMap<CourseUpdateDTO, Course>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => Enum.Parse<CourseType>(src.Type)))
                .ForMember(dest => dest.Stream, opt => opt.MapFrom(src => Enum.Parse<StreamType>(src.Stream)));


            CreateMap<AddonCourseCreateDTO, AddonCourse>()
                .ForMember(dest => dest.AddonColleges, opt => opt.MapFrom(src =>
                    src.CollegeIds.Select(id => new AddonCollege { CollegeId = id })));

            CreateMap<AddonCourseUpdateDTO, AddonCourse>()
                .ForMember(dest => dest.AddonColleges, opt => opt.Ignore());

            CreateMap<AddonCourse, AddonCourseResponseDTO>()
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.Name))
                .ForMember(dest => dest.CollegeNames, opt => opt.MapFrom(src => src.AddonColleges.Select(ac => ac.College.Name)));




            CreateMap<College, CollegeResponseDTO>()
                    .ForMember(dest => dest.Addons, opt => opt.MapFrom(src =>
                        src.AddonColleges.Select(ac => ac.AddonCourse.Name).ToList()
                    ));

                CreateMap<CollegeCreateDTO, College>().ReverseMap();
                CreateMap<CollegeUpdateDTO, College>().ReverseMap();

        }
    }

}
