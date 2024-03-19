using AutoMapper;
using sf_dotenet_mod.Application.Dtos.Request;
using sf_dotenet_mod.Application.Dtos.Response;
using sf_dotenet_mod.Application.Services.Base;
using sf_dotenet_mod.Domain.Entities;
using sf_dotenet_mod.Domain.Repositories;

namespace sf_dotenet_mod.Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
        private readonly IRepositoryFactory _repositoryFactory;

        public StudentService(IRepositoryFactory repositoryFactory, IMapper mapper)
        {
            _repositoryFactory = repositoryFactory;
            _mapper = mapper;
            _studentRepository = _repositoryFactory.GetStudentRepository();
        }
        public async Task<Response<bool>> Delete(string id)
        {
            var result = await _studentRepository.Delete(id);
            var response = new Response<bool>
            {
                IsSuccessful = true,
                Result = result
            };
            return response;
        }

        public Task<Response<bool>> DeleteEnrollmentByNumber(string enrollmentNo)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<StudentResponse>> Create(StudentRequest request)
        {
            var entity = _mapper.Map<Student>(request);
            var student = await _studentRepository.Create(entity);
            var result = _mapper.Map<StudentResponse>(student);

            var response = new Response<StudentResponse>
            {
                IsSuccessful = true,
                Result = result
            };

            return response;
        }

        public async Task<Response<IEnumerable<StudentResponse>>> GetAll()
        {
            var details = await _studentRepository.GetAll();
            var result = _mapper.Map<IEnumerable<StudentResponse>>(details);

            var response = new Response<IEnumerable<StudentResponse>>
            {
                IsSuccessful = true,
                Result = result
            };

            return response;
        }

        public Task<Response<StudentResponse>> GetByEnrollmentNo(string enrollmentNo)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<StudentResponse>> Get(string id)
        {
            var student = await _studentRepository.Get(id);
            var result = _mapper.Map<StudentResponse>(student);

            var response = new Response<StudentResponse>
            {
                IsSuccessful = true,
                Result = result
            };

            return response;
        }

        public async Task<Response<StudentResponse>> Update(StudentRequest request, string studentId)
        {
            var entity = _mapper.Map<Student>(request);
            var student = await _studentRepository.Update(entity, studentId);
            var result = _mapper.Map<StudentResponse>(student);

            var response = new Response<StudentResponse>
            {
                IsSuccessful = true,
                Result = result
            };

            return response;
        }

        public async Task<Response<List<KeyValuePair<string, int>>>> GetChartDetails()
        {
            var result = await _studentRepository.GetChartDetails();

            var response = new Response<List<KeyValuePair<string, int>>>
            {
                IsSuccessful = true,
                Result = result
            };

            return response;
        }
    }
}

