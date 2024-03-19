using AutoMapper;
using RegistarationApp.Core.Models.Common;
using RegistarationApp.Core.Models.Course;
using RegistartionApp.Core.Domain.Entities;
using RegistartionApp.Core.Domain.Repositories;
using RegistrationApp.Core.Services.Interface;

namespace RegistrationApp.Core.Services
{
    /// <summary>
    /// Cources Releted Services
    /// </summary>
    public class CourseService : ICourseService
    {
        /// <summary>
        /// Used for the course repository details
        /// </summary>
        private readonly IRepository<Course> _courseRepository;

        /// <summary>
        /// Used for map entity to DTO or DTO to entity
        /// </summary>
        private readonly IMapper _mapper;

        public CourseService(IRepository<Course> courseRepository, IMapper mapper)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Delete Course 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Delete(string id)
        {
            var existingCourse = await _courseRepository.GetById(id);
            if (existingCourse == null)
            {
                return false;
            }
            return await _courseRepository.Delete(id);
        }

        /// <summary>
        /// Get Course by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<CourseModel?> GetById(string id)
        {
            return _mapper.Map<CourseModel>(await _courseRepository.GetById(id));
        }

        /// <summary>
        /// Get Course By name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<CourseModel?> GetByName(string name)
        {
            CourseModel? course = null;
            var courses = await _courseRepository.GetAll();
            if (courses != null)
            {
                course = _mapper.Map<CourseModel>(courses?.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)));
            }
            return course;
        }

        /// <summary>
        /// Get All courses
        /// </summary>
        /// <returns></returns>
        public async Task<List<CourseModel>> GetAll()
        {
            var courses = await _courseRepository.GetAll();
            return _mapper.Map<List<CourseModel>>(courses);
        }

        /// <summary>
        /// Create Course
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<CourseModel?> Create(CreateUpdateCourseModel model)
        {
            var courseExist = await GetByName(model.Name);
            if (courseExist != null)
            {
                throw new ArgumentException(Message.CourseAlreadyExist);
            }
            var course = _mapper.Map<Course>(model);

            await _courseRepository.Add(course);
            return _mapper.Map<CourseModel>(course);
        }

        /// <summary>
        /// Update Course
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<CourseModel?> Update(string id, CreateUpdateCourseModel model)
        {
            CourseModel? course = null;
            var courseExist = await _courseRepository.GetById(id);
            if (courseExist != null)
            {
                courseExist.Name = model.Name;
                courseExist.Cost = model.Cost;
                courseExist.Description = model.Description;
                courseExist.DurationInMonths = model.DurationInMonths;

                course= _mapper.Map<CourseModel>(await _courseRepository.Update(courseExist));
            }
            return course;
        }
    }
}
