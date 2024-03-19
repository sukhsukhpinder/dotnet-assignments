using Microsoft.EntityFrameworkCore;
using sf_dotenet_mod.Domain.Entities;
using sf_dotenet_mod.Domain.Repositories;

namespace sf_dotenet_mod.Infrastructure.Repositories
{
    public class StudentRepository(EnrollDbContext dbContext) : IStudentRepository
    {
        private readonly EnrollDbContext _dbContext = dbContext;

        public async Task<bool> Delete(string id)
        {
            var existingRecord = await _dbContext.Students.FindAsync(Guid.Parse(id));
            if (existingRecord != null)
            {
                _dbContext.Remove(existingRecord);
                return _dbContext.SaveChanges() > 0;
            }
            return false;
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

        public async Task<Student> Create(Student entity)
        {
            entity.EnrollmentNo = GenerateEnrollmentNumber();
            await _dbContext.Students.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<Student>> GetAll()
        {
            var students = await _dbContext.Students
                .Include(s => s.State)
                .Include(c => c.Course)
                .Select(s => new Student
                {
                    StudentId = s.StudentId,
                    EnrollmentNo = s.EnrollmentNo,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    DateOfBirth = s.DateOfBirth,
                    Email = s.Email,
                    State = s.State,
                    Course = s.Course,
                }).ToListAsync();

            return students;
        }

        public async Task<Student?> GetByEnrollmentNo(string enrollmentNo)
        {
            var record = await _dbContext.Students.FirstOrDefaultAsync(x => x.EnrollmentNo == enrollmentNo);
            return record;
        }

        public async Task<Student?> Get(string id)
        {
            return await _dbContext.Students.FindAsync(Guid.Parse(id));
        }

        public async Task<Student> Update(Student entity, string id)
        {
            var existingEntity = await _dbContext.Students.FindAsync(Guid.Parse(id));
            if (existingEntity != null)
            {
                entity.StudentId = existingEntity.StudentId;
                _dbContext.Entry(existingEntity).State = EntityState.Detached;
                _dbContext.Entry(entity).State = EntityState.Modified;

                await _dbContext.SaveChangesAsync();
            }
            return entity;
        }

        public async Task<List<KeyValuePair<string, int>>> GetChartDetails()
        {
            var result = await _dbContext.Students.Include(s => s.State)
                .GroupBy(s => s.State.Name)
                .Select(g => new KeyValuePair<string, int>(g.Key, g.Count()))
                .ToListAsync();

            return result;
        }

        private static string GenerateEnrollmentNumber()
        {
            return Guid.NewGuid().ToString("N")[..6].ToUpper();
        }
    }
}
