using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HCAMiniEHR.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string Specialization { get; set; } = string.Empty;

        public ICollection<Appointment> Appointments { get; set; }
            = new List<Appointment>();
    }
}
