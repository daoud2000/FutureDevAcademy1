using Application.Dto.CoursesDto;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Repository.Interface
{
    public interface ICourseService
    {
        Task CreateCourse(CreateCourseDto input);
        Task DeleteCourse(int id);
        Task UpdateCourse(int id, UpdateCourseDto input);
        Task<List<CourseDetailsDto>> GetCourses(CourseFilterDto input);
        Task<CourseDetailsDto> GetCourseById(int id);
        Task EnrollCourse(EnrollCourseDto input);
        Task<List<CourseEnrollmentDto>> GetCourseEnrollment(int id);
        Task<int> GetTotalEnrollments();

    }
}
