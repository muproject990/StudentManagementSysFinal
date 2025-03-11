using AutoMapper;
using StudentManagement.App.DTOs;
using StudentManagement.Domain.Entities;

namespace StudentManagement.App.Automapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateStudentDto, Student>();
            CreateMap<CreateGradeDto, Grade>();
            CreateMap<CreateCourseDto, Course>();
            CreateMap<CreateEnrollmentDto, Enrollment>();
        }
    }
}