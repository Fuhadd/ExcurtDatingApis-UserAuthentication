using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAuthenticationService.Core.DTOs
{
    public class LoginUserDTO
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = String.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = String.Empty;
    }
    public class RegisterEmailDTO
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime CreatedAt => DateTime.UtcNow;
    }

    public class UpdateMobileNumberDTO
    {
        public string? MobileNumberDialCode { get; set; }
        public string? MobileNumber { get; set; }
        public string? Otp { get; set; }
    }

    public class UpdateNameDTO
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }

    public class UpdateBirthDayDTO
    {
        public DateTime DateOfBirth { get; set; }

        public int Age => DateTime.Now.Year - DateOfBirth.Year;
    }

    public class UpdateGenderDTO
    {
        public string Gender { get; set; } = string.Empty;
    }

    public class UpdateInterestDTO
    {
        public string? Interests { get; set; }
    }

    public class ApiUserDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? ProfileImageName { get; set; }
        public string? ProfileImageContentType { get; set; }
        public string? ProfileImageBase64 { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Hash { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; } = DateTime.Now;
        public string Gender { get; set; } = string.Empty;
        public int Age { get; set; }
        public int Likeability { get; set; }
        public string? Bio { get; set; } = string.Empty;
        public string? MobileNumberDialCode { get; set; }
        public string? MobileNumber { get; set; }
        public int OnboardingStep { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Interests { get; set; }
        public virtual List<UserImageDTO>? UserImages { get; set; }


    }

    public class UploadUserImageDTO
    {
        [Required]
        public string Name { get; set; } = String.Empty;
        [Required]
        public string ImageUrl { get; set; } = String.Empty;
        [Required]
        public int ImageSize { get; set; }
    }


}
