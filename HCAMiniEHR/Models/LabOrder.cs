using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HCAMiniEHR.Models
{
    public class LabOrder
    {
        public int LabOrderId { get; set; }

        [Required]
        public string TestName { get; set; }

        public DateTime OrderedDate { get; set; }

        public string Status { get; set; }

        // Foreign Key
        public int AppointmentId { get; set; }

        [ForeignKey("AppointmentId")]
        public Appointment Appointment { get; set; }
    }
}
