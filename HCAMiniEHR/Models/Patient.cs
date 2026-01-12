using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HCAMiniEHR.Models
{
    public class Patient
    {
        public int PatientId { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [MaxLength(15)]
        public string Phone { get; set; }

        // Navigation
        public ICollection<Appointment> Appointments { get; set; }
    }
}
