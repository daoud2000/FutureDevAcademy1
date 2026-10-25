using Application.Dto.CoursesDto;
using Application.Repository.Interface;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace Application.Repository.Service
{

    public class CourseService : ICourseService
    {
        private readonly IGenericRepository<Course> _courseRepository;
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IGenericRepository<Enrollment> _enrollmentRepository;
        private readonly IGenericRepository<User> _userRepo;
        public CourseService(IGenericRepository<Course> courseRepository, IGenericRepository<Category> categoryRepository, IGenericRepository<Enrollment> enrollmentRepository, IGenericRepository<User> userRepo)
        {


            _courseRepository = courseRepository;
            _categoryRepository = categoryRepository;
            _enrollmentRepository = enrollmentRepository;
            _userRepo = userRepo;



        }
        public async Task CreateCourse(CreateCourseDto input)
        {
            var cateogry = _categoryRepository.GetAll().FirstOrDefault(x => x.Id == input.CategoryId);
            if (cateogry == null)
            {
                throw new Exception("category not found");


            }



            var course = _courseRepository.GetAll().FirstOrDefault(x => x.Ttile == input.Ttile);
            if (course != null)
            { throw new Exception("course already exist "); }

            if (input.StartDate > input.EndDate)

            { throw new Exception("start date cant be after end date"); }
            if (input.Price < 0)
            {
                throw new Exception("price cant be less than zero");
            }


            var newcourse = new Course()
            {
                Ttile = input.Ttile,
                Price = input.Price,
                Describtion = input.Describtion,
                StartDate = input.StartDate,
                EndDate = input.EndDate,
                CategoryId = input.CategoryId



            };



            await _courseRepository.Insert(newcourse);
            await _courseRepository.SaveChanges();











        }


        public async Task DeleteCourse(int id)
        {
            var course = await _courseRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (course == null)
            {

                throw new Exception("Course Does not exist");


            }
            var enroll = await _enrollmentRepository.GetAll().FirstOrDefaultAsync(x => x.CourseId == course.Id);

            if (enroll != null)
            {

                throw new Exception("cant delete, it contains students");
            }

            await _courseRepository.Delete(course);
            await _courseRepository.SaveChanges();
        }

        public async Task EnrollCourse(EnrollCourseDto input)
        {
            var user = await _userRepo.GetById(input.UserId);
            var course = await _courseRepository.GetById(input.CourseId);
            var check = await _enrollmentRepository.GetAll().FirstOrDefaultAsync(x => x.CourseId == input.CourseId && x.StudentId == input.UserId);
            if (check != null)
            {
                throw new Exception("student already enrolled on this course");
            }
            var enrollment = new Enrollment()
            {
                CourseId = course.Id,
                StudentId = user.Id,
                EnrollmentDate= DateTime.UtcNow,
            };
            await _enrollmentRepository.Insert(enrollment);
            await _enrollmentRepository.SaveChanges();

        }

        public async Task<CourseDetailsDto> GetCourseById(int id)
        {
            var course = await _courseRepository.GetAll().Include(x => x.category).FirstOrDefaultAsync(x => x.Id == id);
            if (course == null)
            {
                throw new Exception("course not found");
            }
            var response = new CourseDetailsDto()
            {
                category = course.category.Name,
                Describtion = course.Describtion,
                Id = course.Id,
                StartDate = course.StartDate,
                EndDate = course.EndDate,
                Price = course.Price,
                Ttile = course.Ttile,


            };
            return response;
        }
        public async Task<List<CourseEnrollmentDto>> GetCourseEnrollment(int id)
        {
            var course = await _courseRepository.GetById(id);
            var enrollment = await _enrollmentRepository.GetAll().Include(x=>x.student).ThenInclude(x=>x.userId)
                .Include(x => x.course).ThenInclude(x =>x.category)
                .Where(x => x.CourseId == id).Select(x=> new CourseEnrollmentDto()
                {
                    StudentId = x.StudentId,
                    FullName =x.student.userId.FullName,
                    Enrollmentdate = x.EnrollmentDate,
                    
                    
                    
                }).ToListAsync();
            if (enrollment == null)
            {
                throw new Exception("No enrollments Found");
            }

            return enrollment;
        }

        public async Task<List<CourseDetailsDto>> GetCourses(CourseFilterDto input)
        {
            var courses = await _courseRepository.GetAll().Where(x => !string.IsNullOrEmpty(input.Ttile) ? x.Ttile.Contains(input.Ttile.ToLower().Trim()) : true
                && !string.IsNullOrEmpty(input.Category) ? x.category.Name == input.Category.Trim().ToLower() : true).Select(x => new CourseDetailsDto()
                {
                    category = x.category.Name,
                    Describtion = x.Describtion,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Id = x.Id,
                    Price = x.Price,
                    Ttile = x.Ttile,


                }).ToListAsync();
            var enrollmentCounts = await _enrollmentRepository
    .GetAll()
    .GroupBy(e => e.CourseId)
    .Select(g => new
    {
        CourseId = g.Key,
        Count = g.Count()
    })
    .ToDictionaryAsync(x => x.CourseId, x => x.Count);

            foreach (var course in courses)
            {
                course.TotalEnrollments =
                    enrollmentCounts.TryGetValue(course.Id, out var count)
                        ? count
                        : 0;
            }

           

            return courses;
        }
        public async Task<int> GetTotalEnrollments()
        {
            return await _enrollmentRepository
                .GetAll()
                .CountAsync();
        }

        public async Task UpdateCourse(int id, UpdateCourseDto input)
        {
            var course = await _courseRepository.GetAll().Include(x => x.category).FirstOrDefaultAsync(x => x.Id == id);
            if (course == null) { throw new Exception("course not found"); }

            if (course.StartDate < DateTime.UtcNow) { throw new Exception("Cant Update, has been started"); }
            ;

            if (input.StartDate > input.EndDate)
            {
                throw new Exception("cant put start date after end date");
            }
            if(input.StartDate< DateTime.UtcNow) { throw new Exception(" Cant put dates in the past"); }
            if (input.Price <= 0)
            {
                throw new Exception("cant put price equal or less than zero");

            }
            if (!course.category.Name.Contains(input.Category))
            { throw new Exception("this Category is not exist"); }

                {
                    course.StartDate = input.StartDate;
                    course.EndDate = input.EndDate;
                    course.Describtion = input.Describtion;
                    course.Price = input.Price;
                    course.Id = id;
                    course.category.Name = input.Category;
                    course.Ttile = input.Ttile;


                    _courseRepository.Update(course);
                    _courseRepository.SaveChanges();
                }
            }
        }
    }

    
