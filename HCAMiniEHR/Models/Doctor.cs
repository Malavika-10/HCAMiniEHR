using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HCAMiniEHR.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }

        [Required]
        [RegularExpression(@"^[A-Za-z\s]+$",
        ErrorMessage = "Full Name must contain only letters.")]
        public string Name { get; set; } = string.Empty;
        [RegularExpression(@"^[A-Za-z\s]+$",
        ErrorMessage = "Full Name must contain only letters.")][Required]

        public string Specialization { get; set; } = string.Empty;

        public ICollection<Appointment> Appointments { get; set; }
            = new List<Appointment>();
    }
}
