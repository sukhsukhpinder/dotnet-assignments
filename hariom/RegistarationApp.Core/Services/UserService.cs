using AutoMapper;
using RegistarationApp.Core.Models.Common;
using RegistarationApp.Core.Models.User;
using RegistartionApp.Core.Domain.Entities;
using RegistartionApp.Core.Domain.Repositories;
using RegistrationApp.Core.Services.Interface;

namespace RegistrationApp.Core.Services
{
    public class UserService : IUserService
    {
        /// <summary>
        /// Used for the User repository details
        /// </summary>
        private readonly IRepository<User> _userRepository;

        /// <summary>
        /// Used for the map objects
        /// </summary>
        private readonly IMapper _mapper;

        public UserService(IRepository<User> userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Delete User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Delete(string id)
        {
            var existingRegistration = await _userRepository.GetById(id);
            if (existingRegistration == null)
            {
                return false;
            }
            return await _userRepository.Delete(id);
        }

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserInfoModel?> GetById(string id)
        {
            return _mapper.Map<UserInfoModel>(await _userRepository.GetById(id));
        }

        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<UserInfoModel?> GetByEmail(string email)
        {
            var students = await _userRepository.GetAll();
            if (students != null)
            {
                return _mapper.Map<UserInfoModel>(students.FirstOrDefault(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase)));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns></returns>
        public async Task<List<UserInfoModel>> GetAll()
        {
            var registrations = await _userRepository.GetAll();
            return _mapper.Map<List<UserInfoModel>>(registrations);//.Where(x => x.Role != Roles.admin.ToString()).ToList()
        }

        /// <summary>
        /// Create New user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<UserModel?> Create(CreateUpdateUserModel model)
        {
            var studentExist = await GetByEmail(model.Email);
            if (studentExist != null)
            {
                throw new ArgumentException(Message.UserAlreadyExist);
            }
            var user = _mapper.Map<User>(model);
            user.Role = Roles.student.ToString();

            await _userRepository.Add(user);
            return _mapper.Map<UserModel>(user);
        }

        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<UserModel?> Update(string id, CreateUpdateUserModel model)
        {
            var existingRegistration = await _userRepository.GetById(id);
            if (existingRegistration == null)
            {
                return null;
            }
            existingRegistration.Name = model.Name;
            existingRegistration.Email = model?.Email;
            existingRegistration.Password = model?.Password;

            return _mapper.Map<UserModel>(await _userRepository.Update(existingRegistration));
        }


    }
}
