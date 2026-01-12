using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HCAMiniEHR.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public string Status { get; set; } = string.Empty;

        // =====================
        // Foreign Keys
        // =====================
        public int PatientId { get; set; }
        public int DoctorId { get; set; }

        // =====================
        // Navigation Properties
        // =====================
        [ForeignKey("PatientId")]
        public Patient Patient { get; set; } = null!;

        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; } = null!;

        public ICollection<LabOrder> LabOrders { get; set; }
            = new List<LabOrder>();
    }
}
