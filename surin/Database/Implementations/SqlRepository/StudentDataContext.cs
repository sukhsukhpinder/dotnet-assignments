using Database.Contracts;
using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database.Implementations.SqlRepository
{
    public class StudentDataContext : IStudentContract
    {
        private readonly RegistrationDBContext _context;

        public StudentDataContext(RegistrationDBContext context)
        {
            _context = context;
        }
        public async Task<bool> DeleteEnrollmentById(int id)
        {
            var existingRecord = await _context.Students.FindAsync(id);
            if (existingRecord != null)
            {
                _context.Remove(existingRecord);
                return _context.SaveChanges() > 0;
            }
            return false;

        }

        public async Task<bool> DeleteEnrollmentByNumber(string enrollmentNo)
        {
            var existingRecord = await _context.Students.FirstOrDefaultAsync(x => x.EnrollmentNo == enrollmentNo);
            if (existingRecord != null)
            {
                _context.Remove(existingRecord);
                return _context.SaveChanges() > 0;
            }
            return false;
        }

        public async Task<Student> EnrollStudent(Student entity)
        {
            entity.EnrollmentNo = GenerateEnrollmentNumber();
            entity.isActive = entity.isActive || true;
            await _context.Students.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<IEnumerable<Student>> GetAll()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student> GetByEnrollmentNo(string enrollmentNo)
        {
            var record = await _context.Students.FirstOrDefaultAsync(x => x.EnrollmentNo == enrollmentNo);
            return record!;
        }

        public async Task<Student> GetById(int id)
        {
            return await _context.Students.FindAsync(id);
        }

        public async Task<Student> UpdateEnrollment(Student entity, int id)
        {
            var existingEntity = await _context.Students.FindAsync(id);
            if (existingEntity != null)
            {
                entity.StudentId = existingEntity.StudentId;
                entity.EnrollmentNo = existingEntity.EnrollmentNo;
                existingEntity.FirstName = entity.FirstName;
                existingEntity.LastName = entity.LastName;
                existingEntity.DateOfBirth = entity.DateOfBirth;
                existingEntity.Email = entity.Email;
                existingEntity.StateId = entity.StateId;
                existingEntity.CourseId = entity.CourseId;
                existingEntity.isActive = entity.isActive;
                _context.Entry(existingEntity).State = EntityState.Detached;
                _context.Entry(entity).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return entity;
            }
            return null!;
        }


        public async Task<IEnumerable<(string? StateName, double Percentage)>> GetStateStudentPercentage()
        {
            var statePercentages = await _context.States
                .Select(state => new
                {
                    StateName = state.Name,
                    Percentage = (double)state.Students.Count() / _context.Students.Count() * 100
                })
                .ToListAsync();

            return statePercentages.Select(result => (
                result.StateName,
                result.Percentage
            ));
        }

        public async Task<double> GetSuccessfulJoinPercentage()
        {
            int successfulJoinCount = await _context.Students.CountAsync(s => s.isActive);

            int totalStudents = await _context.Students.CountAsync();

            if (totalStudents == 0)
            {
                return 0;
            }

            double successfulJoinPercentage = (double)successfulJoinCount / totalStudents * 100;

            return successfulJoinPercentage;

        }

        private string GenerateEnrollmentNumber()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
        }
    }
}
