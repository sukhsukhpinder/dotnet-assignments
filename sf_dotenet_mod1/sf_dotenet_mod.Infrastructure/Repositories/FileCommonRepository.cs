using Newtonsoft.Json;
using sf_dotenet_mod.Domain.Repositories;

namespace sf_dotenet_mod.Infrastructure.Repositories
{
    public class FileCommonRepository : ICommonRepository
    {
        private readonly string _courseFilePath = Environment.CurrentDirectory + "\\Courses.txt";
        private readonly string _stateFilePath = Environment.CurrentDirectory + "\\States.txt";

        public async Task<List<KeyValuePair<int, string>>> GetAllActiveCourse()
        {
            string data = await File.ReadAllTextAsync(_courseFilePath);
            var courses = JsonConvert.DeserializeObject<List<KeyValuePair<int, string>>>(data);
            return courses ?? ([]);
        }

        public async Task<List<KeyValuePair<int, string>>> GetAllActiveStates()
        {
            string data = await File.ReadAllTextAsync(_stateFilePath);
            var states = JsonConvert.DeserializeObject<List<KeyValuePair<int, string>>>(data);
            return states ?? ([]);
        }
    }
}
