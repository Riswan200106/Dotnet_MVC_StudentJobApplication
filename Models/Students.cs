using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentJobApplication.Models
{


    public class Students
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone Number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone Number must be exactly 10 digits.")]
        public string PhoneNumber { get; set; }
        public string JobPosition { get; set; }

        [Display(Name = "Photo")]
        public string PhotoBase64 { get; set; } = null;

        [Display(Name = "Resume")]
        public string ResumeBase64 { get; set; } = null;

        [Display(Name = "Application Date")]
        public DateTime ApplicationDate { get; set; } = DateTime.Now; // Automatically sets the current date and time
    }

}