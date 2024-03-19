using Microsoft.AspNetCore.Mvc.Rendering;

namespace sf_dotenet_mod.Application.Dtos.Request
{
    public class StudentViewRequest
    {
        public StudentRequest StudentRequest { get; set; }
        public SelectList Courses { get; set; }
        public SelectList States { get; set; }
    }
}
