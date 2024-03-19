using EnrollHub.Domain.Entities;
using EnrollHub.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EnrollHub.Infrastructure.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly EnrollDbContext _dbContext;
        public StudentRepository(EnrollDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> DeleteEnrollmentById(string id)
        {
            if (Guid.TryParse(id, out Guid studentId))
            {
                var existingRecord = await _dbContext.Students.FindAsync(studentId);
                if (existingRecord != null)
                {
                    _dbContext.Remove(existingRecord);
                    return _dbContext.SaveChanges() > 0;
                }
                return false;
            }
            else
            {
                throw new ArgumentException($"Enrollment not found");
            }
            
        }

        public async Task<bool> DeleteEnrollmentByNumber(string enrollmentNo)
        {
            var existingRecord = await _dbContext.Students.FirstOrDefaultAsync(x => x.EnrollmentNo == enrollmentNo);
            if (existingRecord != null)
            {
                _dbContext.Remove(existingRecord);
                return _dbContext.SaveChanges() > 0;
            }
            return false;
        }

        public async Task<Student> EnrollStudent(Student entity)
        {
            entity.EnrollmentNo = GenerateEnrollmentNumber();
            entity.CreatedOn = DateTime.UtcNow;
            entity.Active = true;
            await _dbContext.Students.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<Student>> GetAll()
        {
            return await _dbContext.Students.Include(s=>s.State).Include(c=>c.Course).ToListAsync();
        }

        public async Task<Student> GetByEnrollmentNo(string enrollmentNo)
        {
            var record = await _dbContext.Students.FirstOrDefaultAsync(x => x.EnrollmentNo == enrollmentNo);
            return record;
        }

        public async Task<Student> GetById(string id)
        {
            if (Guid.TryParse(id, out Guid studentId))
            {
                return await _dbContext.Students.FindAsync(studentId);
            }
            else
            {
                throw new ArgumentException($"User with ID {id} not found");
            }
        }

        public async Task<bool> UpdateEnrollment(Student entity, string id)
        {
            if(Guid.TryParse(id, out Guid studentId))
            {
                var existingEntity = await _dbContext.Students.FindAsync(studentId);
                if (existingEntity != null)
                {
                    entity.StudentId = existingEntity.StudentId;
                    entity.EnrollmentNo = existingEntity.EnrollmentNo;
                    entity.CreatedOn = existingEntity.CreatedOn;
                    entity.CreatedBy = existingEntity.CreatedBy;
                    entity.ModifiedOn = DateTime.UtcNow;
                    _dbContext.Entry(existingEntity).State = EntityState.Detached;
                    _dbContext.Entry(entity).State = EntityState.Modified;

                    return await _dbContext.SaveChangesAsync() > 0;
                }
            }
            return false;
        }


        private string GenerateEnrollmentNumber()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
        }
    }
}
