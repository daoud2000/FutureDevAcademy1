using Application.Dto.CoursesDto;
using Application.Dto.UserDto;
using Application.Repository.Interface;
using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace Application.Repository.Service
{
    public  class StudentService : IStudentService
    {
        private readonly IGenericRepository<User> _userRepo;
        private readonly IGenericRepository<Role> _roleRepo;
        private readonly IGenericRepository<Student> _studentrepo;
        private readonly IGenericRepository<Course> _courseRepo;

        private readonly IHttpContextAccessor _http;
        public StudentService(IGenericRepository<User> userRepo, IGenericRepository<Role> roleRepo, IGenericRepository<Student> studentrepo, IHttpContextAccessor http, IGenericRepository<Course> courseRepo) {
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _studentrepo = studentrepo;
            _http = http;
            _courseRepo = courseRepo;

        
        
        
        }
        public async Task UserRegistration(UserRegistrationDto input)
        {
            var user = await _userRepo.GetAll().Include(x=>x.role)
                .FirstOrDefaultAsync(x => x.Email == input.Email);
            if (user != null) { throw new Exception("User Already Exists"); }
            {
                
                bool passwordValidate = Regex.IsMatch(input.Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");
                if (!passwordValidate)
                {
                    throw new Exception("Passowrd is weaks");
                }
                string emailPattern = @"^[a-zA-Z0-9._%+\-]+@[a-zA-Z0-9.\-]+\.[A-Za-z]{2,}$";
                bool emailValidate = Regex.IsMatch(input.Email, emailPattern);
                if (!emailValidate)
                {
                    throw new Exception("Email is not valid");
                }

                string mobilePattern =  @"^(?:\+?962|00962)?0?7[7-9]\d{7}$";
                bool mobileValidate = Regex.IsMatch(input.PhoneNumber, mobilePattern);
                if (!mobileValidate)
                {
                    throw new Exception("mobile is not valid");
                }
                var studentrole = await _roleRepo.GetAll().FirstOrDefaultAsync(r => r.Code == RoleEnum.Student);
                var newuser = new User()
                {
                    FullName = input.FullName,
                    Email = input.Email,
                    PhoneNumber = input.PhoneNumber,
                    role = studentrole,





                };
                var hashpassword = new PasswordHasher<User>();
                var password = hashpassword.HashPassword(newuser, input.Password);
                newuser.Password = password;

                await _userRepo.Insert(newuser);
                await _userRepo.SaveChanges();
                var student = new Student()
                {
                    UserId = newuser.Id,
                    UniversityName = input.UniversityName,






                };
                await _studentrepo.Insert(student);
                await _studentrepo.SaveChanges();

                

            }
            
            
        }
        public async Task <List<CourseDetailsDto>> GetMyCourses()
        {
            var claim =  _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var claimid = Convert.ToInt32(claim);
            var courses = await _courseRepo.GetAll().Select(x => new CourseDetailsDto()
            {
                Id = x.Id,
                category = x.category.Name,
                Describtion = x.Describtion,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Price = x.Price,
                Ttile= x.Ttile,



            }).ToListAsync();
            return courses;

        }
        public async Task UpdateMyInfo(UpdateUserDetails input)
        {
            var claim = _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var claimid = Convert.ToInt32(claim);

            var userinfo = await _userRepo.GetById(claimid);

            userinfo.PhoneNumber = input.Phone;
            userinfo.FullName = input.FullName;
            userinfo.Email = input.Email;

             _userRepo.Update(userinfo);
            await _userRepo.SaveChanges();
            var studentinfo = await _studentrepo.GetAll().FirstOrDefaultAsync(x=>x.UserId==claimid);
            studentinfo.Birthdate = input.Birthdate;
            studentinfo.UniversityName = input.UniversityName;

            _studentrepo.Update(studentinfo);
            await _studentrepo.SaveChanges();

            
        }



              
        public async Task<List<CourseDetailsDto>> GetActiveCourses(CourseFilterDto input)
        {
            var courses = await _courseRepo.GetAll().Where(x => !string.IsNullOrEmpty(input.Ttile) ? x.Ttile.Contains(input.Ttile.ToLower().Trim()) : true
                && !string.IsNullOrEmpty(input.Category) ? x.category.Name == input.Category.Trim().ToLower() : true && x.StartDate>=DateTime.UtcNow).Select(x => new CourseDetailsDto()
                {
                    category = x.category.Name,
                    Describtion = x.Describtion,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Id = x.Id,
                    Price = x.Price,
                    Ttile = x.Ttile,


                }).ToListAsync();

            return courses;
        }
    }
}
