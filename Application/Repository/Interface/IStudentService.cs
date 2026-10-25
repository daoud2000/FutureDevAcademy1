using Application.Dto.CoursesDto;
using Application.Dto.UserDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Repository.Interface
{
    public interface IStudentService
    {
        Task UserRegistration(UserRegistrationDto input);
        Task <List<CourseDetailsDto>> GetMyCourses();
        Task<List<CourseDetailsDto>> GetActiveCourses(CourseFilterDto input);
        Task UpdateMyInfo(UpdateUserDetails input);

    }
}
