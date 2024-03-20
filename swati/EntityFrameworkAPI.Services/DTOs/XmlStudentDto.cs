﻿using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkAPI.Services.DTOs
{

    public class XmlStudentDto : BaseDto<XmlStudentDto, XmlStudent>
    {
        public Guid StudentId { get; set; }
        [Required]
        public string StudentName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]

        public string Password { get; set; }

        public string PasswordHash { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string Mobile { get; set; }
        public string Gender { get; set; }
        public string Class { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public DateTime JoinedDate { get; set; } = DateTime.UtcNow;
        public string ImagePath { get; set; }
        public string Role { get; set; } = "Student";
    }
}