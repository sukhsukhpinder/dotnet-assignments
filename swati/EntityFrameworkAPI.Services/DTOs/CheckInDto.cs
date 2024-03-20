using EntityFrameworkAPI.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkAPI.Services.DTOs
{
    public class CheckInDto : BaseDto<CheckInDto, StudentCheckIn>
    {
        public int StudentCheckInId { get; set; }

        [Required]
        public int StudentId { get; set; }

        [DataType(DataType.Time)]
        public DateTime CheckOut { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public DateTime CheckIn { get; set; }
    }
}
